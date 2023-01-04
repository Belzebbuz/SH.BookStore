using SH.Bookstore.Books.Domain.Common.Contracts;

namespace SH.Bookstore.Books.Domain.Mongo;

public class MgTag : IMongo
{
    public Guid Id { get; set; }
    public string Title { get; set; }
}