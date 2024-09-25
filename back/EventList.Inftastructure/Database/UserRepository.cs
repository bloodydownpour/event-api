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
        EventUser eventUser = new EventUser(EventId, UserId);
        if (context.EventUsers.All(eu => eu != eventUser))
        {
            context.EventUsers.Add(eventUser);
        }
        else
        {
            throw new Exception("User is already enrolled");
        }
    }
    //Регистрация пользователя
    public void AddUser(User user)
    {
        if (context.Users.All(u => u._Email != user._Email))
        {
            context.Users.Add(user);
        }
        else throw new Exception("User exists");
    }
    //Получение участника по электронной почте
    public Task<User?> GetUserByEmail(string Email)
    {
        return context.Users.FirstOrDefaultAsync(x => x._Email == Email);
    }
    public bool IsUserEmailFree(string Email)
    {
        return context.Users.All(x => x._Email != Email);
    }
    //Получение участника по его ID
    public Task<User?> GetUserByGuid(Guid guid)
    {
        return context.Users.FirstOrDefaultAsync(x => x.UserId == guid);
    }
    //Отмена участия пользователя в событии
    public void RetractUserFromEvent(Guid EventId, Guid UserId)
    {
        EventUser eventUser = new EventUser(EventId, UserId);
        if (context.EventUsers.Any(eu => eu == eventUser))
        {
            context.EventUsers.Remove(eventUser);
        }
        else
        {
            throw new Exception("User isn't participating in this event");
        }
    }
    public void ToggleAdmin(Guid UserId)
    {
        User? user = GetUserByGuid(UserId).Result;
        if (user != null)
        {
            user.IsAdmin = !user.IsAdmin;
            context.Users.Entry(user).CurrentValues.SetValues(user);
        }
    }
    public void UpdateUserPfp(Guid id, string fileName)
    {
        User? user = GetUserByGuid(id).Result;
        if (user != null)
        {
            user.PfpName = fileName;
            context.Users.Entry(user).CurrentValues.SetValues(user);
        }
    }

    public List<User> GetUsersForThisEvent(Guid EventId)
    {
        List<EventUser> eu = [.. context.EventUsers.Where(eu => eu.EventId == EventId)];
        List<User> users = new List<User>();
        foreach(EventUser user in eu)
        {
            User? u = GetUserByGuid(user.UserId).Result; 
            if (u != null)
                users.Add(u);
        }
        return users;
    }
}
