using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NUnit.Framework.Constraints;
using WebApplication1.Structure.Data;
using WebApplication1.Structure.JWT;
using WebApplication1.Structure.Database;
using WebApplication1.Pagination;
using System;
using System.Collections;
using NUnit.Framework;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Win32.SafeHandles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
namespace WebApplication1.Structure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class EventController : Controller
    {

        private readonly UnitOfWork unitOfWork;
        private readonly IWebHostEnvironment environment;
        public EventController(UnitOfWork unitOfWork, IWebHostEnvironment environment)
        {
            this.unitOfWork = unitOfWork;
            this.environment = environment;
        }
        //Получение списка всех событий
        
        [HttpGet("GetEventList")]
        public List<Event> GetAllEvents()
        {
            return [.. unitOfWork.Events.GetEvents()];
        }
        [HttpGet("GetEventListPaginated")]
        public List<Event> GetAllEventsPaginated(int page = 1)
        {
            List<Event> events = [.. unitOfWork.Events.GetEvents()];

            const int pageSize = 5;

            if (page < 1) page = 1;
            int recsCount = events.Count();
            Pager pager = new Pager(recsCount, page, pageSize);

            int recSkip = (page - 1) * pageSize;
            List<Event> showedData = events.Skip(recSkip).Take(pager.PageSize).ToList();
            return showedData;
        }
        //Получение определённого события по его ID
        [HttpGet("GetEventById")]
        public Event GetEvent_ID(Guid id)
        {
            return unitOfWork.Events.GetEventById(id);
        }
        //Получение события по его названию
        [HttpGet("GetEventByName")]
        public Event GetEvent_Name(string Name)
        {
            return unitOfWork.Events.GetEventByName(Name);
        }
        //Добавление нового события
        
        [HttpPost("AddEvent")]
        //[CheckAdmin]
        public async Task<string> AddEvent(Event _event)
        {
            unitOfWork.Events.AddEvent(_event);
            await unitOfWork.SaveAsync();
            return "OK";
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
            unitOfWork.Events.EditEvent(newEvent);
            await unitOfWork.SaveAsync();
        }
        //Удаление события

        [HttpGet("GetEventsForThisUser")]
        public List<Event> GetEventsForThisUser(Guid UserId)
        {
            return unitOfWork.Events.GetEventsForThisUser(UserId);
        }

        [HttpDelete("DeleteEvent")]
        public async Task DeleteEvent(Guid eventId)
        {
            unitOfWork.Events.DeleteEvent(eventId);
            await unitOfWork.SaveAsync();
        }
    }
}
