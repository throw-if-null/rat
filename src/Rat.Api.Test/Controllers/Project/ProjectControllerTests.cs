using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Rat.Api.Controllers.Projects.Models;
using Snapshooter.Xunit;
using Xunit;

namespace Rat.Api.Test.Controllers.Project
{
    public class ProjectControllerTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ProjectControllerTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Fact]
        public async Task Should_Return_Projects_For_User()
        {
            var response = await _client.GetAsync("/api/projects");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            using (var content = await response.Content.ReadAsStreamAsync())
            {
                var projects = await JsonSerializer.DeserializeAsync<IEnumerable<ProjectOverviewModel>>(content);

                Snapshot.Match(
                    projects);
            }
        }
    }
}
