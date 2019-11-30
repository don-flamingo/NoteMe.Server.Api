using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using NoteMe.Common.DataTypes;
using NoteMe.Common.Domain.Notes.Commands;
using NoteMe.Common.Domain.Notes.Dto;
using NoteMe.Server.Infrastructure.Framework.Generators.Domain;
using NoteMe.Server.Tests.Integration.Fixtures;
using Xunit;

namespace NoteMe.Server.Tests.Integration.Controllers
{
    public class NotesControllerTests : IClassFixture<BackendFixture>
    {
        private readonly BackendFixture _backendFixture;

        public NotesControllerTests(BackendFixture backendFixture)
        {
            _backendFixture = backendFixture;
        }

        [Fact]
        public async Task Notes_ShouldBe_Saved()
        {
            var faker = new Faker();

            var command = new CreateNoteCommand
            {
                Id = Guid.NewGuid(),
                Name = faker.Lorem.Sentence(),
                Content = faker.Lorem.Paragraph(),
                Latitude =  faker.Address.Latitude(),
                Longitude =  faker.Address.Longitude()
            };

            await _backendFixture.LoginAsync(UserSeeder.TestEmail, UserSeeder.TestPassword);
            var noteDto = await _backendFixture.SendAsync<NoteDto>(HttpMethod.Post, Endpoints.Notes._, command);

            noteDto.Id.Should().Be(command.Id);
            noteDto.Name.Should().Be(command.Name);
            noteDto.Content.Should().Be(command.Content);
            noteDto.CreatedAt.Should().NotBe(new DateTime());
            noteDto.UserId.Should().Be(UserSeeder.TestId);
            
            noteDto = await _backendFixture.SendAsync<NoteDto>(HttpMethod.Get, Endpoints.Notes._ + command.Id);

            noteDto.Id.Should().Be(command.Id);
            noteDto.Name.Should().Be(command.Name);
            noteDto.Content.Should().Be(command.Content);
            noteDto.CreatedAt.Should().NotBe(new DateTime());
            noteDto.UserId.Should().Be(UserSeeder.TestId);
        }
    }
}