using AuctionHouseAPI.Application.CQRS.Features.Categories.Commands;
using FluentValidation;

namespace AuctionHouseAPI.Application.CQRS.Validators
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator()
        {
            RuleFor(x => x.CreateCategoryDTO.Name).NotEmpty().NotNull().Length(2,20);
        }
    }
}
