using Microsoft.AspNetCore.Mvc;
using EventList.Domain.Data;
using EventList.Application.ImageHandler;
using EventList.API.DTO;
using AutoMapper;
using EventList.Infrastructure.CQRS.Commands;
using EventList.Infrastructure.CQRS.Queries;

namespace EventList.API.Structure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(IWebHostEnvironment environment, IMapper mapper,
        UserCommands commands, UserQueries queries, ImageHandler imageHandler) : Controller
    {
        private readonly IWebHostEnvironment environment = environment;
        private readonly IMapper mapper = mapper;
        private readonly UserCommands commands = commands;
        private readonly UserQueries queries = queries;
        private readonly ImageHandler imageHandler = imageHandler;

        [HttpGet("GetUsers")]
        public List<User> GetAllUsers()
        {
            return queries.GetAllUsers();
        }
        [HttpGet("GetUserByGuid")]
        public UserDTO GetUserByGuid(Guid id)
        {
            return mapper.Map<UserDTO>(queries.GetUserByGuid(id).Result);
        }
        [HttpPost("RegisterUserInEvent")]
        public async Task<string> RegisterUserInEvent(Guid EventId, Guid UserId)
        {
            await commands.RegisterUserInEvent(EventId, UserId);
            return "Added";
        }
        [HttpPost("ToggleAdmin")]
        public async Task ToggleAdmin(Guid UserId)
        {
            await commands.ToggleAdmin(await queries.GetUserByGuid(UserId));
        }
        [HttpPost("UploadImage")]
        public async Task<string> UploadImage(IFormFile file)
        {
            return await imageHandler.UploadImage(environment.ContentRootPath, "UserPhotos", file);
        }
        [HttpPost("JWT_Login")]
        public async Task<String> Login(string Email, string Password)
        {
            return await commands.Login(Email, Password);
        }
        [HttpPost("UpdateUserPfp")]
        public async Task UpdateUserPfp(Guid id, string fileName)
        {
            await commands.UpdateUserPfp(await queries.GetUserByGuid(id), fileName);
        }
        [HttpPost("RetractUserFromEvent")]
        public async Task RetractUserFromEvent(Guid EventId, Guid UserId)
        {
            await commands.RetractUserFromEvent(EventId, UserId);
        }
        [HttpGet("GetUsersForThisEvent")]
        //[Authorize]
        public List<User> GetUsersForThisEvent(Guid EventId)
        {
            return queries.GetUsersForThisEvent(EventId);
        }
        [HttpGet("RefreshToken")]
        public async Task<string> RefreshToken(string token)
        {
            return await commands.RefreshToken(token);
        }
        [HttpPost("Logout")]
        public async Task Logout(string token)
        {
            await commands.ClearRefreshToken(await commands.GetRefreshToken(token));
        }
    }
}
