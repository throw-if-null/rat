using System.Threading;
using System.Threading.Tasks;
using Rat.Data.Entities;
using Rat.DataAccess.Users;

namespace Rat.Api.Test._.Mocks
{
    internal class TestUserRepository : IUserRepository
    {
        public Task<User> Create(string externalId, CancellationToken cancellation)
        {
            return Task.FromResult(new User { Id = 1, ExternalId = externalId });
        }

        public Task<User> Retrieve(string externalId, CancellationToken cancellation)
        {
            if (externalId.Equals("no-user"))
                return null;

            return Task.FromResult(new User { Id = 1, ExternalId = externalId });
        }
    }
}
