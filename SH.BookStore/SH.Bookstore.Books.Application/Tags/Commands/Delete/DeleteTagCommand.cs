using SH.Bookstore.Books.Application.Contracts.Commands;
using SH.Bookstore.Books.Application.Contracts.Repository;
using SH.Bookstore.Books.Application.Specifications.TagsSpecifications;
using SH.Bookstore.Books.Domain.Aggregates.TagAggregate;
using SH.Bookstore.Shared.Wrapper;

using Throw;

namespace SH.Bookstore.Books.Application.Tags.Commands.Delete;
public record DeleteTagCommand(Guid Id) : ICommand;

public class DeleteTagCommandHandler : ICommandHandler<DeleteTagCommand>
{
    private readonly IRepository<Tag> _repository;

    public DeleteTagCommandHandler(IRepository<Tag> repository) => _repository = repository;
    public async Task<IResult> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        var tag = await _repository.SingleOrDefaultAsync(TagsSpec.GetById(request.Id));
        tag.ThrowIfNull($"Tag with id: {request.Id} not found!");
        await _repository.DeleteAsync(tag);
        return Result.Success();
    }
}