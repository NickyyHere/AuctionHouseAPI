using AuctionHouseAPI.Application.CQRS.Features.Users.Commands;
using AuctionHouseAPI.Application.CQRS.Features.Users.Queries;
using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AuctionHouseAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // POST
        [HttpPost]
        public async Task<ActionResult<int>> CreateUser([FromBody] CreateUserDTO createUserDTO)
        {
            int userId;
            try
            {
                var command = new CreateUserCommand(createUserDTO);
                userId = await _mediator.Send(command);
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }
            return Ok(userId);
        }
        // GET
        [HttpGet("{id}")]
        public async Task<ActionResult> GetUser(int id)
        {
            var query = new GetUserByIdQuery(id);
            var user = await _mediator.Send(query);
            return Ok(user);
        }
        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            var query = new GetAllUsersQuery();
            var users = await _mediator.Send(query);
            return Ok(users);
        }
        // DELETE
        [HttpDelete, Authorize]
        public async Task<ActionResult> DeleteUser()
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Problem();
            }
            var command = new DeleteUserCommand(userId);
            await _mediator.Send(command);
            return NoContent();
        }
        // PUT
        [HttpPut, Authorize]
        public async Task<ActionResult> EditUser([FromBody] UpdateUserDTO editedUser)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Problem();
            }
            var command = new UpdateUserCommand(editedUser, userId);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}