using AutoMapper;
using NoteMe.Common.Commands;
using NoteMe.Common.Dtos;
using NoteMe.Server.Core.Models;

namespace NoteMe.Server.Infrastructure.Mappers
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