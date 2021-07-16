using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Rat.Core;

namespace Rat.Api.Controllers
{
    public abstract class RatController : ControllerBase
    {
        protected IActionResult HandleUnscusseful(RatContext context)
        {
            switch (context.Status)
            {
                case ProcessingStatus.BadRequest:
                    return Invalid(context.ValidationErrors);

                case ProcessingStatus.NotFound:
                    return NotFound();

                default:
                    return Problem();
            }
        }

        private BadRequestObjectResult Invalid(IDictionary<string, string> validationErrors)
        {
            foreach (var (key, value) in validationErrors)
            {
                ModelState.AddModelError(key, value);
            }

            return BadRequest(ModelState);
        }
    }
}
