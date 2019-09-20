using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoteMe.Common.Commands;
using NoteMe.Server.Infrastructure.Commands;
using NoteMe.Server.Infrastructure.Services;

namespace NoteMe.Server.Api.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly IMemoryCacheService _memoryCacheService;

        public UsersController(
            IMemoryCacheService memoryCacheService,
            ICommandDispatcher commandDispatcher) 
            : base(commandDispatcher)
        {
            _memoryCacheService = memoryCacheService;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterCommand command)
        {
            await DispatchAsync(command);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginCommand command)
        {
            await DispatchAsync(command);
            var jwt = _memoryCacheService.GetJwt(command.Id);
            return Json(jwt);
        }
    }
}