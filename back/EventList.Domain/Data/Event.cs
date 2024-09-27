using System.ComponentModel.DataAnnotations;
namespace EventList.Domain.Data
{
    public class Event
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
