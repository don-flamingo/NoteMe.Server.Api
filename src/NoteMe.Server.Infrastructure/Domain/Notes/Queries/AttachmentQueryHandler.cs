using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using N.Pag.Extensions;
using NoteMe.Common.Domain.Notes.Dto;
using NoteMe.Common.Domain.Notes.Queries;
using NoteMe.Common.Domain.Pagination;
using NoteMe.Server.Core.Models;
using NoteMe.Server.Infrastructure.Cqrs.Queries;
using NoteMe.Server.Infrastructure.Framework.Mappers;
using NoteMe.Server.Infrastructure.Sql;
using System.Linq;

namespace NoteMe.Server.Infrastructure.Domain.Notes.Queries
{
    public class AttachmentQueryHandler : IQueryHandler<GetAttachmentQuery, PaginationDto<AttachmentDto>>
    {
        private readonly INoteMeMapper _mapper;
        private readonly NoteMeContext _context;

        public AttachmentQueryHandler(
            INoteMeMapper mapper,
            NoteMeContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        
        public async Task<PaginationDto<AttachmentDto>> HandleAsync(GetAttachmentQuery query)
        {
            var queryable = _context.Attachments
                .Where(x => x.Note.UserId == query.RequestBy)
                .Where(query);

            var count = await queryable.CountAsync();
            var result = await queryable.TransformBy(query).ToListAsync();
            var pagination = new PaginationDto<Attachment>
            {
                Data = result,
                TotalCount = count
            };

            return _mapper.Map<PaginationDto<AttachmentDto>>(pagination);
        }
    }
}