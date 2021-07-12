using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rat.Api.Controllers.Projects.Models;
using Rat.Core;
using Rat.Core.Commands.Projects.CreateProject;
using Rat.Core.Commands.Projects.DeleteProject;
using Rat.Core.Commands.Projects.PatchProject;
using Rat.Core.Queries.Projects.GetProjectById;
using Rat.Core.Queries.Projects.GetProjectsForUser;

namespace Rat.Api.Controllers.Projects
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Post(CreateProjectModel model, CancellationToken cancellation)
        {
            var response = await _mediator.Send(new CreateProjectRequest { Name = model.Name }, cancellation);

            switch (response.Context.Status)
            {
                case ProcessingStatus.Ok:
                    return CreatedAtRoute("GetById", new { id = response.Project.Id }, response.Project);

                case ProcessingStatus.BadRequest:
                    response.Context.ValidationErrors.ToList().ForEach(error => ModelState.AddModelError(error.Key, error.Value));
                    return BadRequest(ModelState);

                case ProcessingStatus.Error:
                    return Problem(response.Context.FailureReason, null, 500, "Unexpected exception",null);

                default:
                    throw new ApplicationException("ProcessingStatus == None!");
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            // extract UserId from HttpContext
            var response = await _mediator.Send(new GetProjectsForUserRequest { UserId = 1 }, cancellation);

            switch (response.Context.Status)
            {
                case ProcessingStatus.Ok:
                    return Ok(response.UserProjectStats);

                case ProcessingStatus.NotFound:
                    return NotFound();

                case ProcessingStatus.Error:
                    return Problem(response.Context.FailureReason, null, 500, "Unexpected exception", null);

                default:
                    throw new ApplicationException("ProcessingStatus == None!");
            }
        }

        [HttpGet("{id:int}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<IActionResult> Get(int id, CancellationToken cancellation)
        {
            var response = await _mediator.Send(new GetProjectByIdRequest { Id = id }, cancellation);

            switch (response.Context.Status)
            {
                case ProcessingStatus.Ok:
                    return Ok(response.Project);

                case ProcessingStatus.BadRequest:
                    response.Context.ValidationErrors.ToList().ForEach(error => ModelState.AddModelError(error.Key, error.Value));
                    return BadRequest(ModelState);

                case ProcessingStatus.NotFound:
                    return NotFound();

                case ProcessingStatus.Error:
                    return Problem(response.Context.FailureReason, null, 500, "Unexpected exception", null);

                default:
                    throw new ApplicationException("ProcessingStatus == None!");
            }
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Patch(PatchProjectModel model, CancellationToken cancellation)
        {
            var response = await _mediator.Send(new PatchProjectRequest { Id = model.Id, Name = model.Name }, cancellation);

            switch (response.Context.Status)
            {
                case ProcessingStatus.Ok:
                    return Ok(response.Project);

                case ProcessingStatus.BadRequest:
                    response.Context.ValidationErrors.ToList().ForEach(error => ModelState.AddModelError(error.Key, error.Value));
                    return BadRequest(ModelState);

                case ProcessingStatus.NotFound:
                    return NotFound();

                case ProcessingStatus.Error:
                    return Problem(response.Context.FailureReason, null, 500, "Unexpected exception", null);

                default:
                    throw new ApplicationException("ProcessingStatus == None!");
            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellation)
        {
            var response = await _mediator.Send(new DeleteProjectRequest { Id = id }, cancellation);

            switch (response.Context.Status)
            {
                case ProcessingStatus.Ok:
                    return Ok();

                case ProcessingStatus.BadRequest:
                    response.Context.ValidationErrors.ToList().ForEach(error => ModelState.AddModelError(error.Key, error.Value));
                    return BadRequest(ModelState);

                case ProcessingStatus.NotFound:
                    return NotFound();

                case ProcessingStatus.Error:
                    return Problem(response.Context.FailureReason, null, 500, "Unexpected exception", null);

                default:
                    throw new ApplicationException("ProcessingStatus == None!");
            }
        }
    }
}
