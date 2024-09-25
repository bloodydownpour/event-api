using EventList.Domain.Data;

namespace EventList.Domain.Interfaces
{
    public interface IEventRepository
    {
        public IQueryable<Event> GetEvents();
        public Task<Event?> GetEventById(Guid id);
        public Task<Event?> GetEventByName(string Name);
        public void AddEvent(Event entity);
        public void EditEvent(Event newEvent);
        public void DeleteEvent(Guid EventId);
        public void ClearRelations(Guid EventId);
        public List<Event> GetEventsForThisUser(Guid UserId);
    }
}
