using AuctionHouseAPI.DTOs.Read;
using AuctionHouseAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService) 
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var token = await _authService.GetUserAuthenticationToken(loginDTO);
            return Ok(token);
        }
    }
}
