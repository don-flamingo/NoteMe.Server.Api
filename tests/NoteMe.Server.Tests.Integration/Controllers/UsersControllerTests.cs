using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using NoteMe.Common.DataTypes;
using NoteMe.Common.Domain.Users.Commands;
using NoteMe.Common.Domain.Users.Dto;
using NoteMe.Common.Generators.Users;
using NoteMe.Server.Tests.Integration.Fixtures;
using Xunit;

namespace NoteMe.Server.Tests.Integration.Controllers
{
    public class UsersControllerTests : 
        IClassFixture<UsersGenerator>,
        IClassFixture<BackendFixture>
    {
        private readonly UsersGenerator _usersGenerator;
        private readonly BackendFixture _backendFixture;

        public UsersControllerTests(
            UsersGenerator usersGenerator,
            BackendFixture backendFixture)
        {
            _usersGenerator = usersGenerator;
            _backendFixture = backendFixture;
        }

        [Fact]
        public async Task api_users_should_can_register()
        {
            var cmd = new UserRegisterCommand
            {
                Id = Guid.NewGuid(),
                Email = _usersGenerator.GetEmail(),
                Password = _usersGenerator.GetPassword()
            };

            await _backendFixture.SendAsync<object>(HttpMethod.Post, Endpoints.Users, cmd);
        }
        
        [Fact]
        public async Task api_users_after_register_should_login()
        {
            var cmd = new UserRegisterCommand
            {
                Id = Guid.NewGuid(),
                Email = _usersGenerator.GetEmail(),
                Password = _usersGenerator.GetPassword()
            };

            await _backendFixture.SendAsync<object>(HttpMethod.Post, Endpoints.Users, cmd);

            var login = new LoginCommand
            {
                Id = Guid.NewGuid(),
                Password = cmd.Password,
                Email = cmd.Email
            };
            
            var jwt = await _backendFixture.SendAsync<JwtDto>(HttpMethod.Post, Endpoints.Login, cmd);

            jwt.User.Email.Should().Be(cmd.Email);
            jwt.User.Id.Should().Be(cmd.Id);

        }
    }
}