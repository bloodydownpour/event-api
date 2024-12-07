using EventList.Application.Exceptions;
using EventList.Application.ImageProcessing;
using EventList.Domain.CommandData;
using EventList.Domain.Interfaces;

namespace EventList.Application.CQRS.Commands
{
    public class EventCommands(IUnitOfWork unitOfWork, ImageHandler imageHandler)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        private readonly ImageHandler imageHandler = imageHandler;

        public async Task AddEvent(AddEventCommandData data)
        {
            unitOfWork.Events.AddEvent(data.Event);
            await unitOfWork.SaveAsync();
        }
        public async Task EditEvent(EditEventCommandData data)
        {
            if (await unitOfWork.Events.GetEventById(data.Event.EventId) == null)
                throw new NotFoundException("Event not found");

            unitOfWork.Events.EditEvent(data.Event);
            await unitOfWork.SaveAsync();
        }
        public async Task DeleteEvent(DeleteEventCommandData data)
        {
            if (await unitOfWork.Events.GetEventById(data.Event.EventId) == null) 
                throw new NotFoundException("Event not found");
            imageHandler.DeleteImage(data.FilePath, "EventPhotos", data.Event.FileName);
            unitOfWork.Events.DeleteEvent(data.Event);
            await unitOfWork.SaveAsync();
        }
    }
}
