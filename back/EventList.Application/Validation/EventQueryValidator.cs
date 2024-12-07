using EventList.Domain.QueryData;
using FluentValidation;

namespace EventList.Application.Validation
{
    public class GetEventsPaginatedValidator : AbstractValidator<GetEventsPaginatedQueryData>
    {
        public GetEventsPaginatedValidator()
        {
            RuleFor(query => query.Page).NotEmpty().WithMessage("Page number cannot be zero or empty");
        }
    }
    public class GetEvent_IDValidator : AbstractValidator<GetEvent_IDQueryData>
    {
        public GetEvent_IDValidator()
        {
            RuleFor(query => query.Id).NotEmpty().WithMessage("Event ID cannot be empty");
        }
    }
    public class GetEvent_NameValidator : AbstractValidator<GetEvent_NameQueryData>
    {
        public GetEvent_NameValidator()
        {
            RuleFor(query => query.Name).NotEmpty().WithMessage("Event name cannot be empty");
        }
    }
    public class GetEventsForThisUserValidator : AbstractValidator<GetEventsForThisUserQueryData>
    {
        public GetEventsForThisUserValidator()
        {
            RuleFor(query => query.UserId).NotEmpty().WithMessage("User ID cannot be empty");
        }
    }
}
