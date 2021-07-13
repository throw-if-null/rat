using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Rat.Data.Entities;
using Rat.Data.Views;
using Rat.DataAccess.Projects;

namespace Rat.Api.Test._.Mocks
{
    internal class TestProjectRepository : IProjectRepository
    {
        public async Task<Project> Create(Project project, CancellationToken cancellation)
        {
            await Task.CompletedTask;

            return project with { Id = 1 };
        }

        public async Task Delete(int id, CancellationToken cancellation)
        {
            await Task.CompletedTask;
        }

        public async Task<Project> Retrieve(int id, CancellationToken cancellation)
        {
            await Task.CompletedTask;

            if (id == 100)
                return null;

            return new () { Id = id, Name = "Test" };
        }

        public async Task<UserProjectStats> RetrieveUserProjectStats(int userId, CancellationToken cancellation)
        {
            await Task.CompletedTask;

            return new()
            {
                UserId = userId,
                ProjectStats = new List<ProjectStats>
                {
                    new () {Id = 1, Name = "Test1", TotalConfigurationCount = 12, TotalEntryCount = 45},
                    new () {Id = 1, Name = "Test2", TotalConfigurationCount = 3, TotalEntryCount = 22}
                }
            };
        }

        public async Task<Project> Update(Project project, CancellationToken cancellation)
        {
            await Task.CompletedTask;

            return project with { Name = project.Name == null ? "Test" : project.Name };
        }
    }
}
