using System.Threading.Tasks;
using NoteMe.Common.Providers;

namespace NoteMe.Server.Infrastructure.Cqrs.Queries
{
    public interface IQueryHandler<in TQuery, TResult>
        where TQuery : IQueryProvider
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}