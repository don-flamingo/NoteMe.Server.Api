using NoteMe.Common.Domain.Notes.Commands;
using NoteMe.Common.Domain.Notes.Dto;
using NoteMe.Server.Core.Models;
using NoteMe.Server.Infrastructure.Framework.Mappers;
using NoteMe.Server.Infrastructure.Sql;

namespace NoteMe.Server.Infrastructure.Domain.Notes.Mappers
{
    public class NoteMapperProfile : NoteMeMapperProfile
    {
        public NoteMapperProfile()
        {
            CreateMap<CreateNoteCommand, Note>()
                .ForMember(x => x.UserId, opt => opt.MapFrom(dst => dst.RequestBy))
                .ForMember(x => x.Location,
                    opt => opt.MapFrom(dst => NoteMeGeometryFactory.CreatePoint(dst.Longitude, dst.Latitude)));
            CreateMap<Note, NoteDto>()
                .ForMember(x => x.Latitude,
                    opt => opt.MapFrom(dst => dst.Location.Y))
                .ForMember(x => x.Longitude,
                    opt => opt.MapFrom(dst => dst.Location.X));
                

            CreateMap<CreateAttachmentCommand, Attachment>();
            CreateMap<Attachment, AttachmentDto>();
        }
    }
}