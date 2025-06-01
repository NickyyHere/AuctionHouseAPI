using AuctionHouseAPI.Application.CQRS.Features.Auth.Queries;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouseAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            string token;
            try
            {
                var query = new GetAuthTokenQuery(loginDTO);
                token = await _mediator.Send(query);
            }
            catch (EntityDoesNotExistException)
            {
                return NotFound("Invalid username or password");
            }
            return Ok(new { token });
        }
    }
}
