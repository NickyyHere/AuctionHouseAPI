using AuctionHouseAPI.Domain.EFCore.Repositories.Interfaces;
using AuctionHouseAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouseAPI.Domain.EFCore.Repositories
{
    public class EFUserRepository : EFBaseRepository<User>, IEFUserRepository
    {
        public EFUserRepository(AppDbContext context) : base(context) { }

        public Task<User?> GetByUsernameAsync(string username)
        {
            return _context.Users
                .FirstOrDefaultAsync(x => x.Username == username);
        }
    }
}
