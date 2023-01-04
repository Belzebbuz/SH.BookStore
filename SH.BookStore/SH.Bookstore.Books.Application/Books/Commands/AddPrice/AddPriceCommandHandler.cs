using SH.Bookstore.Books.Application.Contracts.Commands;
using SH.Bookstore.Books.Application.Contracts.Repository;
using SH.Bookstore.Books.Application.Specifications.BookSpecifications;
using SH.Bookstore.Books.Domain.Aggregates.BookAggrgate;
using SH.Bookstore.Shared.Wrapper;

using Throw;

namespace SH.Bookstore.Books.Application.Books.Commands.AddPrice;

public class AddPriceCommandHandler : ICommandHandler<AddPriceCommand>
{
    private readonly IRepository<Book> _repository;

    public AddPriceCommandHandler(IRepository<Book> repository) => _repository = repository;
    public async Task<IResult> Handle(AddPriceCommand request, CancellationToken cancellationToken)
    {
        var book = await _repository.SingleOrDefaultAsync(BooksSpec.GetById(request.BookId));
        book.ThrowIfNull();
        book.AddPrice(request.ActualDate, request.PriceValue);
        await _repository.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}