using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rat.Api.Routes.Data;
using Rat.DataAccess;
using Rat.DataAccess.Entities;
using Xunit;

namespace Rat.Api.Test.Controllers.Project
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

			var command = new CommandDefinition("SELECT Id, Name FROM ProjectType");
			var projectTypes = await connection.QueryAsync<ProjectTypeEntity>(command);

			var jsType = projectTypes.First(x => x.Name == "js");
            var csharpType = projectTypes.First(x => x.Name == "csharp");

			command = new CommandDefinition(
				"",
				new { Name = "Patch", ProjectTypeId = jsType.Id });

			var projectId = await connection.QuerySingleAsync<int>(command);

            var model = new PatchProjectRouteInput(projectId, "New test", csharpType.Id);

            var response = await _fixture.Client.PatchAsync(
                $"/api/projects/{model.Id}",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            var contentStream = await response.Content.ReadAsStreamAsync();
            var content = await JsonSerializer.DeserializeAsync<PatchProjectRouteOutput>(contentStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("New test", content.Name);
            Assert.Equal(csharpType.Id, content.TypeId);
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

			var command = new CommandDefinition(
				"SELECT Id FROM ProjectType WHERE Name = @Name",
				new { Name = "js" });

			var projectTypeId = await connection.QuerySingleAsync<int>(command);

			command = new CommandDefinition(
				"INSERT INTO Project (Name, ProjectTypeId) VALUES(@Name, @ProjectTypeId)",
				new { Name = "Patch", ProjectTypeId = projectTypeId });

			var projectId = await connection.QuerySingleAsync<int>(command);

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

			var command = new CommandDefinition(
				"SELECT Id FROM ProjectType WHERE Name = @Name",
				new { Name = "js" });

			var projectTypeId = await connection.QuerySingleAsync<int>(command);

			command = new CommandDefinition(
				"INSERT INTO Project (Name, ProjectTypeId) VALUES(@Name, @ProjectTypeId)",
				new { Name = "Patch", ProjectTypeId = projectTypeId });

			var projectId = await connection.QuerySingleAsync<int>(command);

			command = new CommandDefinition(
				"DELETE FROM Project WHERE Id = @Id",
				new { Id = projectId });

			await connection.ExecuteAsync(command);

			var model = new PatchProjectRouteInput(projectId, "Rat", projectTypeId);

            var response = await _fixture.Client.PatchAsync(
                $"/api/projects/{model.Id}",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
