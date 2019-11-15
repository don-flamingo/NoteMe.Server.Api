using NoteMe.Server.Infrastructure.Cqrs.Commands;
using NoteMe.Server.Infrastructure.Cqrs.Queries;
using NoteMe.Server.Infrastructure.Framework.Cache;

namespace NoteMe.Server.Api.Controllers
{
    public class AttachmentsController : NoteMeControllerBase
    {
        public AttachmentsController(IQueryDispatcher queryDispatcher, ICacheService memoryCacheService, ICommandDispatcher commandDispatcher) : base(queryDispatcher, memoryCacheService, commandDispatcher)
        {
        }
        
        
    }
}