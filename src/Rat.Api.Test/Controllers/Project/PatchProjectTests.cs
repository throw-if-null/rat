using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Rat.Api.Controllers.Projects.Models;
using Xunit;

namespace Rat.Api.Test.Controllers.Project
{
    public class PatchProjectTests : ProjectTestsBase
    {
        public PatchProjectTests(CustomWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Theory]
        [InlineData("rat", null)]
        [InlineData(null, "js")]
        [InlineData("rat", "js")]
        public async Task Should_Patch_Only_Not_Null_Properties(string name, string type)
        {
            // Seed and retrieve
            var model = new PatchProjectModel
            {
                Id = 1,
                Name = "Test",
                Type = "Test"
            };

            var patchModel = model with { Name = name, Type = type };

            var response = await Client.PatchAsync(
                "api/projects/",
                new StringContent(JsonSerializer.Serialize(patchModel), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var contentStream = await response.Content.ReadAsStreamAsync();
            var content = await JsonSerializer.DeserializeAsync<ProjectModel>(contentStream);

            Assert.Equal(content.Name, model.Name);
            Assert.Equal(content.Type, model.Type);
        }

        [Fact]
        public async Task Should_Not_Do_Patch_And_Return_NoContent()
        {
            var model = new PatchProjectModel()
            {
                Id = 1,
                Name = null,
                Type = null
            };

            var response = await Client.PostAsync(
                "/api/projects",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
