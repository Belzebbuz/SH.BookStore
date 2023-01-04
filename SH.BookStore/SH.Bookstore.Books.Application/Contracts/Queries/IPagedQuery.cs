using MediatR;

using SH.Bookstore.Shared.Wrapper;

namespace SH.Bookstore.Books.Application.Contracts.Queries;

public interface IPagedQuery<TData> : IQuery, IRequest<PaginatedResult<TData>>
{
}