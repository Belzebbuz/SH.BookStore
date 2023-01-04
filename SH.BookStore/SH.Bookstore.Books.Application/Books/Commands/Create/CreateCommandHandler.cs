using SH.Bookstore.Books.Application.Contracts.Commands;
using SH.Bookstore.Books.Application.Contracts.Repository;
using SH.Bookstore.Books.Application.Specifications.AuthorsSpecifications;
using SH.Bookstore.Books.Application.Specifications.TagsSpecifications;
using SH.Bookstore.Books.Domain.Aggregates.AuthorAggregate;
using SH.Bookstore.Books.Domain.Aggregates.BookAggrgate;
using SH.Bookstore.Books.Domain.Aggregates.TagAggregate;
using SH.Bookstore.Shared.Wrapper;

using Throw;

namespace SH.Bookstore.Books.Application.Books.Commands.Create;
public class CreateCommandHandler : ICommandHandler<CreateCommand>
{
    private readonly IRepository<Book> _bookRepository;
    private readonly IReadRepository<Tag> _tagRepository;
    private readonly IReadRepository<Author> _authorRepository;

    public CreateCommandHandler(IRepository<Book> bookRepository,
                                IReadRepository<Tag> tagsRepository,
                                IReadRepository<Author> authorRepoistory)
    {
        _bookRepository = bookRepository;
        _tagRepository = tagsRepository;
        _authorRepository = authorRepoistory;
    }

    public async Task<IResult> Handle(CreateCommand request, CancellationToken cancellationToken)
    {
        var tags = await _tagRepository.ListAsync(TagsSpec.GetByIds(request.Tags));
        tags.ThrowIfNull().IfCountEquals(0);

        var authors = await _authorRepository.ListAsync(AuthorsSpec.GetByIds(request.Authors));
        authors.ThrowIfNull().IfCountEquals(0);

        var book = Book.Create(request.Title, request.Description, null, tags, authors, request.PriceValue, request.ActualDatePrice);
        await _bookRepository.AddAsync(book);
        return Result.Success();
    }
}
