using MediatR;

using SH.Bookstore.Shared.Wrapper;

namespace SH.Bookstore.Books.Application.Contracts.Queries;

public interface IPagedQueryHandler<TQuery, TData>
    : IRequestHandler<TQuery, PaginatedResult<TData>>
    where TQuery : IPagedQuery<TData>
{
}