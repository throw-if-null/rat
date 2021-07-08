using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rat.Api.Controllers.Projects.Models;

namespace Rat.Api.Controllers.Projects
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        [HttpGet()]
        [Authorize(Policy = "any")]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var projects = new[] {
                new ProjectOverviewModel
                {
                    Id = 1,
                    Name = "Rat",
                    Configurations = 3,
                    Entries = 21
                },
                new ProjectOverviewModel
                {
                    Id = 2,
                    Name = "Cat",
                    Configurations = 4,
                    Entries = 32
                }
            };

            await Task.CompletedTask;

            return Ok(projects);
        }
    }
}
