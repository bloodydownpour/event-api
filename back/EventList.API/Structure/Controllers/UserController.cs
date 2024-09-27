using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventList.Domain.Data;
using EventList.Infrastructure.Database;
using EventList.Persistence.JWT;
using EventList.API.DTO;
using AutoMapper;
using EventList.Infrastructure.CQRS.Commands;
using EventList.Infrastructure.CQRS.Queries;

namespace EventList.API.Structure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly LoginUser loginUser;
        private readonly IWebHostEnvironment environment;
        private readonly IMapper mapper;
        private readonly UserCommands commands;
        private readonly UserQueries queries;
        public UserController(LoginUser loginUser, IWebHostEnvironment environment, IMapper mapper,
            UserCommands commands, UserQueries queries)
        {
            this.loginUser = loginUser;
            this.environment = environment;
            this.mapper = mapper;
            this.commands = commands;
            this.queries = queries;
        }


        [HttpGet("GetUsers")]
        public List<User> GetAllUsers()
        {
            return queries.GetAllUsers();
        }
        [HttpGet("GetUserByGuid")]
        public UserDTO GetUserByGuid(Guid id)
        {
            return mapper.Map<UserDTO>(queries.GetUserByGuid(id));
        }
        [HttpPost("EnrollUserInEvent")]
        public async Task<string> RegisterUserInEvent(Guid EventId, Guid UserId)
        {
            await commands.RegisterUserInEvent(EventId, UserId);
            return "Added";
        }
        [HttpPost("ToggleAdmin")]
        public async Task<string> ToggleAdmin(Guid UserId)
        {
            await commands.ToggleAdmin(UserId);
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
            await commands.UpdateUserPfp(id, fileName);
        }


        [HttpPost("AddUserInEvent")]
        [Authorize]
        public async Task AddUserInEvent(Guid EventId, Guid UserId)
        {
            await commands.AddUserInEvent(EventId, UserId);
        }
        [HttpPost("RetractUserFromEvent")]
        public async Task RetractUserFromEvent(Guid EventId, Guid UserId)
        {
            await commands.RetractUserFromEvent(EventId, UserId);
        }
        [HttpGet("GetUsersForThisEvent")]
        [Authorize]
        public List<User> GetUsersForThisEvent(Guid EventId)
        {
            return queries.GetUsersForThisEvent(EventId);
        }
        
    }
}
