using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NoteMe.Common.Domain.Users.Commands;
using NoteMe.Common.Domain.Users.Dto;
using NoteMe.Common.Exceptions;
using NoteMe.Server.Infrastructure.Cqrs.Commands;
using NoteMe.Server.Infrastructure.Framework.Cache;
using NoteMe.Server.Infrastructure.Framework.Mappers;
using NoteMe.Server.Infrastructure.Framework.Security;
using NoteMe.Server.Infrastructure.Sql;

namespace NoteMe.Server.Infrastructure.Domain.Users.Commands
{
    public class AuthCommandHandler : ICommandHandler<LoginCommand>
    {
        private readonly ISecurityService _securityService;
        private readonly ICacheService _cacheService;
        private readonly INoteMeMapper _mapper;
        private readonly NoteMeContext _context;

        public AuthCommandHandler(
            ISecurityService securityService,
            ICacheService cacheService,
            INoteMeMapper mapper,
            NoteMeContext context)
        {
            _securityService = securityService;
            _cacheService = cacheService;
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

            var hash = _securityService.GetHash(command.Password, user.Salt);
            if (hash != user.Hash)
            {
                throw new ServerException(ErrorCodes.InvalidCredentials);
            }

            var dto = _mapper.Map<UserDto>(user);
            var jwt = _securityService.GetJwt(dto, command.Id);

            _cacheService.Set(jwt);
        }
    }
}