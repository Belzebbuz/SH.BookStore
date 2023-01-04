using SH.Bookstore.Books.Domain.Common.Contracts;

namespace SH.Bookstore.Books.Domain.Mongo;

public class MgAuthor : IMongo
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
}