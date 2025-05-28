using AuctionHouseAPI.Domain.EFCore.Repositories.Interfaces;
using AuctionHouseAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouseAPI.Domain.EFCore.Repositories
{
    public class EFTagRepository : EFBaseRepository<Tag>, IEFTagRepository
    {
        public EFTagRepository(AppDbContext context) : base(context) { }

        public async Task<Tag?> GetByNameAsync(string name)
        {
            return await _context.Tags
                .FirstOrDefaultAsync(t => t.Name == name);
        }
    }
}
