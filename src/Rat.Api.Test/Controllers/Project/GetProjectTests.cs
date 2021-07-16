using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rat.Data;
using Rat.Data.Views;
using Snapshooter.Xunit;
using Xunit;

namespace Rat.Api.Test.Controllers.Project
{
    [CollectionDefinition("Integration")]
    public class GetProjectTests : ProjectTestsBase
    {
        public GetProjectTests(CustomWebApplicationFactory factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_Get_Project_By_Id()
        {
            using var context = new RatDbContext(Configuration.GetConnectionString("RatDb"));
            var projectType = await context.ProjectTypes.FirstOrDefaultAsync(x => x.Name == "csharp");
            var project = await context.Projects.AddAsync(new Data.Entities.Project { Name = "Should_Get_Project_By_Id", Type = projectType });
            await context.SaveChangesAsync();

            var projectId = project.Entity.Id.ToString();

            var response = await Client.GetAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var contentStream = await response.Content.ReadAsStreamAsync();
            var content = await JsonSerializer.DeserializeAsync<ProjectView>(contentStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Snapshot.Match(content);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-4)]
        public async Task Should_Return_BadRequest(int id)
        {
            var projectId = id.ToString();

            var response = await Client.GetAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_Return_Projects_For_User()
        {
            using var context = new RatDbContext(Configuration.GetConnectionString("RatDb"));
            var projectType = await context.ProjectTypes.FirstOrDefaultAsync(x => x.Name == "csharp");
            var user = await context.Users.AddAsync(new Data.Entities.User { UserId = "3feslrj3ssd111" });

            var projectA = await context.Projects.AddAsync(new Data.Entities.Project { Name = "Project A", Type = projectType, Users = new List<Data.Entities.User> { user.Entity } });
            var projectB = await context.Projects.AddAsync(new Data.Entities.Project { Name = "Project B", Type = projectType, Users = new List<Data.Entities.User> { user.Entity } });
            await context.SaveChangesAsync();

            var response = await Client.GetAsync("/api/projects/");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            using (var content = await response.Content.ReadAsStreamAsync())
            {
                UserProjectStatsView projects =
                    await JsonSerializer.DeserializeAsync<UserProjectStatsView>(
                        content,
                        new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                Snapshot.Match(projects);
            }
        }

        [Fact]
        public async Task Should_Return_No_Projects_When_User_Is_Not_In_Rat_Database()
        {
            Client.DefaultRequestHeaders.Add("test-user", "test-user");

            var response = await Client.GetAsync("/api/projects/");
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
            Client.DefaultRequestHeaders.Add("test-user", "null");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("/api/projects", UriKind.Relative)
            };

            var response = await Client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
