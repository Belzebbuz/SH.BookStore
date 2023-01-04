using SH.Bookstore.Identity.Infrastructure.Services.Messages;
using SH.Bookstore.Shared.Common.DI;
using SH.Bookstore.Shared.Wrapper;

namespace Application.Contracts.Services.Identity;

public interface ITokenService : IScopedService
{
    Task<IResult<TokenResponse>> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken);
}
