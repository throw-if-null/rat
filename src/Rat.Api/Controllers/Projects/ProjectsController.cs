using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rat.Api.Controllers.Projects.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace Rat.Api.Controllers.Projects
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Post(CreateProjectModel model, CancellationToken cancellation)
        {
            await Task.CompletedTask;

            model ??= new ();

            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Type))
                return BadRequest();

            var project = new ProjectModel
            {
                Id = 1,
                Created = DateTimeOffset.UtcNow,
                LastModified = DateTimeOffset.MinValue,
                CreatedBy = 1,
                ModifiedBy = 1,
                Name = model.Name,
                Type = model.Type
            };

            return CreatedAtRoute("GetById", new { id = project.Id }, project);
        }

        [HttpGet]
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

        [HttpGet("{id:int}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Get(int id, CancellationToken cancellation)
        {
            await Task.CompletedTask;

            if (id <= 0)
                return NotFound();

            return Ok(new ProjectModel
            {
                Id = 1,
                Name = "Rat",
            Type = "js",
            Created = DateTimeOffset.UtcNow.AddDays(-4),
            CreatedBy = 1,
            LastModified = DateTimeOffset.UtcNow.AddDays(-3),
            ModifiedBy = 1
            });
        }

        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Patch(PatchProjectModel model)
        {
            await Task.CompletedTask;

            model ??= new ();

            if (model.Name != null && model.Name.Length == 0)
                return BadRequest();

            if (model.Type != null && model.Type.Length == 0)
                return BadRequest();

            if (model.Name == null && model.Type == null)
                return NoContent();

            return Ok(new ProjectModel
            {
                Id = model.Id,
                Name = model.Name,
                Type = model.Type,
                Created = DateTimeOffset.UtcNow.AddDays(-5),
                LastModified = DateTimeOffset.UtcNow.AddDays(2),
                CreatedBy = 1,
                ModifiedBy = 2
            });
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellation)
        {
            await Task.CompletedTask;

            return id <= 0 ? NotFound() : Ok();
        }
    }
}
