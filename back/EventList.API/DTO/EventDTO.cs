using System.ComponentModel.DataAnnotations;
using EventList.Domain.Interfaces;
namespace EventList.API.DTO
{
    public class EventDTO : IEventDTO
    {
        public Guid EventId { get; set; }
        public string _EventName { get; set; }
        public string _Description { get; set; }
        public DateTime _Time { get; set; }
        public string _Place { get; set; }
        public string _Category { get; set; }
        public string FileName { get; set; }
    }
}
