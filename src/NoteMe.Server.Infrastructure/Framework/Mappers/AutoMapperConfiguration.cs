using AutoMapper;
using NoteMe.Common.Domain.Users.Commands;
using NoteMe.Common.Domain.Users.Dto;
using NoteMe.Server.Core.Models;

namespace NoteMe.Server.Infrastructure.Framework.Mappers
{
    public static class AutoMapperConfiguration 
    {
        public static MapperConfiguration GetConfiguration(bool untiTestingMode = false)
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<UserRegisterCommand, User>()
                    .PreserveReferences()
                    .ReverseMap();

                cfg.CreateMap<User, UserDto>()
                    .PreserveReferences()
                    .ReverseMap();
            });
        }
    }
}