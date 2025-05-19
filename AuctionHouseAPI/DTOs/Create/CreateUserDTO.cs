using System.ComponentModel.DataAnnotations;

namespace AuctionHouseAPI.DTOs.Create
{
#pragma warning disable
    public class CreateUserDTO
    {
        [Required, MinLength(8, ErrorMessage = "Username must be at least 8 characters long"), MaxLength(50, ErrorMessage = "Username must be less than 50 characters long")]
        public string Username { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(10, ErrorMessage = "Password must be at least 10 characters long"), MaxLength(100, ErrorMessage = "Password must be less than 100 characters long")]
        public string Password { get; set; }
        [Required, MinLength(2, ErrorMessage = "First name must be at least 2 characters long"), MaxLength(100, ErrorMessage = "First name must be less than 100 characters long")]
        public string FirstName { get; set; }
        [Required, MinLength(2, ErrorMessage = "Last name must be at least 2 characters long"), MaxLength(100, ErrorMessage = "Last name must be less than 100 characters long")]
        public string LastName { get; set; }
    }
}
