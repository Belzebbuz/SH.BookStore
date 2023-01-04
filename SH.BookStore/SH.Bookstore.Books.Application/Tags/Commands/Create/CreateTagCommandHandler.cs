using SH.Bookstore.Books.Application.Contracts.Commands;
using SH.Bookstore.Books.Application.Contracts.Repository;
using SH.Bookstore.Books.Application.Specifications.TagsSpecifications;
using SH.Bookstore.Books.Domain.Aggregates.TagAggregate;
using SH.Bookstore.Shared.Wrapper;

using Throw;

namespace SH.Bookstore.Books.Application.Tags.Commands.Create;

public class CreateTagCommandHandler : ICommandHandler<CreateTagCommand>
{
    private readonly IRepository<Tag> _repository;

    public CreateTagCommandHandler(IRepository<Tag> repository)
    {
        _repository = repository;
    }
    public async Task<IResult> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repository.AnyAsync(TagsSpec.GetByTitle(request.Title));
        exists.Throw($"Tag with name: {request.Title} already exists!").IfTrue(exists);
        var tag = Tag.Create(request.Title);
        await _repository.AddAsync(tag);
        return Result.Success();
    }
}
