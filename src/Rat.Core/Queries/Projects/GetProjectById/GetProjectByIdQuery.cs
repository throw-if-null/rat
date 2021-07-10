﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Rat.Core.Queries.Projects.GetProjectById
{
    internal class GetProjectByIdQuery : IRequestHandler<GetProjectByIdRequest, GetProjectByIdResponse>
    {
        public Task<GetProjectByIdResponse> Handle(GetProjectByIdRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
