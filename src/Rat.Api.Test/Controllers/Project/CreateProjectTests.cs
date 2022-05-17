using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Rat.Api.Routes.Data;
using Rat.Api.Test.Mocks;
using Rat.DataAccess;
using Rat.Sql;
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
            var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
			var command = new CommandDefinition("SELECT Id FROM ProjectType WHERE Name = @Name", new { Name = "js" });

			await using var connection = connectionFactory.CreateConnection();

			var projectTypeId = await connection.QuerySingleAsync<int>(command);

            var model = new CreateProjectRouteInput("Rat Api", projectTypeId);

            var response = await _fixture.Client.PostAsync(
                "/api/projects",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var contentStream = await response.Content.ReadAsStreamAsync();
            var content = await JsonSerializer.DeserializeAsync<CreateProjectRouteOutput>(contentStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.True(content.Id > 0);
        }

		[Fact]
		public async Task Should_Return_Forbidden()
		{
			using var scope = _fixture.Provider.CreateScope();
			var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
			var command = new CommandDefinition("SELECT Id FROM ProjectType WHERE Name = @Name", new { Name = "js" });

			await using var connection = connectionFactory.CreateConnection();

			var projectTypeId = await connection.QuerySingleAsync<int>(command);

			var model = new CreateProjectRouteInput("Rat Api", projectTypeId);

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("/api/projects", UriKind.Relative),
				Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
			};

			request.Headers.Add("test-user", "null");

			var response = await _fixture.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

			Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
		}

        [Theory]
        [InlineData("", "1", TestUserProvider.UserId)]
        [InlineData(null, "2", TestUserProvider.UserId)]
		[InlineData("Test", "1", "unknown-user")]
		public async Task Should_Return_BadRequest(string name, string version, string userId)
        {
            var model = new CreateProjectRouteInput(name, 0);

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("/api/projects", UriKind.Relative),
				Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
			};

			request.Headers.Add("test-user", userId);

			var response = await _fixture.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Snapshot.Match(response.ReasonPhrase, $"{nameof(CreateProjectTests)}.{nameof(Should_Return_BadRequest)}.{version}");
        }
    }
}
