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
        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="createUserDTO">CreateUserDTO; User data</param>
        /// <returns>
        /// int
        /// </returns>
        /// <response code="200">User created</response>
        /// <response code="409">Username or email taken</response>
        /// <response code="500">Internal server error - unknown</response>
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
        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">Integer; User id</param>
        /// <returns>
        /// UserDTO
        /// </returns>
        /// <response code="200">User data sent</response>
        /// <response code="500">Internal server error - unknown</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDTO>> GetUser(int id)
        {
            var query = new GetUserByIdQuery(id);
            var user = await _mediator.Send(query);
            return Ok(user);
        }
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>
        /// UserDTO[]
        /// </returns>
        /// <response code="200">Users data sent</response>
        /// <response code="500">Internal server error - unknown</response>
        [HttpGet]
        public async Task<ActionResult<List<UserDTO>>> GetUsers()
        {
            var query = new GetAllUsersQuery();
            var users = await _mediator.Send(query);
            return Ok(users);
        }
        /// <summary>
        /// Delete user
        /// </summary>
        /// <returns>
        /// UserDTO
        /// </returns>
        /// <response code="204">User deleted</response>
        /// <response code="403">Couldn't verify user identity</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error - unknown</response>
        /// <exception cref="EntityDoesNotExistException">Thrown when entity does not exist in database</exception>
        [HttpDelete, Authorize]
        public async Task<ActionResult> DeleteUser()
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Forbid("Couldn't verify user identity");
            }
            try
            {
                var command = new DeleteUserCommand(userId);
                await _mediator.Send(command);
                return NoContent();
            }
            catch(EntityDoesNotExistException e)
            {
                return NotFound(e.Message);
            }
        }
        /// <summary>
        /// Update user
        /// </summary>
        /// <returns>
        /// UserDTO
        /// </returns>
        /// <response code="204">User updated</response>
        /// <response code="403">Couldn't verify user identity</response>
        /// <response code="404">User not found</response>
        /// <response code="500">Internal server error - unknown</response>
        /// <exception cref="EntityDoesNotExistException">Thrown when entity does not exist in database</exception>
        [HttpPut, Authorize]
        public async Task<ActionResult> EditUser([FromBody] UpdateUserDTO editedUser)
        {
            if (!int.TryParse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Forbid("Couldn't verify user identity");
            }
            try
            {
                var command = new UpdateUserCommand(editedUser, userId);
                await _mediator.Send(command);
                return NoContent();
            }
            catch (EntityDoesNotExistException e)
            {
                return NotFound(e.Message);
            }
        }
    }
}