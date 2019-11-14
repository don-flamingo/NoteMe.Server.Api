using System.Threading.Tasks;
using NoteMe.Common.Providers;

namespace NoteMe.Server.Infrastructure.Cqrs.Commands
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommandProvider
    {
        Task HandleAsync(TCommand command);
    }
}