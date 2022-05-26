using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rat.Api.Routes.Data;
using Rat.Sql;
using Xunit;
using Rat.Api.Routes.CreateConfiguration.Data;
using System.Text.Json;
using System.Threading;

namespace Rat.Api.Test.Controllers.Routes
{
	[Collection("Integration")]
	public class CreateConfigurationTests
	{
		private readonly RatFixture _fixture;

		public CreateConfigurationTests(RatFixture fixture)
		{
			_fixture = fixture;
		}

		[Fact]
		public async Task ShouldCreateProject()
		{
			using var scope = _fixture.Provider.CreateScope();
			var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();

			await using var connection = connectionFactory.CreateConnection();
			var typeId = await connection.ConfigurationTypeGetByName("api");
			var projectTypeId = await connection.ProjectTypeGetByName("js");
			var project = await connection.ProjectInsert("Project A", projectTypeId, 1, CancellationToken.None);
			int projectId = project.Id;

			var model = new CreateConfigurationRouteInput("Rat Api", typeId);

			var response = await _fixture.Client.PostAsync(
				$"/api/projects/{projectId}/configurations",
				new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

			Assert.Equal(HttpStatusCode.Created, response.StatusCode);

			var contentStream = await response.Content.ReadAsStreamAsync();
			var output = await JsonSerializer.DeserializeAsync<CreateConfigurationRouteOutput>(contentStream, _fixture.JsonSerializerOption);

			Assert.True(output.Id > 0);
		}
	}
}
