using AuctionHouseAPI.Application.CQRS.Features.Categories.Commands;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Categories.Handlers
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        public CreateCategoryHandler(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request.CreateCategoryDTO);
            return await _categoryService.CreateCategoryAsync(category);
        }
    }
}
