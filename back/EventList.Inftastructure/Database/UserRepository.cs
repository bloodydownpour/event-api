using EventList.Domain.Data;
using EventList.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
namespace EventList.Infrastructure.Database;

public class UserRepository(EventDbContext context) : IUserRepository
{
    private readonly EventDbContext context = context;
    //-----------USER-----------
    //Получение списка всех пользователей
    public IQueryable<User> GetUsers()
    {
        return context.Users;
    }
    //Регистрация участия пользователя в событии
    public void EnrollUserInEvent(Guid EventId, Guid UserId)
    {
        context.EventUsers.Add(new EventUser(EventId, UserId));
    }
    //Регистрация пользователя
    public void AddUser(User user)
    {
        context.Users.Add(user);
    }
    //Получение участника по электронной почте
    public async Task<User?> GetUserByEmail(string Email)
    {
        return await context.Users.FirstOrDefaultAsync(x => x._Email == Email);
    }
    public bool IsUserEmailFree(string Email)
    {
        return context.Users.All(x => x._Email != Email);
    }
    //Получение участника по его ID
    public async Task<User?> GetUserByGuid(Guid guid)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.UserId == guid);
    }
    //Отмена участия пользователя в событии
    public void RetractUserFromEvent(Guid EventId, Guid UserId)
    {
        context.EventUsers.Remove(new EventUser(EventId, UserId));
    }
    public void ToggleAdmin(User user)
    {
        user.IsAdmin = !user.IsAdmin;
        context.Users.Update(user);
    }
    public void UpdateUserPfp(User user, string fileName)
    {
        user.PfpName = fileName;
        context.Users.Update(user);
    }

    public List<EventUser> GetEUForEvent(Guid EventId)
    {
        return [.. context.EventUsers.Where(eu => eu.EventId == EventId)];
    }
    //Получение списка событий для определённого пользователя
    public List<User> GetUsersForThisEvent(List<EventUser> result)
    {
        return [.. result.Select(res => GetUserByGuid(res.UserId).Result)];
    }
    public async Task<RefreshToken?> GetRefreshToken(Guid userId)
    {
        return await context.RefreshTokens.FirstOrDefaultAsync(rt =>
        rt.UserId == userId && rt.Expiration > DateTime.UtcNow);
    }

    public void ClearRefreshToken(RefreshToken refreshToken)
    {
        context.RefreshTokens.Remove(refreshToken);
    }

    public void CreateRefreshToken(Guid userId)
    { 
        context.RefreshTokens.Add(new RefreshToken(userId, DateTime.UtcNow.AddDays(30)));
    }
}
