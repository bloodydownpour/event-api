using AutoMapper;
using EventList.Domain.Data;
using EventList.Domain.Interfaces;

namespace EventList.Infrastructure.CQRS.Queries
{
    public class EventQueries(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        public List<Event> GetAllEvents()
        {
            return [.. unitOfWork.Events.GetEvents()];
        }
        public List<Event> GetEventsPaginated(int id)
        {
            return [.. unitOfWork.Events.GetEventsPaginated(id)];
        }
        public async Task<Event> GetEvent_ID(Guid id)
        {
            return await unitOfWork.Events.GetEventById(id);
        }
        public async Task<Event> GetEvent_Name(string Name)
        {
            return await unitOfWork.Events.GetEventByName(Name);
        }
        public List<Event> GetEventsForThisUser(Guid UserId)
        {
            return unitOfWork.Events.GetEventsForThisUser(
                unitOfWork.Events.GetEUForUser(UserId));
        }
    }
}
