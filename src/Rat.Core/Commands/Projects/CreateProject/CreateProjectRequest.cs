using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace Rat.Core.Commands.Projects.CreateProject
{
    internal record CreateProjectRequest : IRequest<CreateProjectResponse>
    {
    }
}
