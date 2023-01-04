using MediatR;

using SH.Bookstore.Shared.Wrapper;

namespace SH.Bookstore.Books.Application.Contracts.Commands;
public interface ICommand : IRequest<IResult>
{
}
