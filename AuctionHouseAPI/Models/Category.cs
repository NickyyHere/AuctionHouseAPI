﻿namespace AuctionHouseAPI.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<AuctionItem> AuctionItems { get; set; }

        public Category(string name, string description)
        {
            Name = name; 
            Description = description;
            AuctionItems = new List<AuctionItem>();
        }
    }
}
