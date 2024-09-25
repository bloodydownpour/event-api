using EventList.Domain.Data;
using EventList.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace EventList.Infrastructure.Database;

public class EventRepository(EventDbContext context) : IEventRepository
{
    private readonly EventDbContext context = context;
    //Получение списка всех ивентов
    public IQueryable<Event> GetEvents()
    {
        return context.Events;
    }
    //Получение ивента по айдишнику
    public Task<Event?> GetEventById(Guid id)
    {
        return context.Events.FirstOrDefaultAsync(x => x.EventId == id);
    }
    //Получение ивента по названию
    public Task<Event?> GetEventByName(string Name)
    {
        return context.Events.FirstOrDefaultAsync(x => x._EventName == Name);
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
        Event? target = GetEventById(newEvent.EventId).Result;
        if (target != null)
        {
            context.Events.Entry(target).CurrentValues.SetValues(newEvent);
        }
    }
    //Удаление ивента
    public void DeleteEvent(Guid EventId)
    {
        ClearRelations(EventId);
        Event? target = GetEventById(EventId).Result;
        if (context.Events.Any(e => e.EventId == EventId) && target != null)
        {
            context.Events.Remove(target);
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
            Event? target = GetEventById(eventUser.EventId).Result;
            if (target != null)
                events.Add(target);
        }
        return events;
    }
}
