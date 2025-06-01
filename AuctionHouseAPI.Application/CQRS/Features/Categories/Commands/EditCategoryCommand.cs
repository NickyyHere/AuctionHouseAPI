using AuctionHouseAPI.Application.DTOs.Update;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Categories.Commands
{
    public record EditCategoryCommand(UpdateCategoryDTO UpdateCategoryDTO, int categoryId) : IRequest;
}
