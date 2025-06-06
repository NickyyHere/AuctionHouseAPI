
using AuctionHouseAPI.Application.CQRS.Features.Categories.Commands;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuctionHouseAPI.Application.CQRS.Features.Categories.Handlers
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryService _categoryService;
        private readonly ILogger<DeleteCategoryHandler> _logger;
        public DeleteCategoryHandler(ICategoryRepository categoryRepository, ICategoryService categoryService, ILogger<DeleteCategoryHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _categoryService = categoryService;
            _logger = logger;
        }
        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.categoryId)
                ?? throw new EntityDoesNotExistException($"Category with given id ({request.categoryId}) does not exist");
            await _categoryService.DeleteCategoryAsync(category);
            _logger.LogInformation("Category {CategoryName} has been deleted", category.Name);
        }
    }
}
