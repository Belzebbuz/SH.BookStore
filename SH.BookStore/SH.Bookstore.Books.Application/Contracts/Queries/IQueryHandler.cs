using MediatR;

using SH.Bookstore.Shared.Wrapper;

namespace SH.Bookstore.Books.Application.Contracts.Queries;

public interface IQueryHandler<TQuery, TData> 
    : IRequestHandler<TQuery, IResult<TData>>
    where TQuery : IQuery<TData>
{
}
