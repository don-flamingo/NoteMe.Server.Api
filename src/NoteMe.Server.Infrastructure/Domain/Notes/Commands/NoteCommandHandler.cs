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
        ICommandHandler<UpdateNoteCommand>,
        ICommandHandler<DeleteNoteCommand>
    {
        private readonly IGenericCommandHandler _genericCommandHandler;
        private readonly ICacheService _cacheService;
        private readonly INoteMeMapper _mapper;
        private readonly NoteMeContext _noteMeContext;

        public NoteCommandHandler(
            IGenericCommandHandler genericCommandHandler,
            ICacheService cacheService,
            INoteMeMapper mapper,
            NoteMeContext noteMeContext)
        {
            _genericCommandHandler = genericCommandHandler;
            _cacheService = cacheService;
            _mapper = mapper;
            _noteMeContext = noteMeContext;
        }

        public Task HandleAsync(CreateNoteCommand command)
            => _genericCommandHandler.CreateAsync<CreateNoteCommand, Note, NoteDto>(command);

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
                Status = StatusEnum.Historical,
                UserId = existingNote.UserId
            };

            await _noteMeContext.AddAsync(oldNote);

            existingNote.Content = command.Content;
            existingNote.Name = command.Name;
            existingNote.Latitude = command.Latitude;
            existingNote.Longitude = command.Longitude;
            
            var noteDto = _mapper.Map<NoteDto>(existingNote);
            _cacheService.Set(noteDto);
        }

        public Task HandleAsync(DeleteNoteCommand command)
            => _genericCommandHandler.DeleteAsync<NoteDto>(command.Id);
    }
}