using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouseAPI.Domain.EFCore.Repositories
{
    public class EFUserRepository : EFBaseRepository<User>, IUserRepository
    {
        public EFUserRepository(AppDbContext context) : base(context) { }

        public override async Task<int> CreateAsync(User entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public Task<User?> GetByUsernameAsync(string username)
        {
            return _context.Users
                .FirstOrDefaultAsync(x => x.Username == username);
        }

        public async Task UpdateUserAsync(User user)
        {
            await _context.SaveChangesAsync();
        }
    }
}
