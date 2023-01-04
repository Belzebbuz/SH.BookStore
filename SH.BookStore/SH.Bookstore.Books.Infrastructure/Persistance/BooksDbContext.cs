using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SH.Bookstore.Books.Infrastructure.Auditing;
using SH.Bookstore.Books.Infrastructure.Persistance.Settings;
using SH.Bookstore.Shared.Common.Services.Serialize;
using SH.Bookstore.Shared.Identity.RequestCurrentUser;
using SH.Bookstore.Books.Domain.Common.Contracts;
using SH.Bookstore.Books.Domain.Aggregates.BookAggrgate;
using SH.Bookstore.Books.Domain.Aggregates.AuthorAggregate;
using SH.Bookstore.Books.Domain.Aggregates.TagAggregate;
using SH.Bookstore.Books.Domain.Aggregates.BookAggrgate.Entites;
using SH.Bookstore.Shared.Notifications;
using SH.Bookstore.Books.Application.Contracts.Services.Mongo;
using Mapster;
using SH.Bookstore.Books.Domain.Mongo;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using SH.Bookstore.Books.Infrastructure.Mongo;

namespace SH.Bookstore.Books.Infrastructure.Persistance;
internal class BooksDbContext : DbContext
{
    private readonly DatabaseSettings _dbSettings;
    private readonly ISerializerService _serializer;
    private readonly ICurrentUser _currentUser;
    private readonly IEventPublisher _eventPublisher;
    private readonly IServiceProvider _serviceProvider;

    public BooksDbContext(
        DbContextOptions<BooksDbContext> options,
        IOptions<DatabaseSettings> dbOptions,
        ISerializerService serializerService,
        ICurrentUser currentUser,
        IEventPublisher eventPublisher,
        IServiceProvider serviceProvider)
        : base(options)
    {
        _dbSettings = dbOptions.Value;
        _serializer = serializerService;
        _currentUser = currentUser;
        _eventPublisher = eventPublisher;
        _serviceProvider = serviceProvider;
    }
    public DbSet<Trail> AuditTrails => Set<Trail>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Price> Prices => Set<Price>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AppendGlobalQueryFilter<ISoftDelete>(s => s.DeletedOn == null);
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseDatabase(_dbSettings.Provider!, _dbSettings.ConnectionString!);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var auditEntries = HandleAuditingBeforeSaveChanges(_currentUser.GetUserId());

        var result = await SaveChangesWithSyncAsync(cancellationToken);
        await HandleAuditingAfterSaveChangesAsync(auditEntries, cancellationToken);

        await SendDomainEventsAsync();

        return result;
    }

    private async Task<int> SaveChangesWithSyncAsync(CancellationToken cancellationToken)
    {
        await using var transaction = Database.BeginTransaction();

        var result = await base.SaveChangesAsync(cancellationToken);

        foreach (var entity in ChangeTracker.Entries<IMongoEntity>().Select(x => x.Entity))
        {
            await _serviceProvider.GetMongoRepository(entity).AddOrUpdateAsync(((IEntity<Guid>)entity).Id);
        }
        transaction.Commit();
        return result;
    }

    private List<AuditTrail> HandleAuditingBeforeSaveChanges(Guid userId)
    {
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userId;
                    entry.Entity.LastModifiedBy = userId;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedOn = DateTime.UtcNow;
                    entry.Entity.LastModifiedBy = userId;
                    break;

                case EntityState.Deleted:
                    if (entry.Entity is ISoftDelete softDelete)
                    {
                        softDelete.DeletedBy = userId;
                        softDelete.DeletedOn = DateTime.UtcNow;
                        entry.State = EntityState.Modified;
                    }

                    break;
            }

        ChangeTracker.DetectChanges();

        var trailEntries = new List<AuditTrail>();
        foreach (var entry in ChangeTracker.Entries<IAuditableEntity>()
                     .Where(e => e.State is EntityState.Added or EntityState.Deleted or EntityState.Modified)
                     .ToList())
        {
            var trailEntry = new AuditTrail(entry, _serializer)
            {
                TableName = entry.Entity.GetType().Name,
                UserId = userId
            };
            trailEntries.Add(trailEntry);
            foreach (var property in entry.Properties)
            {
                if (property.IsTemporary)
                {
                    trailEntry.TemporaryProperties.Add(property);
                    continue;
                }

                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    trailEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        trailEntry.TrailType = TrailType.Create;
                        trailEntry.NewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        trailEntry.TrailType = TrailType.Delete;
                        trailEntry.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified && entry.Entity is ISoftDelete && property.OriginalValue == null && property.CurrentValue != null)
                        {
                            trailEntry.ChangedColumns.Add(propertyName);
                            trailEntry.TrailType = TrailType.Delete;
                            trailEntry.OldValues[propertyName] = property.OriginalValue;
                            trailEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        else if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
                        {
                            trailEntry.ChangedColumns.Add(propertyName);
                            trailEntry.TrailType = TrailType.Update;
                            trailEntry.OldValues[propertyName] = property.OriginalValue;
                            trailEntry.NewValues[propertyName] = property.CurrentValue;
                        }

                        break;
                }
            }
        }

        foreach (var auditEntry in trailEntries.Where(e => !e.HasTemporaryProperties))
            AuditTrails.Add(auditEntry.ToAuditTrail());

        return trailEntries.Where(e => e.HasTemporaryProperties).ToList();
    }
    private Task HandleAuditingAfterSaveChangesAsync(List<AuditTrail> trailEntries, CancellationToken cancellationToken = new())
    {
        if (trailEntries == null || trailEntries.Count == 0)
            return Task.CompletedTask;

        foreach (var entry in trailEntries)
        {
            foreach (var prop in entry.TemporaryProperties)
                if (prop.Metadata.IsPrimaryKey())
                    entry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                else
                    entry.NewValues[prop.Metadata.Name] = prop.CurrentValue;

            AuditTrails.Add(entry.ToAuditTrail());
        }

        return SaveChangesAsync(cancellationToken);
    }
    private async Task SendDomainEventsAsync()
    {
        var entitiesWithEvents = ChangeTracker.Entries<IEntity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Count > 0)
            .ToArray();

        foreach (var entity in entitiesWithEvents)
        {
            var domainEvents = entity.DomainEvents.ToArray();
            entity.DomainEvents.Clear();
            foreach (var domainEvent in domainEvents)
                await _eventPublisher.PublishAsync(domainEvent);
        }
    }
}
