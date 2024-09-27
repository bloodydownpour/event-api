using Microsoft.AspNetCore.Mvc;
using EventList.Domain.Data;
using EventList.API.DTO;
using AutoMapper;
using EventList.Persistence.JWT;
using EventList.Infrastructure.CQRS.Queries;
using EventList.Infrastructure.CQRS.Commands;
using Microsoft.AspNetCore.Authorization;

namespace EventList.API.Structure.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class EventController(IWebHostEnvironment environment,
    IMapper mapper, EventQueries queries, EventCommands commands) : Controller
{
    private readonly IMapper mapper = mapper;
    private readonly IWebHostEnvironment environment = environment;
    private readonly EventQueries queries = queries;
    private readonly EventCommands commands = commands;

    //Получение списка всех событий

    [HttpGet("GetEventList")]
    public List<Event> GetAllEvents()
    {
        return queries.GetAllEvents();
    }
    [HttpGet("GetEventListPaginated")]
    public List<Event> GetAllEventsPaginated(int page = 1)
    {
        return queries.GetAllEventsPaginated(page);
    }
    //Получение определённого события по его ID
    [HttpGet("GetEventById")]
    public EventDTO GetEvent_ID(Guid id)
    {
        return mapper.Map<EventDTO>(queries.GetEvent_ID(id));
    }
    //Получение события по его названию
    [HttpGet("GetEventByName")]
    public EventDTO GetEvent_Name(string Name)
    {
        return mapper.Map<EventDTO>(queries.GetEvent_Name(Name));
    }
    //Добавление нового события
    
    [HttpPost("AddEvent")]
    [CheckAdmin]
    public async Task AddEvent(Event _event)
    {
        await commands.AddEvent(_event);
    }

    [HttpPost("UploadImage")]
    public async Task<string> UploadImage(IFormFile file)
    {
        string fileName;
        if (file != null && file.Length > 0)
        {
            fileName = file.FileName;
            var physPath = environment.ContentRootPath + "/EventPhotos/" + file.FileName;
            using (var stream = new FileStream(physPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }
        else
        {
            fileName = "default.png";
        }
        return fileName;
    }
    //Изменение информации о существующем событии
    [HttpPost("EditEvent")]
    public async Task EditEvent(Event newEvent)
    {
        await commands.EditEvent(newEvent);
    }
    //Удаление события

    [HttpGet("GetEventsForThisUser")]
    public List<Event> GetEventsForThisUser(Guid UserId)
    {
        return queries.GetEventsForThisUser(UserId);
    }

    [HttpDelete("DeleteEvent")]
    public async Task DeleteEvent(Guid eventId)
    {
       await commands.DeleteEvent(eventId);
    }
}
