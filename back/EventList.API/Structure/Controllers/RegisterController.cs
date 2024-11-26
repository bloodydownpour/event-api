using EventList.Domain.Data;
using Microsoft.AspNetCore.Mvc;
using EventList.Infrastructure.CQRS.Commands;

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
            await commands.AddUser(user);
        }
    }
}
