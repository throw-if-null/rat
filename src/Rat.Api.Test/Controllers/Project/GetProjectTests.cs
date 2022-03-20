using System;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rat.Api.Routes.Data;
using Rat.DataAccess;
using Snapshooter.Xunit;
using Xunit;

namespace Rat.Api.Test.Controllers.Project
{
	[Collection("Integration")]
    public class GetProjectTests
    {
        private readonly RatFixture _fixture;

        public GetProjectTests(RatFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Get_Project_By_Id()
        {
            using var scope = _fixture.Provider.CreateScope();
			var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
			await using var connection = connectionFactory.CreateConnection();

			var command = new CommandDefinition("SELECT Id FROM ProjectType WHERE Name == @Name", new { Name = "csharp" });
			var projectTypeId = await connection.QuerySingleAsync<int>(command);

			command = new CommandDefinition(
				"",
				new { Name = "Should_Get_Project_By_Id", Type = projectTypeId });

			var projectId = await connection.QuerySingleAsync<int>(command);

            var response = await _fixture.Client.GetAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var contentStream = await response.Content.ReadAsStreamAsync();
            var content = await JsonSerializer.DeserializeAsync<GetProjectRouteOutput>(contentStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Snapshot.Match(
                content,
                x =>
                {
                    x.IgnoreField<int>("Id");
                    x.IgnoreField<int>("TypeId");

                    return x;
                });
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-4)]
        public async Task Should_Return_BadRequest(int id)
        {
            var projectId = id.ToString();

            var response = await _fixture.Client.GetAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_Return_Projects_For_User()
        {
			var authProviderUserId = "3er4werlj21x";

			using var scope = _fixture.Provider.CreateScope();
			var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
			await using var connection = connectionFactory.CreateConnection();

			var command = new CommandDefinition("SELECT Id FROM ProjectType WHERE Name == @Name", new { Name = "csharp" });
			var projectTypeId = await connection.QuerySingleAsync<int>(command);

			command = new CommandDefinition(
				"",
				new { UserId = authProviderUserId });

			var userId = await connection.QuerySingleAsync<int>(command);

			command = new CommandDefinition(
				"INSERT INTO Project (Name, ProjectTypeId) VALUES(@Name, @ProjectTypeId); SELECT @@SCOPE_INDENTITY()",
				new { Name = "Project A", ProjectTypeId = projectTypeId });

			var projectIdA = await connection.QuerySingleAsync<int>(command);

			command = new CommandDefinition(
				"INSERT INTO Project (Name, ProjectTypeId) VALUES(@Name, @ProjectTypeId); SELECT @@SCOPE_INDENTITY()",
				new { Name = "Project B", ProjectTypeId = projectTypeId });

			var projectIdB = await connection.QuerySingleAsync<int>(command);

			command = new CommandDefinition(
				"INSERT INTO UserProject (UserId, ProjectID) VALUES(@UserId, @ProjectId",
				new { UserId = userId, ProjectId = projectIdA });

			await connection.ExecuteAsync(command);

			command = new CommandDefinition(
				"INSERT INTO UserProject (UserId, ProjectID) VALUES(@UserId, @ProjectId",
				new { UserId = userId, ProjectId = projectIdB });

			await connection.ExecuteAsync(command);

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri("/api/projects/", uriKind: UriKind.Relative)
			};

			request.Headers.Add("test-user", authProviderUserId);

			var response = await _fixture.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            using (var content = await response.Content.ReadAsStreamAsync())
            {
                var projects =
                    await JsonSerializer.DeserializeAsync<GetProjectForUserRouteOutput>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Snapshot.Match(
                    projects,
                    x =>
                    {
						x.IgnoreFields<int>("UserId");
						x.IgnoreFields<int>("ProjectStats[*].Id");

						return x;
                    });
            }
        }

        [Fact]
        public async Task Should_Return_No_Projects_When_User_Is_Not_In_Rat_Database()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("/api/projects", UriKind.Relative)
            };

            request.Headers.Add("test-user", "test-user");

            var response = await _fixture.Client.SendAsync(request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            using (var content = await response.Content.ReadAsStreamAsync())
            {
				var projects =
                    await JsonSerializer.DeserializeAsync<GetProjectForUserRouteOutput>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Assert.NotNull(projects.UserId);
                Assert.Empty(projects.ProjectStats);
            }
        }

        [Fact]
        public async Task Should_Return_Forbidden_When_NameClaim_Is_Missing()
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("/api/projects", UriKind.Relative)
            };

            request.Headers.Add("test-user", "null");

            var response = await _fixture.Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
