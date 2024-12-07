using Microsoft.AspNetCore.Mvc;
using EventList.Domain.Data;
using EventList.API.DTO;
using AutoMapper;
using EventList.Infrastructure.CQRS.Queries;
using EventList.Application.CQRS.Commands;
using EventList.Application.JWT;
using EventList.Application.ImageProcessing;
using Microsoft.AspNetCore.Authorization;
using EventList.Domain.CommandData;
using EventList.Domain.QueryData;

namespace EventList.API.Structure.Controllers;

[ApiController]
[Route("[controller]")]
//[Authorize]
public class EventController(IWebHostEnvironment environment,
    IMapper mapper, EventQueries queries, EventCommands commands, ImageHandler imageHandler) : Controller
{
    private readonly IMapper mapper = mapper;
    private readonly IWebHostEnvironment environment = environment;
    private readonly EventQueries queries = queries;
    private readonly EventCommands commands = commands;
    private readonly ImageHandler imageHandler = imageHandler;

    //Получение списка всех событий

    [HttpGet("GetEventList")]
    public List<Event> GetAllEvents()
    {
        return queries.GetAllEvents();
    }
    [HttpGet("GetEventsPaginated")]
    public List<Event> GetEventsPaginated(int page = 1)
    {
        return queries.GetEventsPaginated(new GetEventsPaginatedQueryData { Page = page });
    }
    //Получение определённого события по его ID
    [HttpGet("GetEventById")]
    public EventDTO GetEvent_ID(Guid id)
    {
        return mapper.Map<EventDTO>(queries.GetEvent_ID(new GetEvent_IDQueryData { Id = id }));
    }
    //Получение события по его названию
    [HttpGet("GetEventByName")]
    public EventDTO GetEvent_Name(string Name)
    {
        return mapper.Map<EventDTO>(queries.GetEvent_Name(new GetEvent_NameQueryData { Name = Name }));
    }
    //Добавление нового события
    
    [HttpPost("AddEvent")]
    [CheckAdmin]
    public async Task AddEvent(Event _event)
    {
        await commands.AddEvent(new AddEventCommandData { Event = _event });
    }

    [HttpPost("UploadImage")]
    public async Task<string> UploadImage(IFormFile file)
    {
        return await imageHandler.UploadImage(environment.ContentRootPath, "EventPhotos", file);
    }
    //Изменение информации о существующем событии
    [HttpPost("EditEvent")]
    public async Task EditEvent(Event newEvent)
    {
        await commands.EditEvent(new EditEventCommandData { Event = newEvent });
    }
    //Удаление события
    
    [HttpGet("GetEventsForThisUser")]
    [Authorize]
    public List<Event> GetEventsForThisUser(Guid UserId)
    {
        return queries.GetEventsForThisUser(new GetEventsForThisUserQueryData { UserId = UserId });
    }

    [HttpDelete("DeleteEvent")]
    public async Task DeleteEvent(Guid EventId)
    {
       await commands.DeleteEvent(new DeleteEventCommandData {
           Event = await queries.GetEvent_ID(new GetEvent_IDQueryData { Id = EventId }),
           FilePath = environment.ContentRootPath });
    }
}
