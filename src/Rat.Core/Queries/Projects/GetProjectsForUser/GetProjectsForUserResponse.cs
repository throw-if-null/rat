using System.Collections.Generic;

namespace Rat.Core.Queries.Projects.GetProjectsForUser
{
    internal record GetProjectsForUserResponse
    {
        public RatContext Context { get; init; }

        public int UserId { get; init; }

        public IEnumerable<GetProjectsForUserResponse> Projects { get; init; }
    }
}
