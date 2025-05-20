using AuctionHouseAPI.DTOs.Create;
using AuctionHouseAPI.DTOs.Read;
using AuctionHouseAPI.Models;

namespace AuctionHouseAPI.Mappers
{
    public class CategoryMapper : IMapper<CategoryDTO, CreateCategoryDTO, Category>
    {
        public CategoryDTO ToDTO(Category entity)
        {
            return new CategoryDTO(entity.Name, entity.Description);
        }

        public List<CategoryDTO> ToDTO(List<Category> entities)
        {
            var DTOs = new List<CategoryDTO>();
            foreach (var entity in entities)
            {
                DTOs.Add(ToDTO(entity));
            }
            return DTOs;
        }

        public Category ToEntity(CreateCategoryDTO create_dto)
        {
            return new Category(create_dto.Name, create_dto.Description);
        }
    }
}
