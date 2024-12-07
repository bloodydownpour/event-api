using AutoMapper;
using EventList.Domain.Data;
using EventList.Domain.Interfaces;
using EventList.Domain.QueryData;

namespace EventList.Infrastructure.CQRS.Queries
{
    public class EventQueries(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        public List<Event> GetAllEvents()
        {
            return [.. unitOfWork.Events.GetEvents()];
        }
        public List<Event> GetEventsPaginated(GetEventsPaginatedQueryData data)
        {
            return [.. unitOfWork.Events.GetEventsPaginated(data.Page)];
        }
        public async Task<Event> GetEvent_ID(GetEvent_IDQueryData data)
        {
            return await unitOfWork.Events.GetEventById(data.Id);
        }
        public async Task<Event> GetEvent_Name(GetEvent_NameQueryData data)
        {
            return await unitOfWork.Events.GetEventByName(data.Name);
        }
        public List<Event> GetEventsForThisUser(GetEventsForThisUserQueryData data)
        {
            return unitOfWork.Events.GetEventsForThisUser(
                unitOfWork.Events.GetEUForUser(data.UserId));
        }
    }
}
