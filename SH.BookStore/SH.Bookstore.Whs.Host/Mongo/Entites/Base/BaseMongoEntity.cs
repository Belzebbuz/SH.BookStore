using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using SH.Bookstore.Shared.Notifications;

namespace SH.Bookstore.Whs.Host.Mongo.Entites.Base;

public abstract class BaseMongoEntity : IMongoEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; private set; }

    [BsonIgnore]
    public List<DomainEvent> DomanEvents { get; private set; } = new();
}

public class DomainEvent : IEvent
{
}