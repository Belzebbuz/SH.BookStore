using SH.Bookstore.Books.Application.Contracts.Commands;

namespace SH.Bookstore.Books.Application.Books.Commands.AddPrice;
public record AddPriceCommand(Guid BookId, decimal PriceValue, DateTime ActualDate) : ICommand;
