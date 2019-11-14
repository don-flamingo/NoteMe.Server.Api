using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoteMe.Common.Exceptions;
using NoteMe.Common.Providers;
using NoteMe.Server.Infrastructure.Cqrs.Commands;
using NoteMe.Server.Infrastructure.Cqrs.Queries;
using NoteMe.Server.Infrastructure.Framework.Cache;

namespace NoteMe.Server.Api.Controllers
{
    [Route("api/[controller]")]
    public abstract class NoteMeControllerBase : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICacheService _memoryCacheService;
        private readonly ICommandDispatcher _commandDispatcher;

        protected Guid UserId => User?.Identity?.IsAuthenticated == true
            ? Guid.Parse(User.Identity.Name)
            : Guid.Empty;

        protected NoteMeControllerBase(
            IQueryDispatcher queryDispatcher,
            ICacheService memoryCacheService,
            ICommandDispatcher commandDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _memoryCacheService = memoryCacheService;
            _commandDispatcher = commandDispatcher;
        }

        protected Task<TResult> DispatchQueryAsync<TQuery, TResult>(TQuery query)
            where TQuery : IQueryProvider
        {
            Authorize(query);

            return _queryDispatcher.DispatchAsync<TQuery, TResult>(query);
        }
        
        protected Task DispatchAsync<TCommand>(TCommand command)
            where TCommand : ICommandProvider
        {
            Authorize(command);
            
            return _commandDispatcher.DispatchAsync(command);
        }

        private void Authorize<TRequest>(TRequest query)
        {
            if (query is IAuthenticatedRequestProvider authenticatedRequestProvider)
            {
                authenticatedRequestProvider.RequestBy = UserId;

                if (authenticatedRequestProvider.RequestBy == null)
                {
                    throw new ServerException(ErrorCodes.InvalidCredentials);
                }
            }
        }



        protected TDto GetDto<TDto>(Guid id)
            where TDto : IDtoProvider, IIdProvider
            => _memoryCacheService.Get<TDto>(id);
    }
}