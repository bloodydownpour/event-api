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
        public IQueryable<Event> GetEventWithDate(DateOnly RequiredDate)
        {
            return context.Events.Where(x => DateOnly.FromDateTime(x._Time) == RequiredDate);
        }
        public IQueryable<Event> GetEventWithPlace(string Place)
        {
            return context.Events.Where(x => x._Place == Place);
        }
        public IQueryable<Event> GetEventWithCategory(string Category)
        {
            return context.Events.Where(x => x._Category == Category);
        }
        //Удаление ивента
        public void DeleteEvent(Guid EventId)
        {
            if (context.Events.Any(e => e.EventId == EventId))
            {
                context.Events.Remove(GetEventById(EventId));
            }
            else throw new Exception("There is no such event");
        }
    }
}
