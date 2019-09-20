using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoteMe.Server.Infrastructure.Commands;

namespace NoteMe.Server.Api.Controllers
{
    [Route("api/[controller]")]
    public abstract class ControllerBase : Controller
    {
        private readonly ICommandDispatcher _commandDispatcher;

        protected ControllerBase(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }
        
        protected Task DispatchAsync<TCommand>(TCommand command)
        {
            return _commandDispatcher.DispatchAsync(command);
        }
    }
}