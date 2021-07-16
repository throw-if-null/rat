using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Rat.Data
{
    public static class SerrviceCollectionExtensions
    {
        public static IServiceCollection AddRatDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("RatDb");

            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Connection string for RatDb is not set");

            services.AddDbContext<RatDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}
