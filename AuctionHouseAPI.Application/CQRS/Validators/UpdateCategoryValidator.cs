using AuctionHouseAPI.Application.CQRS.Features.Categories.Commands;
using AuctionHouseAPI.Application.DTOs.Update;
using FluentValidation;

namespace AuctionHouseAPI.Application.CQRS.Validators
{
    class UpdateCategoryValidator : AbstractValidator<EditCategoryCommand>
    {
        public UpdateCategoryValidator()
        {
            RuleFor(x => x.UpdateCategoryDTO.Name).Length(2, 20).When(x => x.UpdateCategoryDTO.Name != null);
        }
    }
}
