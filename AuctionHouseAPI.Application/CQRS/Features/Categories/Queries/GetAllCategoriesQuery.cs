using AuctionHouseAPI.Application.DTOs.Read;
using MediatR;

namespace AuctionHouseAPI.Application.CQRS.Features.Categories.Queries
{
    public record GetAllCategoriesQuery() : IRequest<List<CategoryDTO>>;
}
