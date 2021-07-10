using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Rat.Core.Commands.Projects.DeleteProject
{
    internal class DeleteProjectCommand : IRequestHandler<DeleteProjectRequest, DeleteProjectResponse>
    {
        public Task<DeleteProjectResponse> Handle(DeleteProjectRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
