using AuctionHouseAPI.Shared.Settings;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Data;

namespace AuctionHouseAPI.Domain.Dapper
{
    public class DapperContext
    {
        private readonly PgSqlDatabaseSettings _dbSettings;
        public DapperContext(IOptions<PgSqlDatabaseSettings> options)
        {
            _dbSettings = options.Value;
        }

        public IDbConnection CreateConnection()
            => new NpgsqlConnection(_dbSettings.ConnectionString);
    }
}
