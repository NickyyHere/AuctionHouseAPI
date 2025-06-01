using AuctionHouseAPI.Application.CQRS.Features.Categories.Commands;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Categories.Handlers
{
    public class EditCategoryHandler : IRequestHandler<EditCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryService _categoryService;
        public EditCategoryHandler(ICategoryRepository categoryRepository, ICategoryService categoryService)
        {
            _categoryRepository = categoryRepository;
            _categoryService = categoryService;
        }

        public async Task Handle(EditCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.categoryId)
                ?? throw new EntityDoesNotExistException($"Category with given id ({request.categoryId}) does not exist");
            await _categoryService.UpdateCategoryAsync(category, request.UpdateCategoryDTO);
        }
    }
}
