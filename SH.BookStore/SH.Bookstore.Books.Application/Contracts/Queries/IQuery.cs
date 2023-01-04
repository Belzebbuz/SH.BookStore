using MediatR;

using SH.Bookstore.Shared.Wrapper;

namespace SH.Bookstore.Books.Application.Contracts.Queries;

public interface IQuery { }
public interface IQuery<TData> : IQuery, IRequest<IResult<TData>>
{
}
