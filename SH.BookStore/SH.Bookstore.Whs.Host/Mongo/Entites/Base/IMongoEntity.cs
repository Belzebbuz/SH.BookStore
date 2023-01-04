using SH.Bookstore.Shared.Notifications;

namespace SH.Bookstore.Whs.Host.Mongo.Entites.Base;

public interface IMongoEntity
{
    public string Id { get; }
    public List<DomainEvent> DomanEvents { get;}
}
