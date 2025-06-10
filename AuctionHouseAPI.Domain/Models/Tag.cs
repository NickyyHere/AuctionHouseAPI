namespace AuctionHouseAPI.Domain.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<AuctionItemTag> AuctionItems { get; set; } = new List<AuctionItemTag>();

        public Tag()
        {
            Name = string.Empty;
        }
        public Tag(string name)
        {
            Name = name;
        }
    }
}
