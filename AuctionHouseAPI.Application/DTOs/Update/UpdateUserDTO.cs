using System.ComponentModel.DataAnnotations;

namespace AuctionHouseAPI.Application.DTOs.Update
{
    public class UpdateUserDTO
    {
        [EmailAddress]
        public string? Email { get; set; }
        [MinLength(10, ErrorMessage = "Password must be at least 10 characters long"), MaxLength(100, ErrorMessage = "Password must be less than 100 characters long")]
        public string? Password { get; set; }
        [MinLength(2, ErrorMessage = "First name must be at least 2 characters long"), MaxLength(100, ErrorMessage = "First name must be less than 100 characters long")]
        public string? FirstName { get; set; }
        [MinLength(2, ErrorMessage = "Last name must be at least 2 characters long"), MaxLength(100, ErrorMessage = "Last name must be less than 100 characters long")]
        public string? LastName { get; set; }
    }
}
