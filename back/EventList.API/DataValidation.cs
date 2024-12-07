using EventList.Application.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace EventList.API
{
    public static class DataValidation
    {
        public static IServiceCollection AddDataValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<RegisterValidator>();
            services
                .AddCommandValidation()
                .AddQueryValidation();
            return services;
        }
        private static IServiceCollection AddCommandValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<AddEventCommandValidator>()
                .AddValidatorsFromAssemblyContaining<EditEventCommandValidator>()
                .AddValidatorsFromAssemblyContaining<DeleteEventCommandValidator>()
                .AddValidatorsFromAssemblyContaining<ToggleAdminCommandValidator>()
                .AddValidatorsFromAssemblyContaining<UpdateUserPfpCommandValidator>()
                .AddValidatorsFromAssemblyContaining<RegisterUserInEventCommandValidator>()
                .AddValidatorsFromAssemblyContaining<RetractUserFromEventCommandValidator>()
                .AddValidatorsFromAssemblyContaining<GetRefreshTokenCommandValidator>()
                .AddValidatorsFromAssemblyContaining<LoginCommandValidator>()
                .AddValidatorsFromAssemblyContaining<RefreshTokenCommandValidator>()
                .AddValidatorsFromAssemblyContaining<ClearRefreshTokenCommandValidator>();
            return services;
        }
        private static IServiceCollection AddQueryValidation(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<GetEventsPaginatedValidator>()
                .AddValidatorsFromAssemblyContaining<GetEvent_IDValidator>()
                .AddValidatorsFromAssemblyContaining<GetEvent_NameValidator>()
                .AddValidatorsFromAssemblyContaining<GetEventsForThisUserValidator>()
                .AddValidatorsFromAssemblyContaining<GetUserByGuidValidator>()
                .AddValidatorsFromAssemblyContaining<GetUsersForThisEventValidator>();
            return services;
        }
    }
}
