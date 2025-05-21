using AuctionHouseAPI.DTOs.Create;
using AuctionHouseAPI.DTOs.Read;
using AuctionHouseAPI.DTOs.Update;
using AuctionHouseAPI.Exceptions;
using AuctionHouseAPI.Models;
using AuctionHouseAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Unicode;

namespace AuctionHouseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // POST
        [HttpPost]
        public async Task<ActionResult<int>> CreateUser([FromBody] CreateUserDTO createUserDTO)
        {
            int userId;
            try
            {
                userId = await _userService.CreateUser(createUserDTO);
            }
            catch (DuplicateEntityException e)
            {
                return Conflict(e.Message);
            }
            return Ok(userId);
        }
        // GET
        [HttpGet("{id}")]
        public ActionResult GetUser(int id)
        {
            var user = _userService.GetUserById(id);
            return Ok(user);
        }
        [HttpGet]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await _userService.GetAllUsers();
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
            await _userService.DeleteUser(userId);
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
            await _userService.UpdateUser(editedUser, userId);
            return NoContent();
        }
    }
}