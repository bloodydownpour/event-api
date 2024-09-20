using System.ComponentModel.DataAnnotations;
namespace EventList.Domain.Data
{
    public class Event
        //(Guid Id, string EventName, string Description, DateTime Time, string Place, string Category)
    {
        [Required]
        public Guid EventId { get; set; }
        [Required]
        public string _EventName { get; set; }
        [Required]
        public string _Description { get; set; }
        [Required]
        public DateTime _Time { get; set; }
        [Required]
        public string _Place { get; set; }
        [Required]
        public string _Category { get; set; }
        public string FileName { get; set; }
        //-----------------------------------

    }
}
