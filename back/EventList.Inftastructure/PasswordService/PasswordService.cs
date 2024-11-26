using EventList.Domain.Data;
using EventList.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace EventList.Infrastructure.PasswordService
{
    public class PasswordService : IPasswordService
    {
        public string Encrypt(string value)
        {
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value))).ToLower();
        }
        public bool VerifyPassword(string Password, User user)
        {
            return Encrypt(Password).Equals(user._Password, StringComparison.CurrentCultureIgnoreCase);
        }
    }
}
