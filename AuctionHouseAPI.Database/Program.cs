using System.Data;
using Dapper;
using Npgsql;

namespace DatabaseCreator;

public class Program
{
    public static async Task Main(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable("PGSQL_CONNECTION_STRING");
        var sqlFilePath = "Database.sql";

        if (!File.Exists(sqlFilePath))
        {
            Console.WriteLine($"SQL file not found at: {sqlFilePath}");
            return;
        }

        var sqlScript = await File.ReadAllTextAsync(sqlFilePath);

        try
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

            using var conn = new NpgsqlConnection(connectionString);
            await conn.OpenAsync();

            var batches = sqlScript.Split(';')
                .Where(b => !string.IsNullOrWhiteSpace(b))
                .Select(b => b.Trim());

            foreach (var batch in batches)
            {
                try
                {
                    await conn.ExecuteAsync(batch);
                    Console.WriteLine($"Executed: {batch.Substring(0, Math.Min(batch.Length, 50))}...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing batch: {batch.Substring(0, Math.Min(batch.Length, 50))}...");
                    Console.WriteLine(ex.Message);
                }
            }

            Console.WriteLine("Database setup completed!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting up database: {ex.Message}");
        }
    }
}