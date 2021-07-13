using System.Threading;
using System.Threading.Tasks;
using Rat.Data.Entities;
using Rat.Data.Views;

namespace Rat.DataAccess.Projects
{
    public interface IProjectRepository
    {
        Task<Project> Create(Project project, CancellationToken cancellation);

        Task<Project> Retrieve(int id, CancellationToken cancellation);

        Task<UserProjectStats> RetrieveUserProjectStats(int userId, CancellationToken cancellation);

        Task<Project> Update(Project project, CancellationToken cancellation);

        Task Delete(int id, CancellationToken cancellation);
    }
}
