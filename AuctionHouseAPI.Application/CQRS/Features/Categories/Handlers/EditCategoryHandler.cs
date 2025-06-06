using AuctionHouseAPI.Application.CQRS.Features.Categories.Commands;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuctionHouseAPI.Application.CQRS.Features.Categories.Handlers
{
    public class EditCategoryHandler : IRequestHandler<EditCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<EditCategoryHandler> _logger;
        public EditCategoryHandler(ICategoryRepository categoryRepository, ICategoryService categoryService, ILogger<EditCategoryHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _categoryService = categoryService;
            _logger = logger;
        }

        public async Task Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.categoryId)
                ?? throw new EntityDoesNotExistException($"Category with given id ({request.categoryId}) does not exist");
            await _categoryService.UpdateCategoryAsync(category, request.UpdateCategoryDTO);
            _logger.LogInformation("Category {CategoryId} has been edited", category.Id);
        }
    }
}
