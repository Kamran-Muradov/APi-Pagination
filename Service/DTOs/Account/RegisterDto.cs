using FluentValidation;

namespace Service.DTOs.Account
{
    public class RegisterDto
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(m => m.UserName)
                .NotEmpty()
                .WithMessage("User name is required");

            RuleFor(m => m.FullName)
                .NotEmpty()
                .WithMessage("Full name is required");

            RuleFor(m => m.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email format is wrong");

            RuleFor(m => m.Password)
                .NotEmpty()
                .WithMessage("Password is required");
        }
    }
}
