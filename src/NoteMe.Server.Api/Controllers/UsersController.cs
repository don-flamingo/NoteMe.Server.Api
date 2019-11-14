using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoteMe.Common.DataTypes;
using NoteMe.Common.Domain.Users.Commands;
using NoteMe.Common.Domain.Users.Dto;
using NoteMe.Server.Infrastructure.Cqrs.Commands;
using NoteMe.Server.Infrastructure.Cqrs.Queries;
using NoteMe.Server.Infrastructure.Framework.Cache;

namespace NoteMe.Server.Api.Controllers
{
    public class UsersController : NoteMeControllerBase
    {
        public UsersController(
            IQueryDispatcher queryDispatcher,
            ICacheService memoryCacheService,
            ICommandDispatcher commandDispatcher) 
            : base(queryDispatcher, memoryCacheService, commandDispatcher)
        {
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterCommand command)
        {
            await DispatchAsync(command);
            var created = GetDto<UserDto>(command.Id);
            return Created(Endpoints.Users, created);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginCommand command)
        {
            await DispatchAsync(command);
            var jwt = GetDto<JwtDto>(command.Id);
            return Ok(jwt);
        }
    }
}