using SH.Bookstore.Books.Application.Contracts.Commands;

namespace SH.Bookstore.Books.Application.Tags.Commands.Create;
public record CreateTagCommand(string Title) : ICommand;
