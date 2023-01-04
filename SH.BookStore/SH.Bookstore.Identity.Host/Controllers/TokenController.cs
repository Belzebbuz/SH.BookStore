using Application.Contracts.Services.Identity;

using Microsoft.AspNetCore.Mvc;

using SH.Bookstore.Identity.Infrastructure.Services.Messages;
using SH.Bookstore.Shared.Wrapper;

namespace SH.Bookstore.Identity.Host.Controllers;

[Route("token")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public TokenController(ITokenService tokenService) => _tokenService = tokenService;

    [HttpPost]
    public async Task<IResult<TokenResponse>> GetTokenAsync(TokenRequest request, CancellationToken cancellationToken)
        => await _tokenService.GetTokenAsync(request, GetIpAddress(), cancellationToken);

    private string GetIpAddress() =>
       Request.Headers.ContainsKey("X-Forwarded-For")
           ? Request.Headers["X-Forwarded-For"]
           : HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString() ?? "N/A";
}
