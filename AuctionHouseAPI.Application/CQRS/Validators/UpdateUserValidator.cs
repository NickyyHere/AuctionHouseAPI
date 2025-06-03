using AuctionHouseAPI.Application.CQRS.Features.Users.Commands;
using AuctionHouseAPI.Application.DTOs.Update;
using FluentValidation;

namespace AuctionHouseAPI.Application.CQRS.Validators
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.updateUserDTO.Email).EmailAddress().When(x => x.updateUserDTO.Email != null);
            RuleFor(x => x.updateUserDTO.Password).Length(10, 100).When(x => x.updateUserDTO.Password != null);
            RuleFor(x => x.updateUserDTO.FirstName).Length(2, 100).When(x => x.updateUserDTO.FirstName != null);
            RuleFor(x => x.updateUserDTO.LastName).Length(2, 100).When(x => x.updateUserDTO.LastName != null);
        }
    }
}
