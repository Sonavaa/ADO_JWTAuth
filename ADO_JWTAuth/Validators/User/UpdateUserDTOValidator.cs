using ADO_JWTAuth.DTOs;
using FluentValidation;

namespace ADO_JWTAuth.Validators.User
{
    public class UpdateUserDTOValidator : AbstractValidator<UpdateUserDTO>
    {
        public UpdateUserDTOValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .Matches(@"^[a-zA-Z0-9._%+-]+@(gmail\.com|mail\.ru|hotmail\.com|outlook\.com)$")
                .WithMessage("Invalid email format.");

            RuleFor(u => u.Username)
                .NotEmpty()
                .WithMessage("Username is required.")
                .Matches(@"_")
                .WithMessage("Username must contain '_'.");

            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage("Password is required.")
                .MinimumLength(8)
                .WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"\d")
                .WithMessage("Password must contain at least one number.");

            RuleFor(u => u.RoleId)
                .GreaterThan(0)
                .WithMessage("RoleId is required.");
        }
    }
}
