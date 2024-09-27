using EventList.Domain.Data;
using EventList.Infrastructure.Database;
using Microsoft.AspNetCore.Mvc;

namespace EventList.Infrastructure.CQRS.Commands
{
    public class UserCommands(UnitOfWork unitOfWork)
    {
        private readonly UnitOfWork unitOfWork = unitOfWork;
        public async Task RegisterUserInEvent(Guid EventId, Guid UserId)
        {
            unitOfWork.Users.EnrollUserInEvent(EventId, UserId);
            await unitOfWork.SaveAsync();
        }
        public async Task ToggleAdmin(Guid UserId)
        {
            unitOfWork.Users.ToggleAdmin(UserId);
            await unitOfWork.SaveAsync();
        }
        public async Task UpdateUserPfp(Guid id, string fileName)
        {
            unitOfWork.Users.UpdateUserPfp(id, fileName);
            await unitOfWork.SaveAsync();
        }
        public async Task AddUserInEvent(Guid EventId, Guid UserId)
        {
            unitOfWork.Users.EnrollUserInEvent(EventId, UserId);
            await unitOfWork.SaveAsync();
        }
        public async Task RetractUserFromEvent(Guid EventId, Guid UserId)
        {
            unitOfWork.Users.RetractUserFromEvent(EventId, UserId);
            await unitOfWork.SaveAsync();
        }
    }
}
