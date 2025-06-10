using AuctionHouseAPI.Application.CQRS.Features.Auctions.Commands;
using FluentValidation;

namespace AuctionHouseAPI.Application.CQRS.Validators
{
    public class CreateAuctionItemValidator : AbstractValidator<CreateAuctionCommand>
    {
        public CreateAuctionItemValidator()
        {
            RuleFor(x => x.CreateAuctionDTO.Item.Name).NotEmpty().Length(3, 255);
            RuleFor(x => x.CreateAuctionDTO.Item.Description).NotEmpty();
            RuleFor(x => x.CreateAuctionDTO.Item.CategoryId).NotEmpty();
        }
    }
}
