using AuctionHouseAPI.Shared.Exceptions;
using AuctionHouseAPI.Shared.Settings;
using Dapper;
using DbUp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Reflection;

namespace AuctionHouseAPI.Migrations
{
    public class MigrationHostedService : IHostedService
    {
        private readonly PgSqlDatabaseSettings _dbOptions;
        public MigrationHostedService(IOptions<PgSqlDatabaseSettings> options)
        {
            _dbOptions = options.Value;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await CreateDatabaseIfNotExistAsync(_dbOptions.ConnectionString!);
            var updater = DeployChanges.To
                .PostgresqlDatabase(_dbOptions.ConnectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .WithVariablesDisabled()
                .Build();

            var result = updater.PerformUpgrade();

            if (!result.Successful)
            {
                throw new DatabaseUpdateException("Updating database failed");
            }
            Console.WriteLine("Successfuly updated database.");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private async Task CreateDatabaseIfNotExistAsync(string connectionString)
        {
            var builder = new NpgsqlConnectionStringBuilder(connectionString);
            var dbName = builder.Database;
            builder.Database = "postgres";

            using var tempConn = new NpgsqlConnection(builder.ConnectionString);
            await tempConn.OpenAsync();

            var dbExists = await tempConn.QueryFirstOrDefaultAsync<bool>(
                "SELECT 1 FROM pg_database WHERE datname = @dbName",
                new { dbName });

            if (!dbExists)
            {
                Console.WriteLine($"Creating database: {dbName}");
                await tempConn.ExecuteAsync($"CREATE DATABASE \"{dbName}\"");
            }
        }
    }
}
