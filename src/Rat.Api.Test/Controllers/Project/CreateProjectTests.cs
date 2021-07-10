using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Rat.Api.Controllers.Projects.Models;
using Snapshooter.Xunit;
using Xunit;

namespace Rat.Api.Test.Controllers.Project
{
    public class CreateProjectTests : ProjectTestsBase
    {
        public CreateProjectTests(CustomWebApplicationFactory<Startup> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_Return_Created()
        {
            var model = new CreateProjectModel
            {
                Name = "Rat Api",
                Type = "csharp"
            };

            var response = await Client.PostAsync(
                "/api/projects",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var contentStream = await response.Content.ReadAsStreamAsync();
            var content = await JsonSerializer.DeserializeAsync<ProjectModel>(contentStream);

            Assert.True(content.Id > 0);
        }

        [Theory]
        [InlineData("", "js", "1")]
        [InlineData("rat", "", "2")]
        [InlineData("cat", null, "3")]
        [InlineData(null, "csharp", "4")]
        [InlineData(null, null, "5")]
        [InlineData("", "", "6")]
        public async Task Should_Return_BadRequest(string name, string type, string version)
        {
            var model = new CreateProjectModel
            {
                Name = name,
                Type = type
            };

            var response = await Client.PostAsync(
                "/api/projects",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Snapshot.Match(response.ReasonPhrase, $"{nameof(CreateProjectTests)}.{nameof(Should_Return_BadRequest)}.{version}");
        }
    }
}
