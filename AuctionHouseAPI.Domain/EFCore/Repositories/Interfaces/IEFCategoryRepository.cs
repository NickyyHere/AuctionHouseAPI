using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Domain.EFCore.Repositories.Interfaces
{
    public interface IEFCategoryRepository : IEFCoreRepository<Category>{}
}
