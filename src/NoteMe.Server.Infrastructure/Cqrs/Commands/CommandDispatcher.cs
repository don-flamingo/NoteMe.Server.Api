using System;
using System.Threading.Tasks;
using Autofac;
using NoteMe.Common.Providers;
using NoteMe.Server.Infrastructure.Sql;

namespace NoteMe.Server.Infrastructure.Cqrs.Commands
{
    public interface ICommandDispatcher
    {
        Task DispatchAsync<TCommand>(TCommand command)
            where TCommand : ICommandProvider;
    }
    
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly NoteMeContext _context;
        private readonly IComponentContext _container;

        public CommandDispatcher(NoteMeContext context,
            IComponentContext container)
        {
            _context = context;
            _container = container;
        }

        public async Task DispatchAsync<TCommand>(TCommand command)
            where TCommand : ICommandProvider
        {
            var handler = _container.Resolve<ICommandHandler<TCommand>>();
            await handler.HandleAsync(command);

            await _context.SaveChangesAsync();
        }
    }
}