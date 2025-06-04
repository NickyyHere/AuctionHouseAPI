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
        /// <summary>
        /// Get authorization token
        /// </summary>
        /// <param name="loginDTO">LoginDTO; login data</param>
        /// <returns>
        /// { token }
        /// </returns>
        /// <response code="200">Login successful</response>
        /// <response code="404">Resource not found</response>
        /// <response code="500">Internal server error - unknown</response>
        /// <exception cref="EntityDoesNotExistException">Thrown when entity does not exist in database</exception>
        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginDTO loginDTO)
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
