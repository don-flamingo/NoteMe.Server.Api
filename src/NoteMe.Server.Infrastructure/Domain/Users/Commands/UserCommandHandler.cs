using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteMe.Common.Domain.Users.Commands;
using NoteMe.Common.Domain.Users.Dto;
using NoteMe.Common.Exceptions;
using NoteMe.Server.Core.Models;
using NoteMe.Server.Infrastructure.Cqrs.Commands;
using NoteMe.Server.Infrastructure.Framework.Cache;
using NoteMe.Server.Infrastructure.Framework.Mappers;
using NoteMe.Server.Infrastructure.Framework.Security;
using NoteMe.Server.Infrastructure.Sql;

namespace NoteMe.Server.Infrastructure.Domain.Users.Commands
{
    public class UserCommandHandler : ICommandHandler<UserRegisterCommand>
    {
        private readonly ICacheService _memoryCacheService;
        private readonly ISecurityService _encrypterService;
        private readonly INoteMeMapper _mapper;
        private readonly NoteMeContext _context;

        public UserCommandHandler(
            ICacheService memoryCacheService,
            ISecurityService encrypterService,
            INoteMeMapper mapper,
            NoteMeContext context)
        {
            _memoryCacheService = memoryCacheService;
            _encrypterService = encrypterService;
            _mapper = mapper;
            _context = context;
        }
        
        public async Task HandleAsync(UserRegisterCommand command)
        {
            var user = _mapper.Map<User>(command);
            
            var isExists = await _context.Users.AnyAsync(x => x.Email == command.Email);
            if (isExists)
            {
                throw new ServerException(ErrorCodes.UserAlreadyExists);
            }

            user.Salt = _encrypterService.GetSalt();
            user.Hash = _encrypterService.GetHash(command.Password, user.Salt);

            await _context.AddAsync(user);
            
            var created = _mapper.Map<UserDto>(user);
            _memoryCacheService.Set(created);

        } 
    }
}