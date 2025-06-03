
using AuctionHouseAPI.Application.CQRS.Features.Categories.Commands;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Shared.Exceptions;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Categories.Handlers
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICategoryService _categoryService;
        public DeleteCategoryHandler(ICategoryRepository categoryRepository, ICategoryService categoryService)
        {
            _categoryRepository = categoryRepository;
            _categoryService = categoryService;
        }
        public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.categoryId)
                ?? throw new EntityDoesNotExistException($"Category with given id ({request.categoryId}) does not exist");
            await _categoryService.DeleteCategoryAsync(category);
        }
    }
}
