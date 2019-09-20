using System;
using System.Threading.Tasks;
using Autofac;
using NoteMe.Server.Infrastructure.Sql;

namespace NoteMe.Server.Infrastructure.Commands
{
    public interface ICommandDispatcher 
    {
        Task DispatchAsync<TCommand>(TCommand command);
    }
    
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly NoteMeContext _context;
        private readonly IContainer _container;

        public CommandDispatcher(NoteMeContext context,
            IContainer container)
        {
            _context = context;
            _container = container;
        }
        
        public async Task DispatchAsync<TCommand>(TCommand command)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var handler = _container.Resolve<ICommandHandler<TCommand>>();

                    await handler.HandleAsync(command);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}