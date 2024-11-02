using EventList.Domain.Data;

namespace EventList.Domain.Interfaces
{
    public interface IEventRepository
    {
        public IQueryable<Event> GetEvents();
        public IQueryable<Event> GetEventsPaginated(int id);
        public Task<Event?> GetEventById(Guid id);
        public Task<Event?> GetEventByName(string Name);
        public void AddEvent(Event entity);
        public void EditEvent(Event newEvent);
        public void DeleteEvent(Event Event);
        public List<EventUser> GetEUForUser(Guid UserId);
        public List<Event> GetEventsForThisUser(List<EventUser> result);
    }
}
