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

			var input = new CreateProjectRouteInput("Rat Api", projectTypeId);

            var response = await _fixture.Client.PostAsync(
                "/api/projects",
                new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var contentStream = await response.Content.ReadAsStreamAsync();
            var output = await JsonSerializer.DeserializeAsync<CreateProjectRouteOutput>(contentStream, _fixture.JsonSerializerOption);

            Assert.True(output.Id > 0);
        }

		[Fact]
		public async Task Should_Return_Forbidden()
		{
			var input = new CreateProjectRouteInput("Rat Api", 0);

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("/api/projects", UriKind.Relative),
				Content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json")
			};

			request.Headers.Add("test-user", "null");

			var response = await _fixture.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

			Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
		}

        [Theory]
        [InlineData("", 1, TestMemberProvider.MemberId, "1")]
        [InlineData(null, 1, TestMemberProvider.MemberId, "2")]
		[InlineData("Test", 0, TestMemberProvider.MemberId, "3")]
		[InlineData("Test", -1, TestMemberProvider.MemberId, "4")]
		public async Task Should_Return_BadRequest(string name, int projectTypeId, int memberId, string version)
        {
            var input = new CreateProjectRouteInput(name, projectTypeId);

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri("/api/projects", UriKind.Relative),
				Content = new StringContent(JsonSerializer.Serialize(input), Encoding.UTF8, "application/json")
			};

			request.Headers.Add("test-user", memberId.ToString());

			var response = await _fixture.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

			var content = await response.Content.ReadAsStringAsync();
            Snapshot.Match(content, $"{nameof(CreateProjectTests)}.{nameof(Should_Return_BadRequest)}.{version}");
        }
    }
}
