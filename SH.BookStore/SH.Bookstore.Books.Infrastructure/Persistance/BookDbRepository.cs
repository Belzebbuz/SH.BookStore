using Ardalis.Specification.EntityFrameworkCore;
using Ardalis.Specification;
using Mapster;
using SH.Bookstore.Books.Domain.Common.Contracts;
using SH.Bookstore.Books.Application.Contracts.Repository;

namespace SH.Bookstore.Books.Infrastructure.Persistance;
internal class BookDbRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    public BookDbRepository(BooksDbContext dbContext) : base(dbContext)
    {
    }
    protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
        ApplySpecification(specification, false)
            .ProjectToType<TResult>();
}
