using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Application.Mappers;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;

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
                var newId = await _categoryRepository.CreateAsync(category);
                await _categoryRepository.CommitTransactionAsync();
                return newId;
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
                var category = await _categoryRepository.GetByIdAsync(id) ?? throw new EntityDoesNotExistException($"Category with given id ({id}) does not exist");
                await _categoryRepository.DeleteAsync(category);
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
            var categories = _mapper.ToDTO((List<Category>)await _categoryRepository.GetAllAsync());
            return categories;
        }

        public async Task<CategoryDTO> GetCategory(int id)
        {
            var category = _mapper.ToDTO(await _categoryRepository.GetByIdAsync(id) ?? throw new EntityDoesNotExistException($"Category with given id ({id}) does not exist"));
            return category;
        }

        public async Task UpdateCategory(UpdateCategoryDTO categoryDTO, int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id) ?? throw new EntityDoesNotExistException($"Category with given id ({id}) does not exist");
            await _categoryRepository.BeginTransactionAsync();
            try
            {
                if (!string.IsNullOrWhiteSpace(categoryDTO.Name))
                    category.Name = categoryDTO.Name;
                if (!string.IsNullOrWhiteSpace(categoryDTO.Description))
                    category.Description = categoryDTO.Description;
                await _categoryRepository.UpdateCategoryAsync(category);
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
