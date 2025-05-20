using AuctionHouseAPI.DTOs.Create;
using AuctionHouseAPI.DTOs.Read;
using AuctionHouseAPI.DTOs.Update;
using AuctionHouseAPI.Models;
using AuctionHouseAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpPost]
        public async Task<ActionResult<int>> CreateUser([FromBody] CreateUserDTO createUserDTO)
        {
            var userId = await _userService.CreateUser(createUserDTO);
            return Ok(userId);
        }
        [HttpPut("{id}"), Authorize]
        public async Task<ActionResult> EditUser(int id, [FromBody] UpdateUserDTO editedUser)
        {
            await _userService.UpdateUser(editedUser, id);
            return NoContent();
        }
        [HttpDelete("{id}"), Authorize]
        public async Task<ActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUser(id);
            return NoContent();
        }
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
    }
}