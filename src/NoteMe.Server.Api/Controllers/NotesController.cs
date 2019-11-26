using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoteMe.Common.DataTypes.Domain.Notes.Queries;
using NoteMe.Common.Domain.Notes.Commands;
using NoteMe.Common.Domain.Notes.Dto;
using NoteMe.Common.Domain.Notes.Queries;
using NoteMe.Common.Domain.Pagination;
using NoteMe.Server.Infrastructure.Cqrs.Commands;
using NoteMe.Server.Infrastructure.Cqrs.Queries;
using NoteMe.Server.Infrastructure.Framework.Cache;

namespace NoteMe.Server.Api.Controllers
{
    public class NotesController : NoteMeControllerBase
    {
        public NotesController(IQueryDispatcher queryDispatcher, ICacheService memoryCacheService, ICommandDispatcher commandDispatcher) : base(queryDispatcher, memoryCacheService, commandDispatcher)
        {
        
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateNoteCommand command)
        {
            await DispatchAsync(command);
            var created = GetDto<NoteDto>(command.Id);
            return Ok(created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] UpdateNoteCommand command)
        {
            command.Id = id;
            await DispatchAsync(command);
            var updated = GetDto<NoteDto>(command.Id);
            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var cmd = new DeleteNoteCommand
            {
                Id = id
            };

            await DispatchAsync(cmd);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync([FromQuery] GetNotesQuery query)
        {
            var notes = await DispatchQueryAsync<GetNotesQuery, PaginationDto<NoteDto>>(query);
            return Ok(notes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var query = new GetNoteQuery {Id = id};
            var note = await DispatchQueryAsync<GetNoteQuery, NoteDto>(query);
            return Ok(note);
        }
        
        [HttpGet("history/{id}")]
        public async Task<IActionResult> GetHistoryAsync(Guid id)
        {
            var query = new GetNoteHistoryQuery {Id = id};
            var notes = await DispatchQueryAsync<GetNoteHistoryQuery, PaginationDto<NoteDto>>(query);
            return Ok(notes);
        }
    }
}