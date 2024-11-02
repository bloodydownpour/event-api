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
        public Event? GetEvent_ID(Guid id)
        {
            return unitOfWork.Events.GetEventById(id).Result;
        }
        public Event? GetEvent_Name(string Name)
        {
            return unitOfWork.Events.GetEventByName(Name).Result;
        }
        
        public List<Event> GetEventsForThisUser(Guid UserId)
        {

            return unitOfWork.Events.GetEventsForThisUser(
                unitOfWork.Events.GetEUForUser(UserId));
        }
    }
}
