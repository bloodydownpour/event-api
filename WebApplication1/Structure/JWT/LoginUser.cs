using Microsoft.AspNet.Identity;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Structure.Data;
using WebApplication1.Structure.Database;

namespace WebApplication1.Structure.JWT
{
    public class LoginUser(UnitOfWork uow, TokenProvider tokenProvider)
    {
        private readonly UnitOfWork unit = uow;
        private readonly TokenProvider _token = tokenProvider;
        public string Handle(string Email, string Password)
        {
            User user = unit.Users.GetUserByEmail(Email) ?? throw new Exception("User not found");
            var hash = SHA256.Create();
            bool verified = (Convert.ToHexString(hash.ComputeHash(Encoding.UTF8.GetBytes(Password))).Equals(user._Password, StringComparison.CurrentCultureIgnoreCase));
            if (!verified)
            {
                throw new InvalidOperationException("Incorrect password");
            }
            string token = _token.Create(user);
            return token;
        }
    }
}
