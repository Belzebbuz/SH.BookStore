using Ardalis.Specification;

using SH.Bookstore.Books.Domain.Common.Contracts;

namespace SH.Bookstore.Books.Application.Contracts.Repository;

/// <inheritdoc/>
public interface IRepository<T> : IRepositoryBase<T>
    where T : class, IAggregateRoot
{
}
/// <inheritdoc/>
public interface IReadRepository<T> : IReadRepositoryBase<T>
    where T : class, IAggregateRoot
{
}