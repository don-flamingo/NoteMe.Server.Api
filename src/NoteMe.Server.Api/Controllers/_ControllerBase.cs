using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoteMe.Common.Providers;
using NoteMe.Server.Infrastructure.Commands;
using NoteMe.Server.Infrastructure.Cqrs.Commands.Common;
using NoteMe.Server.Infrastructure.Services;

namespace NoteMe.Server.Api.Controllers
{
    [Route("api/[controller]")]
    public abstract class ControllerBase : Controller
    {
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly ICommandDispatcher _commandDispatcher;

        protected ControllerBase(
            IMemoryCacheService memoryCacheService,
            ICommandDispatcher commandDispatcher)
        {
            _memoryCacheService = memoryCacheService;
            _commandDispatcher = commandDispatcher;
        }
        
        protected Task DispatchAsync<TCommand>(TCommand command)
        {
            return _commandDispatcher.DispatchAsync(command);
        }

        protected TDto GetDto<TDto>(Guid id)
            where TDto : IDtoProvider, IIdProvider
            => _memoryCacheService.GetDto<TDto>(id);
    }
}