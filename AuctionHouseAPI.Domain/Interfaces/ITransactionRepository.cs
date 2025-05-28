namespace AuctionHouseAPI.Domain.Interfaces
{
    public interface ITransactionRepository<T> : IBaseRepository<T>
    {
        public Task BeginTransactionAsync();
        public Task CommitTransactionAsync();
        public Task RollbackTransactionAsync();
    }
}
