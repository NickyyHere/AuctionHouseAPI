using System.ComponentModel.DataAnnotations;

namespace AuctionHouseAPI.DTOs.Create
{
#pragma warning disable
    public class CreateCategoryDTO
    {
        [Required, MinLength(2, ErrorMessage = "Category name must be at least 2 characters long"), MaxLength(20, ErrorMessage = "Category name must be less than 20 characters long")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
