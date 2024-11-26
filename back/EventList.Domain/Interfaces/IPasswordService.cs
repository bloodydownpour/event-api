using EventList.Domain.Data;

namespace EventList.Domain.Interfaces
{
    public interface IPasswordService
    {
        public string Encrypt(string value);
        public bool VerifyPassword(string Password, User user);
    }
}
