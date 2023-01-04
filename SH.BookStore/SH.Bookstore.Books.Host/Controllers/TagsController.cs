using Microsoft.AspNetCore.Mvc;

using SH.Bookstore.Books.Application.Tags.Commands.Create;
using SH.Bookstore.Books.Application.Tags.Commands.Delete;
using SH.Bookstore.Books.Application.Tags.DTOs;
using SH.Bookstore.Books.Application.Tags.Queries;
using SH.Bookstore.Shared.Wrapper;

using IResult = SH.Bookstore.Shared.Wrapper.IResult;

namespace SH.Bookstore.Books.Host.Controllers;

[Route("tags")]
public class TagsController : BaseApiController
{
    [HttpPost]
    public async Task<IResult> CreateAsync(CreateTagCommand command)
        => await Mediator.Send(command);

    [HttpPost("search")]
    public async Task<PaginatedResult<TagDto>> SearchAsync(SearchTagQuery query)
        => await Mediator.Send(query);

    [HttpDelete("{tagId}")]
    public async Task<IResult> DeleteAsync(Guid tagId)
        => await Mediator.Send(new DeleteTagCommand(tagId));
}
