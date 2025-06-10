using AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands;
using FluentValidation;

namespace AuctionHouseAPI.Application.CQRS.Validators
{
    public class UpdateAuctionOptionsValidator : AbstractValidator<UpdateAuctionOptionsCommand>
    {
        public UpdateAuctionOptionsValidator()
        {
            RuleFor(x => x.UpdateAuctionOptionsDTO.StartingPrice).GreaterThanOrEqualTo(1).When(x => x.UpdateAuctionOptionsDTO.StartingPrice != null);
            RuleFor(x => x.UpdateAuctionOptionsDTO.StartDateTime).GreaterThanOrEqualTo(DateTime.Now).When(x => x.UpdateAuctionOptionsDTO.StartDateTime != null);
            RuleFor(x => x.UpdateAuctionOptionsDTO.FinishDateTime).GreaterThan(x => x.UpdateAuctionOptionsDTO.StartDateTime.HasValue ? x.UpdateAuctionOptionsDTO.StartDateTime.Value.AddDays(1) : DateTime.Now.AddDays(1)).When(x => x.UpdateAuctionOptionsDTO.FinishDateTime != null);
            RuleFor(x => x.UpdateAuctionOptionsDTO.MinimumOutbid).GreaterThanOrEqualTo(5).When(x => x.UpdateAuctionOptionsDTO.MinimumOutbid != null);
            RuleFor(x => x.UpdateAuctionOptionsDTO.BuyItNowPrice).GreaterThanOrEqualTo(x => x.UpdateAuctionOptionsDTO.StartingPrice).When(x => x.UpdateAuctionOptionsDTO.BuyItNowPrice != null && x.UpdateAuctionOptionsDTO.StartingPrice != null);
        }
    }
}
