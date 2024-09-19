using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WebApplication1.Structure.Data;
using WebApplication1.Structure.Database;
using WebApplication1.Structure.JWT;

namespace WebApplication1.Structure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UnitOfWork unitOfWork;
        private readonly TokenProvider tokenProvider;
        private readonly LoginUser loginUser;
        private readonly IWebHostEnvironment environment;
        public UserController(UnitOfWork unitOfWork, TokenProvider tokenProvider, LoginUser loginUser, IWebHostEnvironment environment)
        {
            this.unitOfWork = unitOfWork;
            this.tokenProvider = tokenProvider;
            this.loginUser = loginUser;
            this.environment = environment;
        }


        [HttpGet("GetUsers")]
        public List<User> GetAllUsers()
        {
            return unitOfWork.Users.GetUsers().ToList();
        }
        [HttpGet("GetUserByGuid")]
        public User GetUserByGuid(Guid id)
        {
            return unitOfWork.Users.GetUserByGuid(id);
        }
        [HttpPost("EnrollUserInEvent")]
        public async Task<string> RegisterUserInEvent(Guid EventId, Guid UserId)
        {
            unitOfWork.Users.EnrollUserInEvent(EventId, UserId);
            await unitOfWork.SaveAsync();
            return "Added";
        }
        [HttpPost("ToggleAdmin")]
        public async Task<string> ToggleAdmin(Guid UserId)
        {
            unitOfWork.Users.ToggleAdmin(UserId);
            await unitOfWork.SaveAsync();
            return "Promoted user to admin";
        }
        [HttpPost("UploadImage")]
        public async Task<string> UploadImage(IFormFile file)
        {
            string fileName;
            if (file != null && file.Length > 0)
            {
                fileName = file.FileName;
                var physPath = environment.ContentRootPath + "/UserPhotos/" + file.FileName;
                using (var stream = new FileStream(physPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            else
            {
                fileName = "default.png";
            }
            return fileName;
        }
        [HttpPost("JWT_Login")]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            IActionResult response = Unauthorized();
            var token = loginUser.Handle(Email, Password);
            if (token != null)
            {
                response = Ok(new { token });
            }
            return response;
        }
        [HttpPost("UpdateUserPfp")]
        public async Task UpdateUserPfp(Guid id, string fileName)
        {
            unitOfWork.Users.UpdateUserPfp(id, fileName);
            await unitOfWork.SaveAsync();
        }


        [HttpPost("AddUserInEvent")]
        [Authorize]
        public async Task AddUserInEvent(Guid EventId, Guid UserId)
        {
            unitOfWork.Users.EnrollUserInEvent(EventId, UserId);
            await unitOfWork.SaveAsync();
        }
    }
}
