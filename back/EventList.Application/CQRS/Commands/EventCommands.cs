using EventList.Application.Exceptions;
using EventList.Application.ImageHandler;
using EventList.Domain.Data;
using EventList.Domain.Interfaces;

namespace EventList.Infrastructure.CQRS.Commands
{
    public class EventCommands(IUnitOfWork unitOfWork, ImageHandler imageHandler)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly ImageHandler imageHandler = imageHandler;

        public async Task AddEvent(Event _event)
        {
            unitOfWork.Events.AddEvent(_event);
            await unitOfWork.SaveAsync();
        }
        public async Task EditEvent(Event newEvent)
        {
            if (unitOfWork.Events.GetEventById(newEvent.EventId).Result == null)
                throw new NotFoundException("Event not found");

            unitOfWork.Events.EditEvent(newEvent);
            await unitOfWork.SaveAsync();
        }
        public async Task DeleteEvent(Event Event, string filePath)
        {
            if (await unitOfWork.Events.GetEventById(Event.EventId) == null) 
                throw new NotFoundException("Event not found");

            imageHandler.DeleteImage(filePath, "EventPhotos", Event.FileName);
            unitOfWork.Events.DeleteEvent(Event);
            await unitOfWork.SaveAsync();
        }
    }
}
