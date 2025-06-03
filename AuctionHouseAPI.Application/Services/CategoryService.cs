using AuctionHouseAPI.Application.DTOs.Create;
using AuctionHouseAPI.Application.DTOs.Read;
using AuctionHouseAPI.Application.DTOs.Update;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AuctionHouseAPI.Shared.Exceptions;
using AutoMapper;

namespace AuctionHouseAPI.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<int> CreateCategoryAsync(Category category)
        {
            var newId = await _categoryRepository.CreateAsync(category);
            return newId;
        }

        public async Task DeleteCategoryAsync(Category category)
        {
            await _categoryRepository.DeleteAsync(category);
        }
        public async Task UpdateCategoryAsync(Category category, UpdateCategoryDTO categoryDTO)
        {
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
