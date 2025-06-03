namespace AuctionHouseAPI.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        public Task BeginTransactionAsync();
        public Task CommitTransactionAsync();
        public Task RollbackTransactionAsync();
    }
}
