using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rat.Api.Auth;
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
    public class ProjectsController : RatController
    {
        private readonly IMediator _mediator;
        private readonly IUserProvider _userProvider;

        public ProjectsController(IMediator mediator, IUserProvider userProvider)
        {
            _mediator = mediator;
            _userProvider = userProvider;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Post(CreateProjectModel model, CancellationToken cancellation)
        {
			var userId = _userProvider.GetUserId();

			if (string.IsNullOrWhiteSpace(userId))
				return Forbid();

			var response =
				await
					_mediator.Send(
						new CreateProjectRequest { Name = model.Name, ProjectTypeId = model.TypeId, UserId = userId },
						cancellation);

            if (response.Context.Status != ProcessingStatus.Ok)
                return HandleUnscusseful(response.Context);

            return CreatedAtRoute("GetById", new { id = response.Project.Id }, response.Project);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(CancellationToken cancellation)
        {
            var userId = _userProvider.GetUserId();

            if (string.IsNullOrWhiteSpace(userId))
                return Forbid();

            var response = await _mediator.Send(new GetProjectsForUserRequest { UserId = userId }, cancellation);

            if (response.Context.Status != ProcessingStatus.Ok)
                return HandleUnscusseful(response.Context);

            return Ok(response.UserProjectStats);
        }

        [HttpGet("{id:int}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id, CancellationToken cancellation)
        {
            var response = await _mediator.Send(new GetProjectByIdRequest { Id = id }, cancellation);

            if (response.Context.Status != ProcessingStatus.Ok)
                return HandleUnscusseful(response.Context);

            return Ok(response.Project);
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Patch(PatchProjectModel model, CancellationToken cancellation)
        {
            var response = await _mediator.Send(new PatchProjectRequest { Id = model.Id, Name = model.Name, ProjectTypeId = model.TypeId }, cancellation);

            if (response.Context.Status != ProcessingStatus.Ok)
                return HandleUnscusseful(response.Context);

            return Ok(response.Project);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellation)
        {
            var response = await _mediator.Send(new DeleteProjectRequest { Id = id }, cancellation);

            if (response.Context.Status != ProcessingStatus.Ok)
                return HandleUnscusseful(response.Context);

            return Ok();
        }
    }
}
