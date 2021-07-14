using System.Threading;
using System.Threading.Tasks;
using Rat.Data.Entities;

namespace Rat.DataAccess.Users
{
    public sealed class NullUserRepository : IUserRepository
    {
        public Task<User> Create(string externalId, CancellationToken cancellation) => Task.FromResult(new User());

        public Task<User> Retrieve(string externalId, CancellationToken cancellation) => Task.FromResult(new User());
    }
}
