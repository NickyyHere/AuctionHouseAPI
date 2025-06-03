using Npgsql;
using System.Data;

namespace AuctionHouseAPI.Domain.Dapper
{
    public class DapperContext
    {
        private readonly string _connectionString;
        public DapperContext()
        {
            _connectionString = Environment.GetEnvironmentVariable("PGSQL_CONNECTION_STRING")!;
        }

        public IDbConnection CreateConnection()
            => new NpgsqlConnection(_connectionString);
    }
}
