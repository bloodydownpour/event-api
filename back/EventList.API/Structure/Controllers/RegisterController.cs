using EventList.Domain.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using EventList.Persistence.Database;

namespace EventList.API.Structure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController : Controller
    {
        private readonly UnitOfWork unitOfWork;
        public RegisterController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        private string Encrypt(string value)
        {
            var hash = SHA256.Create();
            return Convert.ToHexString(hash.ComputeHash(Encoding.UTF8.GetBytes(value))).ToLower();
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
