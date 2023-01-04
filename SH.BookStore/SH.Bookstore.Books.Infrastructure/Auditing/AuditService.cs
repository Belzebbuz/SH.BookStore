using Mapster;

using Microsoft.EntityFrameworkCore;

using SH.Bookstore.Books.Application.Contracts.Services.Audit;
using SH.Bookstore.Books.Infrastructure.Persistance;

namespace SH.Bookstore.Books.Infrastructure.Auditing;
internal class AuditService : IAuditService
{
    private readonly BooksDbContext _context;

    public AuditService(BooksDbContext context) => _context = context;

    public async Task<List<AuditDto>> GetUserTrailsAsync(Guid userId)
    {
        var trails = await _context.AuditTrails
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.DateTime)
            .Take(250)
            .ToListAsync();

        return trails.Adapt<List<AuditDto>>();
    }
}
