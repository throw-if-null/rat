using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rat.Api.Routes.CreateConfiguration.Data;
using Rat.Sql;
using Snapshooter.Xunit;
using Xunit;

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
		public async Task Should_Create_Project()
		{
			using var scope = _fixture.Provider.CreateScope();
			var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();

			await using var connection = connectionFactory.CreateConnection();
			var configurationTypeId = await connection.ConfigurationTypeGetByName("api");
			var projectTypeId = await connection.ProjectTypeGetByName("js");
			var project = await connection.ProjectInsert("Project A", projectTypeId, 1, CancellationToken.None);
			int projectId = project.Id;

			var model = new CreateConfigurationRouteInput("Rat Api", configurationTypeId);

			var response = await _fixture.Client.PostAsync(
				$"/api/projects/{projectId}/configurations",
				new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

			Assert.Equal(HttpStatusCode.Created, response.StatusCode);

			var contentStream = await response.Content.ReadAsStreamAsync();
			var output = await JsonSerializer.DeserializeAsync<CreateConfigurationRouteOutput>(contentStream, _fixture.JsonSerializerOption);

			Assert.True(output.Id > 0);
		}

		[Fact]
		public async Task Should_Return_Forbidden()
		{
			var model = new CreateConfigurationRouteInput("Rat Api", 0);

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("api/projects/0/configurations", UriKind.Relative),
				Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
			};

			request.Headers.Add("test-user", "null");

			var response = await _fixture.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

			Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
		}

		[Theory]
		[InlineData(null, 1, 1, 1, "1")]
		[InlineData("", 1, 1, 1, "2")]
		[InlineData("test", 0, 1, 1, "3")]
		[InlineData(null, -4, 1, 1, "4")]
		[InlineData(null, 1, 0, 1, "5")]
		[InlineData(null, 1, -1, 1, "6")]
		public async Task Should_Return_BadRequest(string name, int projectId, int configurationTypeId, int createdBy, string version)
		{
			if (projectId > 0)
			{
				using var scope = _fixture.Provider.CreateScope();
				var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();

				await using var connection = connectionFactory.CreateConnection();
				var projectTypeId = await connection.ProjectTypeGetByName("js");
				var project = await connection.ProjectInsert("Project A", projectTypeId, 1, CancellationToken.None);
				projectId = project.Id;
			}

			var model = new CreateConfigurationRouteInput(name, configurationTypeId);

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri($"api/projects/{projectId}/configurations", UriKind.Relative),
				Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
			};

			request.Headers.Add("test-user", createdBy.ToString());

			var response = await _fixture.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

			var content = await response.Content.ReadAsStringAsync();
			Snapshot.Match(content, $"{nameof(CreateConfigurationTests)}.{nameof(Should_Return_BadRequest)}.{version}");
		}
	}
}
