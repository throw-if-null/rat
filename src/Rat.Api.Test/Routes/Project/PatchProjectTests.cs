using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rat.Api.Routes.Data;
using Rat.Sql;
using Xunit;

namespace Rat.Api.Test.Routes.Project
{
	[Collection("Integration")]
    public class PatchProjectTests
    {
        private readonly RatFixture _fixture;

        public PatchProjectTests(RatFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Patch()
        {
            using var scope = _fixture.Provider.CreateScope();
			var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
			await using var connection = connectionFactory.CreateConnection();

			var jsTypeId = await connection.ProjectTypeGetByName("js");
			var csTypeId = await connection.ProjectTypeGetByName("csharp");

			var project = await connection.ProjectInsert("Patch", jsTypeId, 1, CancellationToken.None);
			int projectId = project.Id;

            var model = new PatchProjectRouteInput(projectId, "New test", csTypeId);

            var response = await _fixture.Client.PatchAsync(
                $"/api/projects/{model.Id}",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            var contentStream = await response.Content.ReadAsStreamAsync();
            var output = await JsonSerializer.DeserializeAsync<PatchProjectRouteOutput>(contentStream, _fixture.JsonSerializerOption);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("New test", output.Name);
            Assert.Equal(csTypeId, output.TypeId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("sqoyhhpamcsnpwzjdwwneydgighyecnwpykbtbugmqclefuhndqpvnfhupwaofgnwlehtwfujyrlavgubnuvqrjdbbanpwvnaneembgplatqvselnwvfefezxznyvnqkdqaalqwyjmlskovuowehyaujnhevlpcgtxhfwwbiwsuozfmeishfnovyteddvyxfmclwiekfqjmelujrevprrsctksqkvnzwqwksibojrnhmcftdjnogsrmane")]
        public async Task Should_Return_BadRequest_When_Name_Value_Is_Invalid(string name)
        {
            using var scope = _fixture.Provider.CreateScope();
			var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
			await using var connection = connectionFactory.CreateConnection();

			var projectTypeId = await connection.ProjectTypeGetByName("js");

			var project = await connection.ProjectInsert("Patch", projectTypeId, 1, CancellationToken.None);
			int projectId = project.Id;

			var model = new PatchProjectRouteInput(projectId, name, projectTypeId);

			var response = await _fixture.Client.PatchAsync(
                $"/api/projects/{model.Id}",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_Return_NotFound()
        {
            using var scope = _fixture.Provider.CreateScope();
			var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
			await using var connection = connectionFactory.CreateConnection();

			var projectTypeId = await connection.ProjectTypeGetByName("js");
			var project = await connection.ProjectInsert("Patch", projectTypeId, 1, CancellationToken.None);
			int projectId = project.Id;
			await connection.ProjectDelete(projectId, 1, CancellationToken.None);

			var model = new PatchProjectRouteInput(projectId, "Rat", projectTypeId);

            var response = await _fixture.Client.PatchAsync(
                $"/api/projects/{model.Id}",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
