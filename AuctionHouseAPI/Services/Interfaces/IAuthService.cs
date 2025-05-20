using AuctionHouseAPI.DTOs.Read;

namespace AuctionHouseAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> GetUserAuthenticationToken(LoginDTO loginDTO);
    }
}
