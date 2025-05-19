namespace AuctionHouseAPI.Models
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
        public ICollection<Bid> Bids { get; set; }
        public ICollection<Auction> Auctions { get; set; }

        public User(string username, string email, string password, string firstName, string lastName)
        {
            Username = username;
            Email = email;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            JoinedDateTime = DateTime.Now;
            Bids = new List<Bid>();
            Auctions = new List<Auction>();
        }
    }
}
