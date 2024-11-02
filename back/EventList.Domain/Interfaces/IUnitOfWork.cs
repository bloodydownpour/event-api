namespace EventList.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IEventRepository Events { get; }
        IUserRepository Users { get; }
        public Task SaveAsync();
    }
}
