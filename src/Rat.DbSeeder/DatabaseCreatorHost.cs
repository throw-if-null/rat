using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rat.Data;

namespace Rat.DbSeeder
{
    public class DatabaseCreatorHost : IHostedService
    {
        private readonly RatDbContext _context;
        private readonly ILogger _logger;

        public DatabaseCreatorHost(RatDbContext context, ILogger<DatabaseCreatorHost> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var pendingMigration = await _context.Database.GetPendingMigrationsAsync(cancellationToken);
            _logger.LogInformation($"Number of pending migrations: {pendingMigration.Count()}");

            await _context.Database.MigrateAsync(cancellationToken);
            _logger.LogInformation("Database migrated successfully");

            var migrations = await _context.Database.GetAppliedMigrationsAsync(cancellationToken);

            _logger.LogInformation($"Total number of applied migrations: {migrations.Count()}");

            foreach (var migration in migrations)
            {
                _logger.LogInformation(migration);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
