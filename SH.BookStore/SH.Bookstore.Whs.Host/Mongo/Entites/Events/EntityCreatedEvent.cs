using SH.Bookstore.Shared.Notifications;
using SH.Bookstore.Whs.Host.Mongo.Entites.Base;

namespace SH.Bookstore.Whs.Host.Mongo.Entites.Events;
public static class EntityCreatedEvent
{
    public static EntityCreatedEvent<TEntity> WithEntity<TEntity>(TEntity entity)
       where TEntity : IMongoEntity
       => new(entity);
}
public class EntityCreatedEvent<TEntity> : DomainEvent
    where TEntity : IMongoEntity
{
    internal EntityCreatedEvent(TEntity entity) => Entity = entity;

    public TEntity Entity { get; }
}
