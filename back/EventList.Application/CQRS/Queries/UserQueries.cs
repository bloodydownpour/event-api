using AutoMapper;
using EventList.Domain.Data;
using EventList.Domain.Interfaces;

namespace EventList.Infrastructure.CQRS.Queries
{
    public class UserQueries(IUnitOfWork unitOfWork, IMapper mapper)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;
        public List<User> GetAllUsers()
        {
            return [.. unitOfWork.Users.GetUsers()];
        }
        public async Task<User?> GetUserByGuid(Guid id)
        {
            return await unitOfWork.Users.GetUserByGuid(id);
        }
        public List<User> GetUsersForThisEvent(Guid EventId)
        {
            return unitOfWork.Users.GetUsersForThisEvent(
                unitOfWork.Users.GetEUForEvent(EventId));
        }

    }
}
