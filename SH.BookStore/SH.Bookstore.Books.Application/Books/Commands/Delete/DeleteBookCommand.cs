using SH.Bookstore.Books.Application.Contracts.Commands;
using SH.Bookstore.Books.Application.Contracts.Repository;
using SH.Bookstore.Books.Application.Specifications.BookSpecifications;
using SH.Bookstore.Books.Domain.Aggregates.BookAggrgate;
using SH.Bookstore.Shared.Wrapper;

using Throw;

namespace SH.Bookstore.Books.Application.Books.Commands.Delete;
public record DeleteBookCommand(Guid BookId) : ICommand;

public class DeleteBookCommandHandler : ICommandHandler<DeleteBookCommand>
{
    private readonly IRepository<Book> _repository;

    public DeleteBookCommandHandler(IRepository<Book> repository) => _repository = repository;

    public async Task<IResult> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _repository.SingleOrDefaultAsync(BooksSpec.GetById(request.BookId));
        book.ThrowIfNull($"Book with id: {request.BookId} not found!");
        await _repository.DeleteAsync(book);
        return Result.Success();
    }
}
