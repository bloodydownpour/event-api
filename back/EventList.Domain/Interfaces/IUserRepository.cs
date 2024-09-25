using EventList.Domain.Data;

namespace EventList.Domain.Interfaces
{
    public interface IUserRepository
    {
        public IQueryable<User> GetUsers();
        public void EnrollUserInEvent(Guid EventId, Guid UserId);
        public void AddUser(User user);
        public Task<User?> GetUserByEmail(string Email);
        public bool IsUserEmailFree(string Email);
        public Task<User?> GetUserByGuid(Guid guid);
        public void RetractUserFromEvent(Guid EventId, Guid UserId);
        public void ToggleAdmin(Guid UserId);
        public void UpdateUserPfp(Guid id, string fileName);
        public List<User> GetUsersForThisEvent(Guid EventId);
    }
}
