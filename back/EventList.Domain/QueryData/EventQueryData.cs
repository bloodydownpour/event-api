namespace EventList.Domain.QueryData
{
    public class GetEventsPaginatedQueryData
    {
        public required int Page;
    }
    public class GetEvent_IDQueryData
    {
        public required Guid Id;
    }
    public class GetEvent_NameQueryData
    {
        public required string Name;
    }
    public class GetEventsForThisUserQueryData
    {
        public required Guid UserId;
    }

}
