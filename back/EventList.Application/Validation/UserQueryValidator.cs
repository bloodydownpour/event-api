using EventList.Domain.QueryData;
using FluentValidation;

namespace EventList.Application.Validation
{
    public class GetUserByGuidValidator : AbstractValidator<GetUserByGuidQueryData>
    {
        public GetUserByGuidValidator() { 
            RuleFor(query => query.Id).NotEmpty().WithMessage("User ID cannot be empty");
        }
    }
    public class GetUsersForThisEventValidator : AbstractValidator<GetUsersForThisEventQueryData>
    {
        public GetUsersForThisEventValidator()
        {
            RuleFor(query => query.EventId).NotEmpty().WithMessage("Event ID cannot be empty");
        }
    }

}
