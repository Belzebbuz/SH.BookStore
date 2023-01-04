using SH.Bookstore.Books.Application.Contracts.Commands;

namespace SH.Bookstore.Books.Application.Books.Commands.Update;
public record UpdateBookCommand(
    Guid Id,
    string Title,
    string Description,
    IEnumerable<Guid> TagIds,
    IEnumerable<Guid> AuthorIds) : ICommand;
