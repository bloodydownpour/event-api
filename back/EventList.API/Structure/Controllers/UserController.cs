using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EventList.Domain.Data;
using EventList.Infrastructure.ImageUploader;
using EventList.API.DTO;
using AutoMapper;
using EventList.Infrastructure.CQRS.Commands;
using EventList.Infrastructure.CQRS.Queries;
using EventList.Application.Exceptions;

namespace EventList.API.Structure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IWebHostEnvironment environment;
        private readonly IMapper mapper;
        private readonly UserCommands commands;
        private readonly UserQueries queries;
        private readonly ImageUploader imageUploader;
        public UserController(IWebHostEnvironment environment, IMapper mapper,
            UserCommands commands, UserQueries queries, ImageUploader imageUploader)
        {
            this.environment = environment;
            this.mapper = mapper;
            this.commands = commands;
            this.queries = queries;
            this.imageUploader = imageUploader;
        }


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
            return await imageUploader.UploadImage(environment.ContentRootPath, "UserPhotos", file);
        }
        [HttpPost("JWT_Login")]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            try
            {
                string token = await commands.Login(Email, Password);
                return Ok(new { token });
            } 
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
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
            await commands.ClearRefreshToken(commands.GetRefreshTokenByToken(token));
        }
    }
}
