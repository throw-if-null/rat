using Microsoft.AspNetCore.Http;
using Rat.Api.Auth;

namespace Rat.Api.Test.Mocks
{
    public class TestUserProvider : IUserProvider
    {
		public const string UserId = "3feslrj3ssd111";

		private readonly IHttpContextAccessor _contextAccessor;

        public TestUserProvider(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetUserId()
        {
            if (_contextAccessor.HttpContext.Request.Headers.ContainsKey("test-user"))
            {
                _contextAccessor.HttpContext.Request.Headers.TryGetValue("test-user", out var values);
                var value = values[0];

                if (value.Equals("null"))
                    return null;

                return value;
            }

            return UserId;
        }
    }
}
