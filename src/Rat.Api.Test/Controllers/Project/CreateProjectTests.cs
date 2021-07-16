using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rat.Api.Controllers.Projects.Models;
using Rat.Data;
using Rat.Data.Views;
using Snapshooter.Xunit;
using Xunit;

namespace Rat.Api.Test.Controllers.Project
{
    [Collection("Integration")]
    public class CreateProjectTests
    {
        private readonly RatFixture _fixture;

        public CreateProjectTests(RatFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Return_Created()
        {
            using var scope = _fixture.Provider.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<RatDbContext>();
            var projectType = await context.ProjectTypes.FirstOrDefaultAsync(x => x.Name == "js");

            var model = new CreateProjectModel
            {
                Name = "Rat Api",
                TypeId = projectType.Id
            };

            var response = await _fixture.Client.PostAsync(
                "/api/projects",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var contentStream = await response.Content.ReadAsStreamAsync();
            var content = await JsonSerializer.DeserializeAsync<ProjectView>(contentStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

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

            var response = await _fixture.Client.PostAsync(
                "/api/projects",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Snapshot.Match(response.ReasonPhrase, $"{nameof(CreateProjectTests)}.{nameof(Should_Return_BadRequest)}.{version}");
        }
    }
}
