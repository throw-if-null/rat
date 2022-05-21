using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Rat.Api.Auth;

namespace Rat.Api.Test.Mocks
{
	public class TestMemberProvider : IMemberProvider
    {
		public const int MemberId = 42;

		private readonly IHttpContextAccessor _contextAccessor;

        public TestMemberProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public async Task<int> GetMemberId(CancellationToken ct)
        {
			await Task.CompletedTask;

            if (_contextAccessor.HttpContext.Request.Headers.ContainsKey("test-user"))
            {
                _contextAccessor.HttpContext.Request.Headers.TryGetValue("test-user", out var values);
                var value = values[0];

                if (value.Equals("null"))
                    return default;

                return int.Parse(value);
            }

            return MemberId;
        }
    }
}
