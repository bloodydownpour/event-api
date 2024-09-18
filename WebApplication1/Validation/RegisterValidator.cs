using FluentValidation;
using FluentValidation.Validators;
using WebApplication1.Structure.Data;
namespace WebApplication1.Validation
{
    public class RegisterValidator : AbstractValidator<User>
    {
        public RegisterValidator() {
            
            RuleFor(x => x._Email)
                    .NotEmpty().WithMessage("Email is required.")
                    .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(x => x._Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
                
                }
    }
}
