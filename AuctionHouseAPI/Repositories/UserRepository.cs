using AuctionHouseAPI.Exceptions;
using AuctionHouseAPI.Models;
using AuctionHouseAPI.Repositories.interfaces;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouseAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateUser(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id) ?? throw new EntityDoesNotExistException($"User with given id ({id} does not exist in database)");
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task UpdateUser()
        {
            await _context.SaveChangesAsync();
        }
    }
}
