using System.Net;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rat.DataAccess;
using Xunit;

namespace Rat.Api.Test.Controllers.Project
{
	[Collection("Integration")]
    public class DeleteProjectTests
    {
        private readonly RatFixture _fixture;

        public DeleteProjectTests(RatFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-323)]
        public async Task Should_Return_BadRequest(int id)
        {
            var projectId = id.ToString();

            var response = await _fixture.Client.DeleteAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public async Task Should_Return_NotFound()
        {
            using var scope = _fixture.Provider.CreateScope();
			var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
			await using var connection = connectionFactory.CreateConnection();

			var command = new CommandDefinition("SELECT Id FROM ProjectType WHERE Name == @Name", new { Name = "js" });
			var projectTypeId = await connection.QuerySingleAsync<int>(command);

			command = new CommandDefinition(
				"",
				new { Name = "Test", Type = projectTypeId });

			var projectId = await connection.QuerySingleAsync<int>(command);

			command = new CommandDefinition(
				"DELETE FROM Project WHERE Id = @Id",
				new { Id = projectId });

			await connection.ExecuteAsync(command);

            var response = await _fixture.Client.DeleteAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_Delete()
        {
            using var scope = _fixture.Provider.CreateScope();
            var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
			await using var connection = connectionFactory.CreateConnection();

			var command = new CommandDefinition("SELECT Id FROM ProjectType WHERE Name == @Name", new { Name = "js" });
			var projectTypeId = await connection.QuerySingleAsync<int>(command);

			command = new CommandDefinition(
				"",
				new { Name = "Test", Type = projectTypeId });

			var projectId = await connection.QuerySingleAsync<int>(command);

			var response = await _fixture.Client.DeleteAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
