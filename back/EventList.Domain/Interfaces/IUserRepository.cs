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
        public void ToggleAdmin(User user);
        public void UpdateUserPfp(User user, string fileName);
        public List<EventUser> GetEUForEvent(Guid EventId);
        public List<User> GetUsersForThisEvent(List<EventUser> result);
        public void SaveRefreshToken(string token, Guid userId);
        public Task<RefreshToken?> GetRefreshTokenByGuid(Guid userId);
        public void ClearRefreshToken(RefreshToken refreshToken);
        public Task UpdateRefreshToken(RefreshToken rt, string newToken);
        public Task<RefreshToken?> GetRefreshTokenByToken(string token);
    }
}
