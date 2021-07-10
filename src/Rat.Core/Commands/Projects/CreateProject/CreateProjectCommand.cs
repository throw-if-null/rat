using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Rat.Core.Commands.Projects.CreateProject
{
    internal class CreateProjectCommand : IRequestHandler<CreateProjectRequest, CreateProjectResponse>
    {
        public Task<CreateProjectResponse> Handle(CreateProjectRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
