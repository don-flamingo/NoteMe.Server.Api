using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteMe.Common.DataTypes.Enums;
using NoteMe.Common.Providers;
using NoteMe.Server.Infrastructure.Framework.Cache;
using NoteMe.Server.Infrastructure.Framework.Mappers;
using NoteMe.Server.Infrastructure.Sql;

namespace NoteMe.Server.Infrastructure.Cqrs.Commands
{
    public interface ICommandHandler<TCommand>
        where TCommand : ICommandProvider
    {
        Task HandleAsync(TCommand command);
    }

    public interface IGenericCommandHandler
    {
        Task CreateAsync<TCommand, TEntity, TDto>(TCommand command)
            where TCommand : ICommandProvider
            where TEntity : IIdProvider
            where TDto : IIdProvider;

        Task DeleteAsync<TEntity>(Guid id, bool force = false)
            where TEntity : class, IIdProvider;
    }

    public class GenericCommandHandler : IGenericCommandHandler
    {
        private readonly ICacheService _cacheService;
        private readonly INoteMeMapper _mapper;
        private readonly NoteMeContext _context;

        public GenericCommandHandler(
            ICacheService cacheService,
            INoteMeMapper mapper,
            NoteMeContext context)
        {
            _cacheService = cacheService;
            _mapper = mapper;
            _context = context;
        }

        public async Task CreateAsync<TCommand, TEntity, TDto>(TCommand command)
            where TCommand : ICommandProvider
            where TEntity : IIdProvider
            where TDto : IIdProvider
        {
            var entity = _mapper.Map<TEntity>(command);
            await _context.AddAsync(entity);

            var dto = _mapper.Map<TDto>(entity);
            _cacheService.Set(dto);
        }

        public async Task DeleteAsync<TEntity>(Guid id, bool force = false) where TEntity : class, IIdProvider
        {
            var entity = await _context.Set<TEntity>().AsTracking().FirstOrDefaultAsync(x => x.Id == id);
            
            if (!force && entity is IStatusProvider statusProvider)
            {
                statusProvider.Status = StatusEnum.Archived;
            }
            else
            {
                _context.Remove(entity);
            }
        }
    }
}