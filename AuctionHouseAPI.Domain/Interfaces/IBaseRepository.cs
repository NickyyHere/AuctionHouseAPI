namespace AuctionHouseAPI.Domain.Interfaces
{
    public interface IBaseRepository<T>
    {
        public Task<int> CreateAsync(T entity);
        public Task DeleteAsync(T entity);
        public Task<T?> GetByIdAsync(int id);
        public Task<IEnumerable<T>> GetAllAsync();
    }
}
