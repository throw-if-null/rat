using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rat.Api.Routes.PatchConfiguration.Data;
using Rat.Sql;
using Xunit;

namespace Rat.Api.Test.Routes.Configuration
{
	[Collection("Integration")]
	public class PatchConfigurationTests
	{
		private readonly RatFixture _fixture;

		public PatchConfigurationTests(RatFixture fixture)
		{
			_fixture = fixture;
		}

		[Fact]
		public async Task Should_Patch()
		{
			using var scope = _fixture.Provider.CreateScope();
			var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();

			await using var connection = connectionFactory.CreateConnection();
			var configurationTypeId = await connection.ConfigurationTypeGetByName("api");
			var jsTypeId = await connection.ProjectTypeGetByName("js");
			var project = await connection.ProjectInsert("Project A", jsTypeId, 1, CancellationToken.None);
			int projectId = project.Id;
			var configurationId = await connection.ConfigurationRootInsert(projectId, "Test", configurationTypeId, 1, CancellationToken.None);
			var csTypeId = await connection.ProjectTypeGetByName("csharp");

			var input = new PatchConfigurationRouteInput(configurationId, "Updated", csTypeId);

			var response = await _fixture.Client.PatchAsync(
				$"/api/projects/{projectId}/configurations/{configurationId}",
				new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json"));

			Assert.Equal(HttpStatusCode.OK, response.StatusCode);

			var contentStream = await response.Content.ReadAsStreamAsync();
			var output = await JsonSerializer.DeserializeAsync<PatchConfigurationRouteOutput>(contentStream, _fixture.JsonSerializerOption);

			Assert.Equal("Updated", output.Name);
			Assert.Equal(csTypeId, output.TypeId);
		}
	}
}
