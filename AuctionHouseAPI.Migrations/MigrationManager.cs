using AuctionHouseAPI.Shared.Exceptions;
using Dapper;
using DbUp;
using Npgsql;
using System.Reflection;

namespace AuctionHouseAPI.Migrations
{
    public static class MigrationManager
    {
        public static async Task Run(string connectionString)
        {
            await CreateDatabaseIfNotExistAsync(connectionString);
            var updater = DeployChanges.To
                .PostgresqlDatabase(connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var result = updater.PerformUpgrade();

            if(!result.Successful)
            {
                throw new DatabaseUpdateException("Updating database failed");
            }
            Console.WriteLine("Successfuly updated database.");
        }
        private static async Task CreateDatabaseIfNotExistAsync(string connectionString)
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
