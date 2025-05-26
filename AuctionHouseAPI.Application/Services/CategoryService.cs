using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Application.Mappers;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Domain.Repositories.interfaces;

namespace AuctionHouseAPI.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper<CategoryDTO, CreateCategoryDTO, Category> _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IMapper<CategoryDTO, CreateCategoryDTO, Category> mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateCategory(CreateCategoryDTO categoryDTO)
        {
            await _categoryRepository.BeginTransactionAsync();
            try
            {
                var category = _mapper.ToEntity(categoryDTO);
                await _categoryRepository.CreateCategory(category);
                await _categoryRepository.CommitTransactionAsync();
                return category.Id;
            }
            catch
            {
                await _categoryRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task DeleteCategory(int id)
        {
            await _categoryRepository.BeginTransactionAsync();
            try
            {
                var category = await _categoryRepository.GetCategoryById(id);
                _categoryRepository.DeleteCategory(category);
                await _categoryRepository.CommitTransactionAsync();
            }
            catch
            {
                await _categoryRepository.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<List<CategoryDTO>> GetAllCategories()
        {
            var categories = _mapper.ToDTO(await _categoryRepository.GetCategories());
            return categories;
        }

        public async Task<CategoryDTO> GetCategory(int id)
        {
            var category = _mapper.ToDTO(await _categoryRepository.GetCategoryById(id));
            return category;
        }

        public async Task UpdateCategory(UpdateCategoryDTO categoryDTO, int id)
        {
            await _categoryRepository.BeginTransactionAsync();
            try
            {
                var category = await _categoryRepository.GetCategoryById(id);
                if (!string.IsNullOrWhiteSpace(categoryDTO.Name))
                    category.Name = categoryDTO.Name;
                if (!string.IsNullOrWhiteSpace(categoryDTO.Description))
                    category.Description = categoryDTO.Description;
                await _categoryRepository.CommitTransactionAsync();
            }
            catch
            {
                await _categoryRepository.RollbackTransactionAsync();
                throw;
            }
        }
    }
}
