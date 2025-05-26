namespace AuctionHouseAPI.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<AuctionItem> AuctionItems { get; set; } = new List<AuctionItem>();
        public Category() 
        {
            Name = string.Empty;
            Description = string.Empty;
        }
        public Category(string name, string description)
        {
            Name = name; 
            Description = description;
        }
    }
}
