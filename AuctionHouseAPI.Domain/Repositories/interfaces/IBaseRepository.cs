using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseAPI.Domain.Repositories.interfaces
{
    public interface IBaseRepository
    {
        public Task SaveChangesAsync();
        public Task BeginTransactionAsync();
        public Task CommitTransactionAsync();
        public Task RollbackTransactionAsync();
    }
}
