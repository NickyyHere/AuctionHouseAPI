namespace AuctionHouseAPI.Domain.Interfaces
{
    public interface IEFCoreRepository<T>
    {
        public Task SaveChangesAsync();
    }
}
