using EventList.Domain.Data;
using EventList.Domain.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
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
            context.Users.Entry(user).CurrentValues.SetValues(user);
    }
    public void UpdateUserPfp(User user, string fileName)
    {
            user.PfpName = fileName;
            context.Users.Entry(user).CurrentValues.SetValues(user);
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
    public async Task<RefreshToken?> GetRefreshTokenByGuid(Guid userId)
    {
        return await context.RefreshTokens.FirstOrDefaultAsync(rt =>
        rt.UserId == userId && rt.Expiration > DateTime.UtcNow);
    }

    public async Task<RefreshToken?> GetRefreshTokenByToken(string token)
    {
        return await context.RefreshTokens.FirstOrDefaultAsync(rt =>
        rt.AccessToken == token && rt.Expiration > DateTime.UtcNow);
    }
    public void ClearRefreshToken(RefreshToken refreshToken)
    {
        context.RefreshTokens.Remove(refreshToken);
    }

    public async Task UpdateRefreshToken(RefreshToken rt, string newToken)
    {
        rt.AccessToken = newToken;
        context.RefreshTokens.Entry(rt).CurrentValues.SetValues(rt);
    }
    public void SaveRefreshToken(string token, Guid userId)
    {
        var refreshToken = new RefreshToken(token, userId, DateTime.UtcNow.AddDays(30));
        context.RefreshTokens.Add(refreshToken);
    }
}
