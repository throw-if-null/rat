using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Rat.Api.Routes.Data;
using Rat.Data;
using Rat.Data.Entities;
using Xunit;

namespace Rat.Api.Test.Controllers.Project
{
	[Collection("Integration")]
    public class PatchProjectTests
    {
        private readonly RatFixture _fixture;

        public PatchProjectTests(RatFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Should_Patch()
        {
            using var scope = _fixture.Provider.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<RatDbContext>();

            var projectTypes = await context.ProjectTypes.ToListAsync();
            var jsType = projectTypes.First(x => x.Name == "js");
            var csharpType = projectTypes.First(x => x.Name == "csharp");

            var project = await context.Projects.AddAsync(new ProjectEntity { Name = "Patch", Type = jsType });
            await context.SaveChangesAsync();

            var model = new PatchProjectRouteInput(project.Entity.Id, "New test", csharpType.Id);

            var response = await _fixture.Client.PatchAsync(
                $"/api/projects/{model.Id}",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            var contentStream = await response.Content.ReadAsStreamAsync();
            var content = await JsonSerializer.DeserializeAsync<PatchProjectRouteOutput>(contentStream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("New test", content.Name);
            Assert.Equal(csharpType.Id, content.TypeId);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("sqoyhhpamcsnpwzjdwwneydgighyecnwpykbtbugmqclefuhndqpvnfhupwaofgnwlehtwfujyrlavgubnuvqrjdbbanpwvnaneembgplatqvselnwvfefezxznyvnqkdqaalqwyjmlskovuowehyaujnhevlpcgtxhfwwbiwsuozfmeishfnovyteddvyxfmclwiekfqjmelujrevprrsctksqkvnzwqwksibojrnhmcftdjnogsrmane")]
        public async Task Should_Return_BadRequest_When_Name_Value_Is_Invalid(string name)
        {
            using var scope = _fixture.Provider.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<RatDbContext>();
            var projectType = await context.ProjectTypes.FirstAsync(x => x.Name == "js");

            var project = await context.Projects.AddAsync(new ProjectEntity { Name = "Patch", Type = projectType });
            await context.SaveChangesAsync();

			var model = new PatchProjectRouteInput(project.Entity.Id, name, projectType.Id);

			var response = await _fixture.Client.PatchAsync(
                $"/api/projects/{model.Id}",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Should_Return_NotFound()
        {
            using var scope = _fixture.Provider.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<RatDbContext>();
            var projectType = await context.ProjectTypes.FirstAsync(x => x.Name == "js");

            var project = await context.Projects.AddAsync(new ProjectEntity { Name = "Patch", Type = projectType });
            await context.SaveChangesAsync();

            context.Projects.Remove(project.Entity);
            await context.SaveChangesAsync();

			var model = new PatchProjectRouteInput(project.Entity.Id, "Rat", projectType.Id);

            var response = await _fixture.Client.PatchAsync(
                $"/api/projects/{model.Id}",
                new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
