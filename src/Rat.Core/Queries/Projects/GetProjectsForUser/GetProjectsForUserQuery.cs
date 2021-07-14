using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rat.Data;
using Rat.Data.Views;

namespace Rat.Core.Queries.Projects.GetProjectsForUser
{
    internal class GetProjectsForUserQuery : IRequestHandler<GetProjectsForUserRequest, GetProjectsForUserResponse>
    {
        private readonly RatDbContext _context;

        public GetProjectsForUserQuery(RatDbContext context)
        {
            _context = context;
        }

        public async Task<GetProjectsForUserResponse> Handle(GetProjectsForUserRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.UserId))
            {
                request.Context.ValidationErrors.Add(
                    $"{nameof(GetProjectsForUserRequest)}.{nameof(GetProjectsForUserRequest.UserId)}",
                    "UserId must be provided");

                request.Context.Status = ProcessingStatus.BadRequest;

                return new() { Context = request.Context };
            }

            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

            if (user == null)
            {
                var userEntity = await _context.Users.AddAsync(new () {UserId = request.UserId }, cancellationToken);
                user = userEntity.Entity;
            }

            var projects = await _context.Projects.Include(x => x.Type).Where(x => x.Users.Contains(user)).ToListAsync(cancellationToken);

            request.Context.Status = ProcessingStatus.Ok;

            return new()
            {
                Context = request.Context,
                UserProjectStats = new()
                {
                    UserId = user.Id,
                    ProjectStats = projects.Select(x => new ProjectStatsView
                    {
                        Id = x.Id,
                        Name = x.Name,
                        TotalConfigurationCount = 0,
                        TotalEntryCount = 0
                    })
                }
            };
        }
    }
}