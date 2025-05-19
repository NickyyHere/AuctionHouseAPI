namespace AuctionHouseAPI.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<AuctionItemTag> AuctionItems { get; set; }

        public Tag(string name)
        {
            Name = name;
            AuctionItems = new List<AuctionItemTag>();
        }
    }
}
