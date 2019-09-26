using System.Threading.Tasks;

namespace NoteMe.Server.Infrastructure.Commands
{
    public interface ICommandHandler<TCommand>
    {
        Task HandleAsync(TCommand command);
    }
}