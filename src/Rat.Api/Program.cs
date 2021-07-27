using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rat.Data;

namespace Rat.Api
{
    [ExcludeFromCodeCoverage]
    internal class Program
    {
        internal static async Task Main(string[] args)
        {
            var cancellationSource = new CancellationTokenSource();

            Console.CancelKeyPress += (s, e) =>
            {
                cancellationSource.Cancel();
                e.Cancel = true;
            };

            var host = CreateHostBuilder(args).Build();

            var environment = host.Services.GetRequiredService<IWebHostEnvironment>();

            if (environment.IsDevelopment())
                await MigrateDatabase(host.Services, cancellationSource.Token);

            await host.RunAsync(cancellationSource.Token);
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        private static async Task MigrateDatabase(IServiceProvider provider, CancellationToken cancellation)
        {
            var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
            var logger = loggerFactory.CreateLogger<Program>();

            using var scope = provider.CreateScope();
            using var context = scope.ServiceProvider.GetRequiredService<RatDbContext>();

            var pendingMigration = (await context.Database.GetPendingMigrationsAsync(cancellation)).ToArray();
            logger.LogInformation($"Number of pending migrations: {pendingMigration.Length}", Array.Empty<object>());

            await context.Database.MigrateAsync(cancellation);
            logger.LogInformation("Database migrated successfully", Array.Empty<object>());

            var migrations = (await context.Database.GetAppliedMigrationsAsync(cancellation)).ToArray();

            logger.LogInformation($"Total number of applied migrations: {migrations.Length}", Array.Empty<object>());

            var builder = new StringBuilder();
            for (int i = 0; i < migrations.Length; i++)
            {
                builder.AppendLine($"Migration [{i}]: {migrations[i]}");
            }

            logger.LogInformation(builder.ToString(), Array.Empty<object>());
        }
    }
}