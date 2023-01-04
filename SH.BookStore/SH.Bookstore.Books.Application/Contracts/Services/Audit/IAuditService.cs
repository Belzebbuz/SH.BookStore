namespace SH.Bookstore.Books.Application.Contracts.Services.Audit;

public interface IAuditService
{
    Task<List<AuditDto>> GetUserTrailsAsync(Guid userId);
}
