namespace EventList.Domain.QueryData
{
    public class GetUserByGuidQueryData
    {
        public required Guid Id;
    }
    public class GetUsersForThisEventQueryData
    {
        public required Guid EventId;
    }
}
