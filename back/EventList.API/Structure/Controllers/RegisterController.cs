using EventList.Domain.Data;
using Microsoft.AspNetCore.Mvc;
using EventList.Application.CQRS.Commands;
using EventList.Domain.CommandData;

namespace EventList.API.Structure.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegisterController(UserCommands commands) : Controller
    {
        private readonly UserCommands commands = commands;
        [HttpPost("RegisterUser")]
        public async Task AddUser(User user)
        {
            await commands.AddUser(new AddUserCommandData { User = user });
        }
    }
}
