using EventList.Domain.Data;

namespace EventList.Domain.CommandData
{
    public class AddEventCommandData
    {
        public required Event Event { get; set; }
    }
    public class EditEventCommandData
    {
        public required Event Event { get; set; }
    }
    public class DeleteEventCommandData
    {
        public required Event Event { get; set; }
        public required string FilePath { get; set; }
    }
}
