using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseAPI.Domain.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly AppDbContext _context;
        private IDbContextTransaction? _currentTransaction;

        protected BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
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
    }
}
