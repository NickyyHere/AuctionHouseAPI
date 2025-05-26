using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Domain.Repositories.interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouseAPI.Domain.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context) { }
        public async Task CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public void DeleteUser(User user)
        {
            _context.Users.Remove(user);
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id) ?? throw new EntityDoesNotExistException($"User with given id ({id}) does not exist in database");
        }

        public async Task<User> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username) ?? throw new EntityDoesNotExistException($"User with given username ({username}) does not exist in database");
        }
        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
