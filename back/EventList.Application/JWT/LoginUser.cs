using System.Security.Cryptography;
using System.Text;
using EventList.Application.Exceptions;
using EventList.Domain.Data;
using EventList.Domain.Interfaces;

namespace EventList.Application.JWT;

public class LoginUser(IUnitOfWork uow, TokenProvider tokenProvider) : ILoginUser
{
    private readonly IUnitOfWork unit = uow;
    private readonly TokenProvider _token = tokenProvider;
    public string Handle(string Email, string Password)
    {
        User? user = unit.Users.GetUserByEmail(Email).Result;
        if (user == null)
            throw new Exception("User not found");
        bool verified = (Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(Password)))
            .Equals(user._Password, StringComparison.CurrentCultureIgnoreCase));
        if (!verified)
        {
            throw new UnauthorizedException("Invalid password");
        }
        string token = _token.Create(user);
        return token;
    }
}
