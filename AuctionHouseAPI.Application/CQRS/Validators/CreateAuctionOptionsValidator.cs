using AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands;
using FluentValidation;

namespace AuctionHouseAPI.Application.CQRS.Validators
{
    public class CreateAuctionOptionsValidator : AbstractValidator<CreateAuctionCommand>
    {
        public CreateAuctionOptionsValidator()
        {
            RuleFor(x => x.CreateAuctionDTO.Options.StartingPrice).NotEmpty().GreaterThanOrEqualTo(1);
            RuleFor(x => x.CreateAuctionDTO.Options.StartDateTime).GreaterThanOrEqualTo(DateTime.Now).When(x => x.CreateAuctionDTO.Options.StartDateTime != null);
            RuleFor(x => x.CreateAuctionDTO.Options.FinishDateTime).NotEmpty().GreaterThan(x => x.CreateAuctionDTO.Options.StartDateTime.HasValue ? x.CreateAuctionDTO.Options.StartDateTime.Value.AddDays(1) : DateTime.Now.AddDays(1));
            RuleFor(x => x.CreateAuctionDTO.Options.MinimumOutbid).NotEmpty().GreaterThanOrEqualTo(5);
            RuleFor(x => x.CreateAuctionDTO.Options.BuyItNowPrice).GreaterThanOrEqualTo(x => x.CreateAuctionDTO.Options.StartingPrice);
        }
    }
}
