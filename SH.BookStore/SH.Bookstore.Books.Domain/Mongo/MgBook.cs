using SH.Bookstore.Books.Domain.Common.Contracts;

namespace SH.Bookstore.Books.Domain.Mongo;
public class MgBook : IMongo
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = Guid.Empty.ToString();
    public DateTime CreatedOn { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; } = decimal.Zero;
    public List<MgAuthor> Authors { get; set; } = default!;
    public List<MgTag> Tags { get; set; } = default!;
}
