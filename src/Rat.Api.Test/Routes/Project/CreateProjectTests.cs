using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Rat.Api.Routes.Data;
using Rat.Api.Test.Mocks;
using Rat.Sql;
using Snapshooter.Xunit;
using Xunit;

namespace Rat.Api.Test.Routes.Project
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

			await using var connection = connectionFactory.CreateConnection();
			var projectTypeId = await connection.ProjectTypeGetByName("js");

			var model = new CreateProjectRouteInput("Rat Api", projectTypeId);

            var response = await _fixture.Client.PostAsync(
                "/api/projects",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var contentStream = await response.Content.ReadAsStreamAsync();
            var output = await JsonSerializer.DeserializeAsync<CreateProjectRouteOutput>(contentStream, _fixture.JsonSerializerOption);

            Assert.True(output.Id > 0);
        }

		[Fact]
		public async Task Should_Return_Forbidden()
		{
			using var scope = _fixture.Provider.CreateScope();
			var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
			await using var connection = connectionFactory.CreateConnection();
			var projectTypeId = await connection.ProjectTypeGetByName("js");

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
        [InlineData("", "1", TestMemberProvider.MemberId)]
        [InlineData(null, "2", TestMemberProvider.MemberId)]
		[InlineData("Test", "1", 31)]
		public async Task Should_Return_BadRequest(string name, string version, int memberId)
        {
            var model = new CreateProjectRouteInput(name, 0);

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("/api/projects", UriKind.Relative),
				Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
			};

			request.Headers.Add("test-user", memberId.ToString());

			var response = await _fixture.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            Snapshot.Match(response.ReasonPhrase, $"{nameof(CreateProjectTests)}.{nameof(Should_Return_BadRequest)}.{version}");
        }
    }
}
