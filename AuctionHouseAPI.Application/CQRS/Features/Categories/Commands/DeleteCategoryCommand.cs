using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Categories.Commands
{
    public record DeleteCategoryCommand(int categoryId) : IRequest;
}
