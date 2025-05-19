namespace AuctionHouseAPI.DTOs.Read
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime JoinedDateTime { get; }

        public UserDTO(int id, string email, string firstName, string lastName, DateTime joinedDateTime)
        {
            Id = id;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            JoinedDateTime = joinedDateTime;
        }
    }
}
