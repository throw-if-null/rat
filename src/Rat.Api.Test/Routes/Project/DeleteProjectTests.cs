using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rat.Sql;
using Xunit;

namespace Rat.Api.Test.Routes.Project
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

			var projectTypeId = await connection.ProjectTypeGetByName("js");
			var project = await connection.ProjectInsert("Test", projectTypeId, 1, CancellationToken.None);
			int projectId = project.Id;
			await connection.ProjectDelete(projectId, 1, CancellationToken.None);

            var response = await _fixture.Client.DeleteAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_Delete()
        {
            using var scope = _fixture.Provider.CreateScope();
            var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
			await using var connection = connectionFactory.CreateConnection();

			var projectTypeId = await connection.ProjectTypeGetByName("js");

			var project = await connection.ProjectInsert("Test", projectTypeId, 1, CancellationToken.None);
			int projectId = project.Id;

			var response = await _fixture.Client.DeleteAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
