using SH.Bookstore.Books.Application.Contracts.Commands;

namespace SH.Bookstore.Books.Application.Authors.Commands.Create;
public record CreateAuthorCommand(string Name, string Description) : ICommand;
