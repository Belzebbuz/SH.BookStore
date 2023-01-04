using SH.Bookstore.Shared.Notifications;
using SH.Bookstore.Whs.Host.Mongo.Entites.Base;

namespace SH.Bookstore.Whs.Host.Mongo.Entites.Events;
public static class EntityUpdatedEvent
{
    public static EntityUpdatedEvent<TEntity> WithEntity<TEntity>(TEntity entity)
        where TEntity : IMongoEntity
        => new(entity);
}

public class EntityUpdatedEvent<TEntity> : DomainEvent
    where TEntity : IMongoEntity
{
    internal EntityUpdatedEvent(TEntity entity) => Entity = entity;

    public TEntity Entity { get; }
}