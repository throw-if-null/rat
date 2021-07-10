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

        [Fact]
        public async Task Should_Return_NotFound()
        {
            var projectId = 0.ToString();

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
