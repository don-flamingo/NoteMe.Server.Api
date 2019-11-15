using System.Threading.Tasks;
using NoteMe.Common.Domain.Notes.Commands;
using NoteMe.Common.Domain.Notes.Dto;
using NoteMe.Server.Core.Models;
using NoteMe.Server.Infrastructure.Cqrs.Commands;

namespace NoteMe.Server.Infrastructure.Domain.Notes.Commands
{
    public class AttachmentCommandHandler : ICommandHandler<CreateAttachmentCommand>,
        ICommandHandler<DeleteAttachmentCommand>
    {
        private readonly IGenericCommandHandler _genericCommandHandler;

        public AttachmentCommandHandler(IGenericCommandHandler genericCommandHandler)
        {
            _genericCommandHandler = genericCommandHandler;
        }

        public Task HandleAsync(CreateAttachmentCommand command)
            => _genericCommandHandler.CreateAsync<CreateAttachmentCommand, Attachment, AttachmentDto>(command);

        public Task HandleAsync(DeleteAttachmentCommand command)
            => _genericCommandHandler.DeleteAsync<Attachment>(command.Id);
    }
}