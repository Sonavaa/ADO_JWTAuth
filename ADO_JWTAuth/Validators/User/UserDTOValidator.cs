using ADO_JWTAuth.DTOs;
using FluentValidation;

namespace ADO_JWTAuth.Validators.User
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage("Email is required.")

                .Matches(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.(com|ru|net|org)$")
                .WithMessage("Invalid email format.")

                .Matches(@"^[^@\s]+@(gmail\.com|mail\.ru|hotmail\.com|outlook\.com)$")
                .WithMessage("Only Gmail, Mail.ru, Hotmail or Outlook emails are allowed.");


            RuleFor(u => u.Username)
                .NotEmpty()
                .WithMessage("Username is required.")

                .Matches(@"_")
                .WithMessage("Username must contain '_'.");


            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage("Password is required.")

                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long.")

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