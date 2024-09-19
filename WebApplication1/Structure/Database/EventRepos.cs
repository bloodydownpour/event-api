using WebApplication1.Structure.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Eventing.Reader;
namespace WebApplication1.Structure.Database
{
    public class EventRepos(EventDbContext context)
    {
        private readonly EventDbContext context = context;
        //Получение списка всех ивентов
        public IQueryable<Event> GetEvents()
        {
            return context.Events;
        }
        //Получение ивента по айдишнику
        public Event GetEventById(Guid id)
        {
            return context.Events.Single(x => x.EventId == id);
        }
        //Получение ивента по названию
        public Event GetEventByName(string Name)
        {
            return context.Events.Single(x => x._EventName == Name);
        }
        //Добавление ивента
        public void AddEvent(Event entity)
        {
            if (context.Events.All(e => e.EventId != entity.EventId)) 
            {
                context.Events.Add(entity);
            }
        }
        //Изменение ивента
        public void EditEvent(Event newEvent)
        {
            context.Events.Entry(GetEventById(newEvent.EventId)).CurrentValues.SetValues(newEvent);
        }
        //Удаление ивента
        public void DeleteEvent(Guid EventId)
        {
            ClearRelations(EventId);
            if (context.Events.Any(e => e.EventId == EventId))
            {
                context.Events.Remove(GetEventById(EventId));
            }
        }
        public void ClearRelations(Guid EventId)
        {
            List<EventUser> entities = context.EventUsers.Where(x => x.EventId == EventId).ToList<EventUser>();
            foreach (var entity in entities)
            {
                context.EventUsers.Remove(entity);
            }
        }
        //Получение списка событий для определённого пользователя
        public List<Event> GetEventsForThisUser(Guid UserId)
        {
            List<EventUser> eu = [.. context.EventUsers.Where(eu => eu.UserId == UserId)];
            List<Event> events = new List<Event>();
            foreach (EventUser eventUser in eu)
            {
                events.Add(GetEventById(eventUser.EventId));
            }
            return events;
        }
    }
}
