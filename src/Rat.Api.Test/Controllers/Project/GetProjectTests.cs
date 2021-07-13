using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Rat.Data.Views;
using Snapshooter.Xunit;
using Xunit;

namespace Rat.Api.Test.Controllers.Project
{
    public class GetProjectTests : ProjectTestsBase
    {
        public GetProjectTests(CustomWebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_Get_Project_By_Id()
        {
            // Seed the real data
            var projectId = 42.ToString();

            var response = await Client.GetAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var contentStream = await response.Content.ReadAsStreamAsync();
            var content = JsonSerializer.DeserializeAsync<Data.Views.UserProjectStats>(contentStream);

            Snapshot.Match(content);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-4)]
        public async Task Should_Return_BadRequest(int id)
        {
            var projectId = id.ToString();

            var response = await Client.GetAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_Return_Projects_For_User()
        {
            var response = await Client.GetAsync("/api/projects/");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            using (var content = await response.Content.ReadAsStreamAsync())
            {
                UserProjectStats projects =
                    await JsonSerializer.DeserializeAsync<UserProjectStats>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Snapshot.Match(projects);
            }
        }

        [Fact(Skip = "Need to fixup claims for this to work")]
        public async Task Should_Return_NotFound_When_User_Is_Not_In_Database()
        {
            var response = await Client.GetAsync($"/api/projects");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
