namespace AuctionHouseAPI.DTOs.Read
{
    public class CategoryDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public CategoryDTO(string name, string description)
        {
            Name = name; 
            Description = description;
        }
    }
}
