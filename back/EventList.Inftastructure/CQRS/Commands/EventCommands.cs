using EventList.Domain.Data;
using EventList.Infrastructure.Database;

namespace EventList.Infrastructure.CQRS.Commands
{
    public class EventCommands(UnitOfWork unitOfWork)
    {
        private readonly UnitOfWork unitOfWork = unitOfWork;
        public async Task AddEvent(Event _event)
        {
            unitOfWork.Events.AddEvent(_event);
            await unitOfWork.SaveAsync();
        }
        public async Task EditEvent(Event newEvent)
        {
            unitOfWork.Events.EditEvent(newEvent);
            await unitOfWork.SaveAsync();
        }
        public async Task DeleteEvent(Guid eventId)
        {
            unitOfWork.Events.DeleteEvent(eventId);
            await unitOfWork.SaveAsync();
        }
    }
}
