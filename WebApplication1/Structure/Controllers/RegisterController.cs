using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Cryptography;
using System.Text;
using WebApplication1.Structure.Data;
using WebApplication1.Structure.Database;
using WebApplication1.Structure.JWT;

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
        public async Task<string> AddUser(string Name, string Surname, string DateOfBirth, string Email, string Password)
        {
 
            User user = new User()
            {
                UserId = Guid.NewGuid(),
                _Name = Name,
                _Surname = Surname,
                _DateOfBirth = DateOnly.Parse(DateOfBirth),
                _RegisterDate = DateOnly.FromDateTime(DateTime.Now),
                _Email = Email,
                _Password = Encrypt(Password),
                IsAdmin = false,
                PfpName = "default.png"
            };
            unitOfWork.Users.AddUser(user);
            await unitOfWork.SaveAsync();
            return "200 OK\n" +
                $"{Encrypt(Password)}";

        }
    }
}
