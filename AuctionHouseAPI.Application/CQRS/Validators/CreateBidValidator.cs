using AuctionHouseAPI.Application.CQRS.Features.Bids.Commands;
using FluentValidation;

namespace AuctionHouseAPI.Application.CQRS.Validators
{
    public class CreateBidValidator : AbstractValidator<CreateBidCommand>
    {
        public CreateBidValidator()
        {
            RuleFor(x => x.CreateBidDTO.AuctionId).NotEmpty();
            RuleFor(x => x.CreateBidDTO.Amount).GreaterThanOrEqualTo(1);
            RuleFor(x => x.userId).NotEmpty();
        }
    }
}
