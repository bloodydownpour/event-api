using EventList.Domain.CommandData;
using FluentValidation;

namespace EventList.Application.Validation
{
    public class AddEventCommandValidator : AbstractValidator<AddEventCommandData>
    {
        public AddEventCommandValidator()
        {
            {
                RuleFor(command => command.Event).NotNull().WithMessage("Event cannot be null");
                RuleFor(command => command.Event.EventId).NotEmpty().WithMessage("Event ID cannot be empty");
            }
        }
    }
    public class EditEventCommandValidator : AbstractValidator<EditEventCommandData>
    {
        public EditEventCommandValidator()
        {
            {
                RuleFor(command => command.Event).NotNull().WithMessage("Event cannot be null");
                RuleFor(command => command.Event.EventId).NotEmpty().WithMessage("Event ID cannot be empty");
            }
        }
    }
    public class DeleteEventCommandValidator : AbstractValidator<DeleteEventCommandData>
    {
        public DeleteEventCommandValidator()
        {
            {
                RuleFor(command => command.Event).NotNull().WithMessage("Event cannot be null");
                RuleFor(command => command.Event.EventId).NotEmpty().WithMessage("Event ID cannot be empty");
                RuleFor(command => command.FilePath).NotEmpty().WithMessage("File path cannot be empty");
            }
        }
    }
}
