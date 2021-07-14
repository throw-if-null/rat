using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Rat.Api.Test.Controllers.Project
{
    public class DeleteProjectTests : ProjectTestsBase
    {
        public DeleteProjectTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-323)]
        public async Task Should_Return_BadRequest(int id)
        {
            var projectId = id.ToString();

            var response = await Client.DeleteAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public async Task Should_Return_NotFound()
        {
            var projectId = 100.ToString();

            var response = await Client.DeleteAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_Delete()
        {
            var projectId = 42.ToString();

            var response = await Client.DeleteAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
