using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using SH.Bookstore.Books.Application.Books.Commands.AddPrice;
using SH.Bookstore.Books.Application.Books.Commands.Create;
using SH.Bookstore.Books.Application.Books.Commands.Delete;
using SH.Bookstore.Books.Application.Books.Queries;
using SH.Bookstore.Books.Domain.Mongo;
using SH.Bookstore.Shared.Wrapper;

using IResult = SH.Bookstore.Shared.Wrapper.IResult;

namespace SH.Bookstore.Books.Host.Controllers;

[Route("books")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class BookController : BaseApiController
{
    [HttpPost]
    public async Task<IResult> CreateAsync(CreateCommand createCommand)
        => await Mediator.Send(createCommand);

    [HttpDelete("{bookId}")]
    public async Task<IResult> DeleteAsync(Guid bookId)
        => await Mediator.Send(new DeleteBookCommand(bookId));

    [HttpPost("search")]
    public async Task<PaginatedResult<MgBook>> SearchAsync(SearchBooksQuery searchBooksQuery)
        => await Mediator.Send(searchBooksQuery);

    [HttpPost("addPrice")]
    public async Task<Shared.Wrapper.IResult> SearchAsync(AddPriceCommand addPriceCommand)
        => await Mediator.Send(addPriceCommand);
}
