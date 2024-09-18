using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using WebApplication1.Structure.Data;
using WebApplication1.Structure.Database;
using WebApplication1.Structure.JWT;
using WebApplication1.Validation;

namespace WebApplication1.Structure.Controllers
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
