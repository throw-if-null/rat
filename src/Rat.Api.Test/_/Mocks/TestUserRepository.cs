using System.Threading;
using System.Threading.Tasks;
using Rat.Data.Entities;
using Rat.DataAccess.Users;

namespace Rat.Api.Test._.Mocks
{
    internal class TestUserRepository : IUserRepository
    {
        public Task<User> Retrieve(string externalId, CancellationToken cancellation)
        {
            return Task.FromResult(new User { Id = 1, ExternalId = "auth0|1431kj4n3ns2" });
        }
    }
}
