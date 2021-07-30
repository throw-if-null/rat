using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rat.Data;
using Rat.Data.Entities;
using Rat.Data.Views;
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
            using var context = scope.ServiceProvider.GetRequiredService<RatDbContext>();

            var projectType = await context.ProjectTypes.FirstOrDefaultAsync(x => x.Name == "csharp");
            var project = await context.Projects.AddAsync(new ProjectEntity { Name = "Should_Get_Project_By_Id", Type = projectType });
            await context.SaveChangesAsync();

            var projectId = project.Entity.Id.ToString();

            var response = await _fixture.Client.GetAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var contentStream = await response.Content.ReadAsStreamAsync();
            var content = await JsonSerializer.DeserializeAsync<ProjectView>(contentStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

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
            using var scope = _fixture.Provider.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<RatDbContext>();
            var projectType = await context.ProjectTypes.FirstOrDefaultAsync(x => x.Name == "csharp");
            var user = await context.Users.AddAsync(new UserEntity { UserId = "3feslrj3ssd111" });

			await context.Projects.AddAsync(new ProjectEntity { Name = "Project A", Type = projectType, Users = new List<ProjectUserEntity> { new() { User = user.Entity } } });
			await context.Projects.AddAsync(new ProjectEntity { Name = "Project B", Type = projectType, Users = new List<ProjectUserEntity> { new() { User = user.Entity } } });
            await context.SaveChangesAsync();

            var response = await _fixture.Client.GetAsync("/api/projects/");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            using (var content = await response.Content.ReadAsStreamAsync())
            {
                var projects =
                    await JsonSerializer.DeserializeAsync<UserProjectStatsView>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Snapshot.Match(
                    projects,
                    x =>
                    {
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
                UserProjectStatsView projects =
                    await JsonSerializer.DeserializeAsync<UserProjectStatsView>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Assert.True(projects.UserId > 0);
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
