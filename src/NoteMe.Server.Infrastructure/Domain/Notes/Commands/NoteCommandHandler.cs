using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteMe.Common.DataTypes.Enums;
using NoteMe.Common.Domain.Notes.Commands;
using NoteMe.Common.Domain.Notes.Dto;
using NoteMe.Server.Core.Models;
using NoteMe.Server.Infrastructure.Cqrs.Commands;
using NoteMe.Server.Infrastructure.Framework.Cache;
using NoteMe.Server.Infrastructure.Framework.Mappers;
using NoteMe.Server.Infrastructure.Sql;

namespace NoteMe.Server.Infrastructure.Domain.Notes.Commands
{
    public class NoteCommandHandler : 
        ICommandHandler<CreateNoteCommand>, 
        ICommandHandler<UpdateNoteCommand>
    {
        private readonly ICacheService _cacheService;
        private readonly INoteMeMapper _mapper;
        private readonly NoteMeContext _noteMeContext;

        public NoteCommandHandler(
            ICacheService cacheService,
            INoteMeMapper mapper,
            NoteMeContext noteMeContext)
        {
            _cacheService = cacheService;
            _mapper = mapper;
            _noteMeContext = noteMeContext;
        }
        
        public async Task HandleAsync(CreateNoteCommand command)
        {
            var note = _mapper.Map<Note>(command);
            await _noteMeContext.AddAsync(note);

            var noteDto = _mapper.Map<NoteDto>(note);
            _cacheService.Set(noteDto);
        }

        public async Task HandleAsync(UpdateNoteCommand command)
        {
            var existingNote = await _noteMeContext.Notes
                .AsTracking()
                .FirstOrDefaultAsync(x => x.Id == command.Id);

            var oldNote = new Note
            {
                Id = Guid.NewGuid(),
                ActualNoteId = existingNote.Id,
                Name = existingNote.Name,
                Content = existingNote.Content,
                Latitude = existingNote.Latitude,
                Status = StatusEnum.Historical
            };

            await _noteMeContext.AddAsync(oldNote);

            existingNote.Content = command.Content;
            existingNote.Name = command.Name;
            existingNote.Latitude = command.Latitude;
            existingNote.Longitude = command.Longitude;
            
            var noteDto = _mapper.Map<NoteDto>(existingNote);
            _cacheService.Set(noteDto);
        }
    }
}