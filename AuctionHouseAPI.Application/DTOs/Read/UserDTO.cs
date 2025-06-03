namespace AuctionHouseAPI.Application.DTOs.Read
{
    public record UserDTO(int Id, string Email, string FirstName, string LastName, DateTime JoinedDateTime);
}
