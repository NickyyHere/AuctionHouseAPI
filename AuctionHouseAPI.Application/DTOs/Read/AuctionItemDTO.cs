namespace AuctionHouseAPI.Application.DTOs.Read
{
    public class AuctionItemDTO
    {
        public int AuctionId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> Tags { get; set; }
        
        public AuctionItemDTO(int auctionId, int categoryId, string name, string description, List<string> tags) 
        {
            AuctionId = auctionId;
            CategoryId = categoryId;
            Name = name;
            Description = description;
            Tags = tags;
        }
    }
}
