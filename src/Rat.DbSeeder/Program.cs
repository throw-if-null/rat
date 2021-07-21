using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rat.Data;

namespace Rat.DbSeeder
{
    internal static class Program
    {
        internal static Task Main(string[] args)
        {
            IConfiguration configuration =
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

            var host = new HostBuilder()
                .ConfigureServices(x =>
                {
                    x.AddLogging(x => x.AddConsole());

                    x.AddRatDbContext(configuration);

                    x.AddHostedService<DatabaseCreatorHost>();
                }).Build();

            return host.RunAsync();
        }
    }
}
