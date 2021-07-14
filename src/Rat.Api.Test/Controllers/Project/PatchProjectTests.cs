using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Rat.Api.Controllers.Projects.Models;
using Rat.Data.Views;
using Xunit;

namespace Rat.Api.Test.Controllers.Project
{
    public class PatchProjectTests : ProjectTestsBase
    {
        public PatchProjectTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should_Patch()
        {
            var model = new PatchProjectModel()
            {
                Id = 1,
                Name = "New test"
            };

            var response = await Client.PatchAsync(
                $"/api/projects/{model.Id}",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            var contentStream = await response.Content.ReadAsStreamAsync();
            var content = await JsonSerializer.DeserializeAsync<ProjectView>(contentStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("New test", content.Name);
        }

        [Fact]
        public async Task Should_Not_Patch()
        {
            var model = new PatchProjectModel()
            {
                Id = 1,
                Name = null
            };

            var response = await Client.PatchAsync(
                $"/api/projects/{model.Id}",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            var contentStream = await response.Content.ReadAsStreamAsync();
            var content = await JsonSerializer.DeserializeAsync<ProjectView>(contentStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("Test", content.Name);
        }

        [Fact]
        public async Task Should_Return_NotFound()
        {
            var model = new PatchProjectModel()
            {
                Id = 100,
                Name = "Rat"
            };

            var response = await Client.PatchAsync(
                $"/api/projects/{model.Id}",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
