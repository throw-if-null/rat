using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rat.Api.Test.Controllers.Routes;
using Rat.Sql;
using Snapshooter.Xunit;
using Xunit;

namespace Rat.Api.Test.Routes.Configuration
{
	[Collection("Integration")]
	public class DeleteConfigurationTests
	{
		private readonly RatFixture _fixture;

		public DeleteConfigurationTests(RatFixture fixture)
		{
			_fixture = fixture;
		}

		[Fact]
		public async Task Should_Delete_Configuration()
		{
			using var scope = _fixture.Provider.CreateScope();
			var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();

			await using var connection = connectionFactory.CreateConnection();
			var configurationTypeId = await connection.ConfigurationTypeGetByName("api");
			var projectTypeId = await connection.ProjectTypeGetByName("js");
			var project = await connection.ProjectInsert("Project A", projectTypeId, 1, CancellationToken.None);
			int projectId = project.Id;
		    var id = await connection.ConfigurationRootInsert(projectId, "WillBeDeleted", configurationTypeId, 1, CancellationToken.None);

			var response = await _fixture.Client.DeleteAsync($"/api/projects/{projectId}/configurations/{id}");

			Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
		}

		[Fact]
		public async Task Should_Retrun_NotFound_When_Project_Is_Missing()
		{
			using var scope = _fixture.Provider.CreateScope();
			var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();

			await using var connection = connectionFactory.CreateConnection();
			var configurationTypeId = await connection.ConfigurationTypeGetByName("api");
			var projectTypeId = await connection.ProjectTypeGetByName("js");
			var project = await connection.ProjectInsert("Project A", projectTypeId, 1, CancellationToken.None);
			int projectId = project.Id;
			await connection.ProjectDelete(projectId, 1, CancellationToken.None);

			var response = await _fixture.Client.DeleteAsync($"/api/projects/{projectId}/configurations/1");

			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Fact]
		public async Task Should_Return_NotFound()
		{
			using var scope = _fixture.Provider.CreateScope();
			var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();

			await using var connection = connectionFactory.CreateConnection();
			var configurationTypeId = await connection.ConfigurationTypeGetByName("api");
			var projectTypeId = await connection.ProjectTypeGetByName("js");
			var project = await connection.ProjectInsert("Project A", projectTypeId, 1, CancellationToken.None);
			int projectId = project.Id;
			var configurationId = await connection.ConfigurationRootInsert(projectId, "Deleted", configurationTypeId, 1, CancellationToken.None);
			await connection.ConfigurationRootDelete(configurationId, 1, CancellationToken.None);

			var response = await _fixture.Client.DeleteAsync($"/api/projects/{projectId}/configurations/1");

			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
		}

		[Theory]
		[InlineData(0, 1, "1")]
		[InlineData(-1, 1, "2")]
		[InlineData(1, 0, "3")]
		[InlineData(1, -1, "4")]
		public async Task Should_Return_BadRequest(int id, int projectId, string version)
		{
			var response = await _fixture.Client.DeleteAsync($"/api/projects/{projectId}/configurations/{id}");
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

			var content = await response.Content.ReadAsStringAsync();
			Snapshot.Match(content, $"{nameof(DeleteConfigurationTests)}.{nameof(Should_Return_BadRequest)}.{version}");
		}

		[Fact]
		public async Task Should_Return_BadRequest_When_Configuration_Does_Not_Belong_To_Project()
		{
			using var scope = _fixture.Provider.CreateScope();
			var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();

			await using var connection = connectionFactory.CreateConnection();
			var configurationTypeId = await connection.ConfigurationTypeGetByName("api");
			var projectTypeId = await connection.ProjectTypeGetByName("js");
			var project = await connection.ProjectInsert("Project A", projectTypeId, 1, CancellationToken.None);
			int projectId = project.Id;
			var configurationId = await connection.ConfigurationRootInsert(projectId, "Deleted", configurationTypeId, 1, CancellationToken.None);
			project = await connection.ProjectInsert("Project B", projectTypeId, 1, CancellationToken.None);
			projectId = project.Id;

			var response = await _fixture.Client.DeleteAsync($"/api/projects/{projectId}/configurations/{configurationId}");
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
		}
	}
}
