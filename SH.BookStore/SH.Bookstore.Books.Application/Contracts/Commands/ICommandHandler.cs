using MediatR;

using SH.Bookstore.Shared.Wrapper;

namespace SH.Bookstore.Books.Application.Contracts.Commands;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, IResult>
    where TCommand : ICommand
{
}