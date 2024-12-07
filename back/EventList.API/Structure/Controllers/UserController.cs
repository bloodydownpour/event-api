using Microsoft.AspNetCore.Mvc;
using EventList.Domain.Data;
using EventList.Application.ImageProcessing;
using EventList.API.DTO;
using AutoMapper;
using EventList.Infrastructure.CQRS.Queries;
using EventList.Application.CQRS.Commands;
using EventList.Domain.CommandData;
using Microsoft.AspNetCore.Authorization;
using EventList.Domain.QueryData;

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
            return mapper.Map<UserDTO>(queries.GetUserByGuid(new GetUserByGuidQueryData { Id = id }).Result);
        }
        [HttpPost("RegisterUserInEvent")]
        public async Task<string> RegisterUserInEvent(Guid EventId, Guid UserId)
        {
            await commands.RegisterUserInEvent(new RegisterUserInEventCommandData { EventId = EventId, UserId = UserId});
            return "Added";
        }
        [HttpPost("ToggleAdmin")]
        public async Task ToggleAdmin(Guid UserId)
        {
            await commands.ToggleAdmin(new ToggleAdminCommandData { 
                User = await queries.GetUserByGuid(new GetUserByGuidQueryData { Id = UserId }) 
            });
        }
        [HttpPost("UploadImage")]
        public async Task<string> UploadImage(IFormFile file)
        {
            return await imageHandler.UploadImage(environment.ContentRootPath, "UserPhotos", file);
        }
        [HttpPost("JWT_Login")]
        public async Task<String> Login(string Email, string Password)
        {
            return await commands.Login(new LoginCommandData { Email = Email, Password = Password});
        }
        [HttpPost("UpdateUserPfp")]
        public async Task UpdateUserPfp(Guid id, string fileName)
        {
            await commands.UpdateUserPfp(new UpdateUserPfpCommandData { User = await queries.GetUserByGuid(
                new GetUserByGuidQueryData { Id = id }), FileName = fileName });
        }
        [HttpPost("RetractUserFromEvent")]
        public async Task RetractUserFromEvent(Guid EventId, Guid UserId)
        {
            await commands.RetractUserFromEvent(new RetractUserFromEventCommandData { EventId = EventId, UserId = UserId });
        }
        [HttpGet("GetUsersForThisEvent")]
        [Authorize]
        public List<User> GetUsersForThisEvent(Guid EventId)
        {
            return queries.GetUsersForThisEvent(new GetUsersForThisEventQueryData { EventId = EventId });
        }
        [HttpGet("RefreshToken")]
        public async Task<string> RefreshToken(string token)
        {
            return await commands.RefreshToken(new RefreshTokenCommandData { Token = token});
        }
        [HttpPost("Logout")]
        public async Task Logout(string token)
        {
            await commands.ClearRefreshToken(new ClearRefreshTokenCommandData { RefreshToken = await commands.GetRefreshToken(new GetRefreshTokenCommandData { AccessToken = token }) });
        }
    }
}
