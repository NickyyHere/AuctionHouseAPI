using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouseAPI.Presentation.Controllers
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
            string token;
            try
            {
                token = await _authService.GetUserAuthenticationToken(loginDTO);
            }
            catch (EntityDoesNotExistException)
            {
                return Unauthorized("Invalid username or password");
            }
            return Ok(new { token });
        }
    }
}
