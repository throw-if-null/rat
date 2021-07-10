using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Rat.Core.Commands.Projects.PatchProject
{
    internal class PatchProjectCommand : IRequestHandler<PatchProjectRequest, PatchProjectResponse>
    {
        public Task<PatchProjectResponse> Handle(PatchProjectRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
