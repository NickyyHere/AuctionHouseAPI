using AuctionHouseAPI.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuctionHouseAPI.Domain.EFCore.Repositories
{
    public abstract class EFBaseRepository<T> : ITransactionRepository, IBaseRepository<T> where T : class
    {
        protected readonly AppDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        protected EFBaseRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task BeginTransactionAsync()
        {
            if (_currentTransaction == null)
                _currentTransaction = await _context.Database.BeginTransactionAsync();
        }
        public async Task CommitTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _context.SaveChangesAsync();
                await _currentTransaction.CommitAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }
        public async Task RollbackTransactionAsync()
        {
            if (_currentTransaction != null)
            {
                await _currentTransaction.RollbackAsync();
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public abstract Task<int> CreateAsync(T entity);

        public virtual async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}
