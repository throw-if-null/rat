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
                Name = "Rat Api"
            };

            var response = await Client.PostAsync(
                "/api/projects",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var contentStream = await response.Content.ReadAsStreamAsync();
            var content = await JsonSerializer.DeserializeAsync<Data.Entities.Project>(contentStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.True(content.Id > 0);
        }

        [Theory]
        [InlineData("", "1")]
        [InlineData(null, "2")]
        public async Task Should_Return_BadRequest(string name, string version)
        {
            var model = new CreateProjectModel
            {
                Name = name
            };

            var response = await Client.PostAsync(
                "/api/projects",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Snapshot.Match(response.ReasonPhrase, $"{nameof(CreateProjectTests)}.{nameof(Should_Return_BadRequest)}.{version}");
        }
    }
}
