using AuctionHouseAPI.Application.DTOs.Read;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Categories.Queries
{
    public record GetCategoryByIdQuery(int id) : IRequest<CategoryDTO>;

}
