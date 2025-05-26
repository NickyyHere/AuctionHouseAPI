namespace AuctionHouseAPI.Application.DTOs.Read
{
    public class AuctionDTO
    {
        public int Id { get; set; }
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public AuctionItemDTO Item { get; set; }
        public AuctionOptionsDTO Options { get; set; }
        public AuctionDTO(int id, string ownerFirstName, string ownerLastName, AuctionItemDTO auctionItemDTO, AuctionOptionsDTO auctionOptionsDTO) 
        {
            Id = id;
            OwnerFirstName = ownerFirstName;
            OwnerLastName = ownerLastName;
            Item = auctionItemDTO;
            Options = auctionOptionsDTO;
        }
        public override bool Equals(object? obj)
        {
            if (obj is not AuctionDTO other) 
                return false;

            return Id == other.Id;
        }
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
