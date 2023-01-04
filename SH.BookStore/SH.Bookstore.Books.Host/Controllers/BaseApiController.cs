using MediatR;

using Microsoft.AspNetCore.Mvc;

namespace SH.Bookstore.Books.Host.Controllers;

[ApiController]
public class BaseApiController : ControllerBase
{
    private ISender _mediator = null!;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

}
