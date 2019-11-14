using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteMe.Common.Domain.Users.Commands;
using NoteMe.Common.Domain.Users.Dto;
using NoteMe.Common.Exceptions;
using NoteMe.Server.Infrastructure.Cqrs.Commands;
using NoteMe.Server.Infrastructure.Framework.Mappers;
using NoteMe.Server.Infrastructure.Services;
using NoteMe.Server.Infrastructure.Sql;

namespace NoteMe.Server.Infrastructure.Domain.Notes.Commands
{
    public class AuthCommandHandler : ICommandHandler<LoginCommand>
    {
        private readonly IMemoryCacheService _memoryCacheService;
        private readonly IJwtService _jwtService;
        private readonly IEncrypterService _encrypterService;
        private readonly INoteMeMapper _mapper;
        private readonly NoteMeContext _context;

        public AuthCommandHandler(
            IMemoryCacheService memoryCacheService,
            IJwtService jwtService,
            IEncrypterService encrypterService,
            INoteMeMapper mapper,
            NoteMeContext context)
        {
            _memoryCacheService = memoryCacheService;
            _jwtService = jwtService;
            _encrypterService = encrypterService;
            _mapper = mapper;
            _context = context;
        }
        
        public async Task HandleAsync(LoginCommand command)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == command.Email);
            if (user == null)
            {
                throw new ServerException(ErrorCodes.InvalidCredentials);
            }

            var hash = _encrypterService.GetHash(command.Password, user.Salt);
            if (hash != user.Hash)
            {
                throw new ServerException(ErrorCodes.InvalidCredentials);
            }

            var dto = _mapper.Map<UserDto>(user);
            var jwt = _jwtService.CreateToken(dto);

            _memoryCacheService.SetJwt(command.Id, jwt);
        }
    }
}