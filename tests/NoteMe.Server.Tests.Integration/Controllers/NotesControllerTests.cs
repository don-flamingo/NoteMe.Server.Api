using System.Threading.Tasks;
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
            
        }
    }
}