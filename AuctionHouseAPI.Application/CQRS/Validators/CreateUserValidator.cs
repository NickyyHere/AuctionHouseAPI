using AuctionHouseAPI.Application.CQRS.Features.Users.Commands;
using FluentValidation;

namespace AuctionHouseAPI.Application.CQRS.Validators
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.CreateUserDTO.Username).NotEmpty().Length(8, 50);
            RuleFor(x => x.CreateUserDTO.Email).NotEmpty().EmailAddress();
            RuleFor(x => x.CreateUserDTO.Password).NotEmpty().Length(10, 100);
            RuleFor(x => x.CreateUserDTO.FirstName).NotEmpty().Length(2, 100);
            RuleFor(x => x.CreateUserDTO.LastName).NotEmpty().Length(2, 100);
        }
    }
}
