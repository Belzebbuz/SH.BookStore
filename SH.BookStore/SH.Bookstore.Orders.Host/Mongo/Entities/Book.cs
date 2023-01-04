using MongoDB.Bson.Serialization.Attributes;

namespace SH.Bookstore.Orders.Host.Mongo.Entities;

[BsonIgnoreExtraElements]
public class Book
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
}
