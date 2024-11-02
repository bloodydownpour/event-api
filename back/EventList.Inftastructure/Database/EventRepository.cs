using EventList.Domain.Data;
using EventList.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace EventList.Infrastructure.Database;

public class EventRepository(EventDbContext context) : IEventRepository
{
    private readonly EventDbContext context = context;
    private const int PAGE_SIZE = 5;
    //Получение списка всех ивентов
    public IQueryable<Event> GetEvents()
    {
        return context.Events;
    }
    //Получение списка всех ивентов, но с пагинацией(NO SHIT SHERLOCK)

    public IQueryable<Event> GetEventsPaginated(int id)
    {
        return context.Events.Skip(PAGE_SIZE * (id - 1)).Take(PAGE_SIZE);
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
        context.Events.Add(entity);
    }
    //Изменение ивента
    public void EditEvent(Event newEvent)
    {
        context.Events
            .Entry(context.Events.Find(newEvent.EventId))
            .CurrentValues.SetValues(newEvent);
    }
    //Удаление ивента
    public void DeleteEvent(Event Event)
    {
        context.EventUsers.
            RemoveRange(context.EventUsers.Where(e => e.EventId == Event.EventId));
        context.Events.Remove(Event);
    }
    public List<EventUser> GetEUForUser(Guid UserId)
    {
        return [.. context.EventUsers.Where(eu => eu.UserId == UserId)];
    }
    //Получение списка событий для определённого пользователя
    public List<Event> GetEventsForThisUser(List<EventUser> result)
    {

        return [.. result.Select(res => GetEventById(res.EventId).Result)];
    }
}
