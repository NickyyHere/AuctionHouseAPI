using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Domain.Repositories.interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouseAPI.Domain.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context) { }
        public async Task CreateCategory(Category category)
        {
            await _context.Categories.AddAsync(category);
        }

        public void DeleteCategory(Category category)
        {
            _context.Categories.Remove(category);
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
            return await _context.Categories.FindAsync(id) ?? throw new EntityDoesNotExistException($"Category with given id ({id}) does not exist in database");
        }
    }
}
