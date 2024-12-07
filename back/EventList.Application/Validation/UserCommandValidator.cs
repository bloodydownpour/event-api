using EventList.Domain.CommandData;
using FluentValidation;

namespace EventList.Application.Validation
{
    public class ToggleAdminCommandValidator : AbstractValidator<ToggleAdminCommandData>
    {
        public ToggleAdminCommandValidator() {
            {
                RuleFor(command => command.User).NotNull().WithMessage("User cannot be null");
                RuleFor(command => command.User.UserId).NotEmpty().WithMessage("User ID cannot be empty");
            }
        }
    }
    public class UpdateUserPfpCommandValidator : AbstractValidator<UpdateUserPfpCommandData>
    {
        public UpdateUserPfpCommandValidator()
        {
            RuleFor(command => command.User).NotNull().WithMessage("User cannot be null");
            RuleFor(command => command.User.UserId).NotEmpty().WithMessage("User ID cannot be empty");
            RuleFor(command => command.FileName).NotEmpty().WithMessage("File name cannot be empty");
        }
    }
    public class RegisterUserInEventCommandValidator : AbstractValidator<RegisterUserInEventCommandData>
    {
        public RegisterUserInEventCommandValidator()
        {
            RuleFor(command => command.EventId).NotEmpty().WithMessage("Event ID cannot be empty");
            RuleFor(command => command.UserId).NotEmpty().WithMessage("User ID cannot be empty");
        }
    }
    public class RetractUserFromEventCommandValidator : AbstractValidator<RetractUserFromEventCommandData>
    {
        public RetractUserFromEventCommandValidator()
        {
            RuleFor(command => command.EventId).NotEmpty().WithMessage("Event ID cannot be empty");
            RuleFor(command => command.UserId).NotEmpty().WithMessage("User ID cannot be empty");
        }
    }
    public class GetRefreshTokenCommandValidator : AbstractValidator<GetRefreshTokenCommandData>
    {
        public GetRefreshTokenCommandValidator()
        {
            RuleFor(command => command.AccessToken).NotEmpty().WithMessage("Access token cannot be empty");
        }
    }
    public class LoginCommandValidator : AbstractValidator<LoginCommandData>
    {
        public LoginCommandValidator()
        {
            RuleFor(command => command.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address");
            RuleFor(command => command.Password).NotEmpty().WithMessage("Password cannot be empty");
        }
    }
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommandData>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(command => command.Token).NotEmpty().WithMessage("Token cannot be empty");
        }
    }
    public class ClearRefreshTokenCommandValidator : AbstractValidator<ClearRefreshTokenCommandData>
    {
        public ClearRefreshTokenCommandValidator()
        {
            RuleFor(command => command.RefreshToken).NotNull().WithMessage("Refresh token cannot be null");
            RuleFor(command => command.RefreshToken.Token).NotEmpty().WithMessage("Refresh token value cannot be empty");
        }
    }
}
