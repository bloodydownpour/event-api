using AutoMapper;
using EventList.Application.Pagination;
using EventList.Domain.Data;
using EventList.Domain.Interfaces;
using EventList.Infrastructure.Database;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Runtime.CompilerServices;

namespace EventList.Infrastructure.CQRS.Queries
{
    public class EventQueries(UnitOfWork unitOfWork, IMapper mapper)
    {
        private readonly UnitOfWork unitOfWork = unitOfWork;
        private readonly IMapper mapper = mapper;
        public List<Event> GetAllEvents()
        {
            return [.. unitOfWork.Events.GetEvents()];
        }
        public List<Event> GetAllEventsPaginated(int page = 1)
        {
            List<Event> events = [.. unitOfWork.Events.GetEvents()];

            const int pageSize = 5;

            if (page < 1) page = 1;
            int recsCount = events.Count;
            Pager pager = new Pager(recsCount, page, pageSize);

            int recSkip = (page - 1) * pageSize;
            List<Event> showedData = events.Skip(recSkip).Take(pager.PageSize).ToList();
            return showedData;
        }
        public Event? GetEvent_ID(Guid id)
        {
            return unitOfWork.Events.GetEventById(id).Result;
        }
        public Event? GetEvent_Name(string Name)
        {
            return unitOfWork.Events.GetEventByName(Name).Result;
        }
        public List<Event> GetEventsForThisUser(Guid UserId)
        {
            return unitOfWork.Events.GetEventsForThisUser(UserId);
        }
    }
}
