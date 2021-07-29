using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rat.Data;
using Rat.Data.Entities;
using Xunit;

namespace Rat.Api.Test.Controllers.Project
{
    [Collection("Integration")]
    public class DeleteProjectTests
    {
        private readonly RatFixture _fixture;

        public DeleteProjectTests(RatFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-323)]
        public async Task Should_Return_BadRequest(int id)
        {
            var projectId = id.ToString();

            var response = await _fixture.Client.DeleteAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public async Task Should_Return_NotFound()
        {
            using var scope = _fixture.Provider.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<RatDbContext>();
            var projectType = await context.ProjectTypes.FirstOrDefaultAsync(x => x.Name == "js");
            var project = await context.Projects.AddAsync(new ProjectEntity { Name = "Test", Type = projectType });
            await context.SaveChangesAsync();

            var projectId = project.Entity.Id.ToString();

            context.Projects.Remove(project.Entity);
            await context.SaveChangesAsync();

            var response = await _fixture.Client.DeleteAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_Delete()
        {
            using var scope = _fixture.Provider.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<RatDbContext>();
            var projectType = await context.ProjectTypes.FirstOrDefaultAsync(x => x.Name == "js");
            var project = await context.Projects.AddAsync(new ProjectEntity { Name = "Test", Type = projectType });
            await context.SaveChangesAsync();

            var projectId = project.Entity.Id.ToString();

            var response = await _fixture.Client.DeleteAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
