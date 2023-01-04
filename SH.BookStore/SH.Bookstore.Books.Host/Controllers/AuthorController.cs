using Microsoft.AspNetCore.Mvc;

using SH.Bookstore.Books.Application.Authors.Commands.Create;
using SH.Bookstore.Books.Application.Authors.Queries;
using SH.Bookstore.Shared.Wrapper;

using IResult = SH.Bookstore.Shared.Wrapper.IResult;

namespace SH.Bookstore.Books.Host.Controllers;

[Route("authors")]
public class AuthorController : BaseApiController
{
    [HttpPost]
    public async Task<IResult> CreateAsync(CreateAuthorCommand command)
        => await Mediator.Send(command);

    [HttpPost("search")]
    public async Task<IResult> SearchAsync(SearchAuthorsQuery query)
        => await Mediator.Send(query);
}
