using EventList.Domain.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using EventList.Infrastructure.Database;

namespace EventList.API.Structure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController(UnitOfWork unitOfWork) : Controller
    {
        private readonly UnitOfWork unitOfWork = unitOfWork;

        private static string Encrypt(string value)
        {
            return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(value))).ToLower();
        }
        // GET: RegistrationController
        [HttpPost("RegisterUser")]
        
        public async Task<string> AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                user._Password = Encrypt(user._Password);
                unitOfWork.Users.AddUser(user);
                await unitOfWork.SaveAsync();
                return "200 OK\n" +
                    $"{user._Password}";
            } else
            {
                return "Failed";
            }
        }
    }
}
