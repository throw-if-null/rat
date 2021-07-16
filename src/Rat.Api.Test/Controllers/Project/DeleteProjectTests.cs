using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rat.Data;
using Xunit;

namespace Rat.Api.Test.Controllers.Project
{
    [CollectionDefinition("Integration")]
    public class DeleteProjectTests : ProjectTestsBase
    {
        public DeleteProjectTests(CustomWebApplicationFactory factory) : base(factory)
        {
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-323)]
        public async Task Should_Return_BadRequest(int id)
        {
            var projectId = id.ToString();

            var response = await Client.DeleteAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }


        [Fact]
        public async Task Should_Return_NotFound()
        {
            using var context = new RatDbContext(Configuration.GetConnectionString("RatDb"));

            var projectType = await context.ProjectTypes.FirstOrDefaultAsync(x => x.Name == "js");
            var project = await context.Projects.AddAsync(new Data.Entities.Project { Name = "Test", Type = projectType });
            await context.SaveChangesAsync();

            var projectId = project.Entity.Id.ToString();

            context.Projects.Remove(project.Entity);
            await context.SaveChangesAsync();

            var response = await Client.DeleteAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Should_Delete()
        {
            using var context = new RatDbContext(Configuration.GetConnectionString("RatDb"));

            var projectType = await context.ProjectTypes.FirstOrDefaultAsync(x => x.Name == "js");
            var project = await context.Projects.AddAsync(new Data.Entities.Project { Name = "Test", Type = projectType });
            await context.SaveChangesAsync();

            var projectId = project.Entity.Id.ToString();

            var response = await Client.DeleteAsync($"/api/projects/{projectId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
