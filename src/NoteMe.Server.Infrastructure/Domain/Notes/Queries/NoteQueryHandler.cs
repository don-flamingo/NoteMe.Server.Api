using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteMe.Common.DataTypes.Domain.Notes.Queries;
using NoteMe.Common.Domain.Notes.Dto;
using NoteMe.Common.Domain.Notes.Queries;
using NoteMe.Common.Domain.Pagination;
using NoteMe.Server.Core.Models;
using NoteMe.Server.Infrastructure.Cqrs.Queries;
using NoteMe.Server.Infrastructure.Framework.Mappers;
using NoteMe.Server.Infrastructure.Sql;
using NPag.Extensions;

namespace NoteMe.Server.Infrastructure.Domain.Notes.Queries
{
    public class NoteQueryHandler : 
        IQueryHandler<GetNoteQuery, NoteDto>,
        IQueryHandler<GetNotesQuery, PaginationDto<NoteDto>>,
        IQueryHandler<GetNoteHistoryQuery, PaginationDto<NoteDto>>
    {
        private readonly INoteMeMapper _mapper;
        private readonly NoteMeContext _context;

        public NoteQueryHandler(
            INoteMeMapper mapper,
            NoteMeContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        
        public async Task<NoteDto> HandleAsync(GetNoteQuery query)
        {
            var note = await _context.Notes
                .Include(x => x.Attachments)
                .FirstOrDefaultAsync(x => x.Id == query.Id);
            
            return _mapper.Map<NoteDto>(note);
        }

        public async Task<PaginationDto<NoteDto>> HandleAsync(GetNotesQuery query)
        {
            var queryable = _context.Notes
                .Where(x => x.UserId == query.RequestBy)
                .Where(query);

            var count = await queryable.CountAsync();
            var result = await queryable.TransformBy(query).ToListAsync();
            var pagination = new PaginationDto<Note>
            {
                Data = result,
                TotalCount = count
            };

            return _mapper.Map<PaginationDto<NoteDto>>(pagination);
        }

        public async Task<PaginationDto<NoteDto>> HandleAsync(GetNoteHistoryQuery query)
        {
            var queryable = _context.Notes
                .Where(x => x.UserId == query.RequestBy && x.ActualNoteId == query.Id)
                .Where(query);

            var count = await queryable.CountAsync();
            var result = await queryable.TransformBy(query).ToListAsync();
            var pagination = new PaginationDto<Note>
            {
                Data = result,
                TotalCount = count
            };

            return _mapper.Map<PaginationDto<NoteDto>>(pagination);
        }
    }
}