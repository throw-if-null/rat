using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Rat.Core.Queries.Projects.GetProjectsForUser
{
    internal record GetProjectsForUserRequest : IRequest<GetProjectsForUserResponse>
    {
    }
}
