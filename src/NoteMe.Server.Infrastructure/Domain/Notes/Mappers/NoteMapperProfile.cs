using NoteMe.Common.Domain.Notes.Commands;
using NoteMe.Common.Domain.Notes.Dto;
using NoteMe.Server.Core.Models;
using NoteMe.Server.Infrastructure.Framework.Mappers;

namespace NoteMe.Server.Infrastructure.Domain.Notes.Mappers
{
    public class NoteMapperProfile : NoteMeMapperProfile
    {
        public NoteMapperProfile()
        {
            CreateMap<CreateNoteCommand, Note>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(dst => dst.RequestBy));
            CreateMap<Note, NoteDto>();

            CreateMap<CreateAttachmentCommand, Attachment>();
            CreateMap<Attachment, AttachmentDto>();
        }
    }
}