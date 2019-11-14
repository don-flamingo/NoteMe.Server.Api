using NoteMe.Common.Domain.Users.Dto;
using NoteMe.Server.Core.Models;
using NoteMe.Server.Infrastructure.Framework.Mappers;

namespace NoteMe.Server.Infrastructure.Domain.Users.Mappers
{
    public class UserMapperProfile : NoteMeMapperProfile
    {
        public UserMapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserMapperProfile, User>();
        }   
    }
}