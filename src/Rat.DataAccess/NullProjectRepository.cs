using System.Threading;
using System.Threading.Tasks;
using Rat.Data.Entities;
using Rat.Data.Views;

namespace Rat.DataAccess
{
    public sealed class NullProjectRepository : IProjectRepository
    {
        public Task<Project> Create(Project project, CancellationToken cancellation) => Task.FromResult(new Project());

        public Task Delete(int id, CancellationToken cancellation) => Task.CompletedTask;

        public Task<Project> Retrieve(int id, CancellationToken cancellation) => Task.FromResult(new Project());

        public Task<UserProjectStats> RetrieveUserProjectStats(int userId, CancellationToken cancellation)
            => Task.FromResult(new UserProjectStats());

        public Task<Project> Update(Project project, CancellationToken cancellation) => Task.FromResult(new Project());
    }
}
