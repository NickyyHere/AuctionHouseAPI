using AuctionHouseAPI.Application.DTOs.Read;

namespace AuctionHouseAPI.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> GetUserAuthenticationToken(LoginDTO loginDTO);
    }
}
