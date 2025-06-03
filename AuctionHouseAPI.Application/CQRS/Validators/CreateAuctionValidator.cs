using AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands;
using FluentValidation;
namespace AuctionHouseAPI.Application.CQRS.Validators
{
    public class CreateAuctionValidator : AbstractValidator<CreateAuctionCommand>
    {
        public CreateAuctionValidator()
        {
            RuleFor(x => x.CreateAuctionDTO.Item).NotNull();
            RuleFor(x => x.CreateAuctionDTO.Options).NotNull();
            RuleFor(x => x.userId).NotEmpty();
        }
    }
}
