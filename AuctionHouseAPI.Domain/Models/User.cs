using AuctionHouseAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionHouseAPI.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime JoinedDateTime { get; }
        public ICollection<Bid> Bids { get; set; } = new List<Bid>();
        public ICollection<Auction> Auctions { get; set; } = new List<Auction>();
        public UserRole Role { get; set; }

        public User()
        {
            Username = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            JoinedDateTime = DateTime.UtcNow;
            Role = UserRole.ROLE_USER;
        }
        public User(string username, string email, string password, string firstName, string lastName)
        {
            Username = username;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            JoinedDateTime = DateTime.UtcNow;
        }
    }
}
