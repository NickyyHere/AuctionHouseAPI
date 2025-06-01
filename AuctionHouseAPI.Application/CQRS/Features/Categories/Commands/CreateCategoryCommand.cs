
using AuctionHouseAPI.Application.DTOs.Create;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Categories.Commands
{
    public record CreateCategoryCommand(CreateCategoryDTO CreateCategoryDTO) : IRequest<int>;
}
