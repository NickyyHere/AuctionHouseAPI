using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionHouseAPI.Domain.Dapper
{
    class DapperContext
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
