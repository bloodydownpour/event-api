using WebApplication1.Structure.Data;
namespace WebApplication1.Structure.Database
{
    public class UserRepos(EventDbContext context)
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
        }
        //Получение участника по электронной почте
        public User GetUserByEmail(string Email)
        {
            return context.Users.Single(x => x._Email == Email);
        }
        //Получение участника по его ID
        public User GetUserByGuid(Guid guid)
        {
            return context.Users.Single(x => x.UserId == guid);
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
            User user = GetUserByGuid(UserId);
            user.IsAdmin = !user.IsAdmin;
            context.Users.Entry(GetUserByGuid(user.UserId)).CurrentValues.SetValues(user);
        }
        public void UpdateUserPfp(Guid id, string fileName)
        {
            User user = GetUserByGuid(id);
            user.PfpName = fileName;
            context.Users.Entry(GetUserByGuid(user.UserId)).CurrentValues.SetValues(user);
        }
    }
}
