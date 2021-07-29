﻿using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Rat.Core.Properties;
using Rat.Data;
using Rat.Data.Exceptions;
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
            request.Validate();

            if (request.Context.Status != ProcessingStatus.GoodRequest)
                return new() { Context = request.Context };

            var userId = request.UserId;
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

            if (user == null)
            {
                var userEntity = await _context.Users.AddAsync(new () {UserId = request.UserId }, cancellationToken);

                var expectedNumberOfChanges = 1;
                var changes = await _context.SaveChangesAsync(cancellationToken);

                if (changes != expectedNumberOfChanges)
                    throw new RatDbException(string.Format(Resources.ExpactedAndActualNumberOfDatabaseChangesMismatch, changes, expectedNumberOfChanges));

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