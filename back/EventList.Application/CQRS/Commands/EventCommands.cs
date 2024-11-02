using EventList.Application.Exceptions;
using EventList.Domain.Data;
using EventList.Domain.Interfaces;

namespace EventList.Infrastructure.CQRS.Commands
{
    public class EventCommands(IUnitOfWork unitOfWork)
    {
        private readonly IUnitOfWork unitOfWork = unitOfWork;
        public async Task AddEvent(Event _event)
        {
            unitOfWork.Events.AddEvent(_event);
            await unitOfWork.SaveAsync();
        }
        public async Task EditEvent(Event newEvent)
        {
            if (unitOfWork.Events.GetEventById(newEvent.EventId).Result != null)
            {
                unitOfWork.Events.EditEvent(newEvent);
                await unitOfWork.SaveAsync();
            }
            else throw new NotFoundException("Event not found");
        }
        public async Task DeleteEvent(Event Event)
        {
            if (Event != null)
            {
                unitOfWork.Events.DeleteEvent(Event);
                await unitOfWork.SaveAsync();
            }
            else throw new NotFoundException("Event not found");
        }
    }
}
