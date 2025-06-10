using AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands;
using FluentValidation;

namespace AuctionHouseAPI.Application.CQRS.Validators
{
    public class UpdateAuctionItemValidator : AbstractValidator<UpdateAuctionItemCommand>
    {
        public UpdateAuctionItemValidator()
        {
            RuleFor(x => x.UpdateAuctionItemDTO.Name).Length(3, 255).When(x => x.UpdateAuctionItemDTO.Name != null);
            RuleFor(x => x.auctionId).NotEmpty();
        }
    }
}
