using SH.Bookstore.Books.Application.Contracts.Commands;

namespace SH.Bookstore.Books.Application.Books.Commands.Create;
public class CreateCommand : ICommand
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<Guid> Authors { get; set; }
    public List<Guid> Tags { get; set; }
    public decimal PriceValue { get; set; }
    public DateTime ActualDatePrice { get; set; }
}
