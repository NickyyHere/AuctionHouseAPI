using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Domain.Models;

namespace AuctionHouseAPI.Application.Mappers
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
