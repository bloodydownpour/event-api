namespace EventList.Domain.Interfaces
{
    public interface ILoginUser
    {
        public string Handle(string Email, string Password);
    }
}
