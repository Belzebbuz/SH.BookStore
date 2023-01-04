using SH.Bookstore.Books.Application.Contracts.Commands;
using SH.Bookstore.Books.Application.Contracts.Repository;
using SH.Bookstore.Books.Domain.Aggregates.AuthorAggregate;
using SH.Bookstore.Shared.Wrapper;

namespace SH.Bookstore.Books.Application.Authors.Commands.Create;

public class CreateAuthorCommandHandler : ICommandHandler<CreateAuthorCommand>
{
    private readonly IRepository<Author> _repository;

    public CreateAuthorCommandHandler(IRepository<Author> repository) => _repository = repository;
    public async Task<IResult> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = Author.Create(request.Name, request.Description);
        await _repository.AddAsync(author);
        return Result.Success();
    }
}