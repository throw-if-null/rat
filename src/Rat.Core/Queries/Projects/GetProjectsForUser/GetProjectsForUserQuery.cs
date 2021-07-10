using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Rat.Core.Queries.Projects.GetProjectsForUser
{
    internal class GetProjectsForUserQuery : IRequestHandler<GetProjectsForUserRequest, GetProjectsForUserResponse>
    {
        public Task<GetProjectsForUserResponse> Handle(GetProjectsForUserRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
