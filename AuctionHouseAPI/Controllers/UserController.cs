using AuctionHouseAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace AuctionHouseAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private List<User> Users { get; set; } = new();
        public UserController() { }
        /// <summary>
        /// Create new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Status code</returns>
        [HttpPost]
        public ActionResult CreateUser([FromBody] User user)
        {
            Users.Add(user);
            return Created();
        }
        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>List of users</returns>
        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            return Ok(Users);
        }
        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>One user</returns>
        [HttpGet("{id}")]
        public ActionResult GetUser(int id) 
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        /// <summary>
        /// Edit user by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="editedUser"></param>
        /// <returns>Status code</returns>
        [HttpPut("{id}")]
        public ActionResult EditUser(int id, [FromBody] User editedUser)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            user.Email = editedUser.Email;
            user.FirstName = editedUser.FirstName;
            user.LastName = editedUser.LastName;
            user.Password = editedUser.Password;
            return NoContent();
        }
        /// <summary>
        /// Delete user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status code</returns>
        [HttpDelete]
        public ActionResult DeleteUser(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            Users.Remove(user);
            return NoContent();
        }
    }
}
