using Microsoft.AspNet.Identity;
using System.Security.Cryptography;
using System.Text;
using EventList.Domain.Data;
using EventList.Persistence.Database;

namespace EventList.Persistence.JWT;

public class LoginUser(UnitOfWork uow, TokenProvider tokenProvider)
{
    private readonly UnitOfWork unit = uow;
    private readonly TokenProvider _token = tokenProvider;
    public string Handle(string Email, string Password)
    {
        User user = unit.Users.GetUserByEmail(Email) ?? throw new Exception("User not found");
        bool verified = (Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(Password))).Equals(user._Password, StringComparison.CurrentCultureIgnoreCase));
        if (!verified)
        {
            throw new InvalidOperationException("Incorrect password");
        }
        string token = _token.Create(user);
        return token;
    }
}
