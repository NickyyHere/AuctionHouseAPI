namespace AuctionHouseAPI.DTOs.Read
{
    public class AuctionDTO
    {
        public string OwnerFirstName { get; set; }
        public string OwnerLastName { get; set; }
        public AuctionItemDTO Item { get; set; }
        public AuctionOptionsDTO Options { get; set; }
        public AuctionDTO(string ownerFirstName, string ownerLastName, AuctionItemDTO auctionItemDTO, AuctionOptionsDTO auctionOptionsDTO) 
        {
            OwnerFirstName = ownerFirstName;
            OwnerLastName = ownerLastName;
            Item = auctionItemDTO;
            Options = auctionOptionsDTO;
        }
    }
}
