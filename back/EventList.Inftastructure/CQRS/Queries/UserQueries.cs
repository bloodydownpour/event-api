using AutoMapper;
using EventList.Domain.Data;
using EventList.Domain.Interfaces;
using EventList.Infrastructure.Database;

namespace EventList.Infrastructure.CQRS.Queries
{
    public class UserQueries(UnitOfWork unitOfWork, IMapper mapper)
    {
        private readonly UnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;
        public List<User> GetAllUsers()
        {
            return [.. unitOfWork.Users.GetUsers()];
        }
        public User? GetUserByGuid(Guid id)
        {
            return unitOfWork.Users.GetUserByGuid(id).Result;
        }
        public List<User> GetUsersForThisEvent(Guid EventId)
        {
            return unitOfWork.Users.GetUsersForThisEvent(EventId);
        }

    }
}
