using System.Threading.Tasks;
using Autofac;
using NoteMe.Common.Providers;

namespace NoteMe.Server.Infrastructure.Cqrs.Queries
{
    public interface IQueryDispatcher
    {
        Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query)
            where TQuery : IQueryProvider;
    }
    
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IComponentContext _componentContext;

        public QueryDispatcher(
            IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }
        
        public Task<TResult> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQueryProvider
        {
            var handler = _componentContext.Resolve<IQueryHandler<TQuery, TResult>>();
            return handler.HandleAsync(query);
        }
    }
}