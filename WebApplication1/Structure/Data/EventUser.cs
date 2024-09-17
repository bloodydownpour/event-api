namespace WebApplication1.Structure.Data
{
    public class EventUser
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public EventUser(Guid EventId, Guid UserId)
        {
            this.EventId = EventId;
            this.UserId = UserId;
        }
    }
}
