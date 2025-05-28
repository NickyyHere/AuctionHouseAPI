using AuctionHouseAPI.Domain.EFCore.Repositories.Interfaces;
using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Domain.EFCore.Repositories
{
    public class EFCategoryRepository : EFBaseRepository<Category>, IEFCategoryRepository
    {
        public EFCategoryRepository(AppDbContext context) : base(context) { }
    }
}
