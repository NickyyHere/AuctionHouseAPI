using AuctionHouseAPI.Application.CQRS.Features.Categories.Commands;
using AuctionHouseAPI.Application.Services.Interfaces;
using AuctionHouseAPI.Domain.Models;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuctionHouseAPI.Application.CQRS.Features.Categories.Handlers
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, int>
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCategoryHandler> _logger;
        public CreateCategoryHandler(ICategoryService categoryService, IMapper mapper, ILogger<CreateCategoryHandler> logger)
        {
            _categoryService = categoryService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request.CreateCategoryDTO);
            _logger.LogInformation("Category {CategoryName} has been created", category.Name);
            return await _categoryService.CreateCategoryAsync(category);
        }
    }
}
