using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using Dapper;
using System.Data;

namespace AuctionHouseAPI.Domain.Dapper.Repositories
{
    public class DapperTagRepository : DapperBaseRepository<Tag>, ITagRepository
    {
        public DapperTagRepository(DapperContext dapperContext) : base(dapperContext) {}

        public override async Task<int> CreateAsync(Tag entity)
        {
            await OpenConnection();
            var sql = "INSERT INTO \"Tags\" (\"Name\") VALUES (@Name) RETURNING \"Id\";";
            var tagId = await _connection!.ExecuteScalarAsync<int>(sql, new { entity.Name }, _currentTransaction);
            await CloseConnection();
            return tagId;
        }

        public override async Task DeleteAsync(Tag entity)
        {
            await OpenConnection();
            var sql = "DELETE FROM \"Tags\" WHERE \"Id\" = @Id;";
            await _connection!.ExecuteAsync(sql, new { entity.Id }, _currentTransaction);
            await CloseConnection();
        }

        public override async Task<IEnumerable<Tag>> GetAllAsync()
        {
            await OpenConnection();
            var sql = "SELECT * FROM \"Tags\";";
            var result = await _connection!.QueryAsync<Tag>(sql);
            await CloseConnection();
            return result.ToList();
        }

        public override async Task<Tag?> GetByIdAsync(int id)
        {
            await OpenConnection();
            var sql = "SELECT * FROM \"Tags\" WHERE \"Id\" = @id";
            var result = await _connection!.QueryFirstOrDefaultAsync<Tag>(sql, new { id });
            await CloseConnection();
            return result;
        }

        public async Task<Tag?> GetByNameAsync(string name)
        {
            await OpenConnection();
            var sql = "SELECT * FROM \"Tags\" WHERE \"Name\" = @name";
            var result = await _connection!.QueryFirstOrDefaultAsync<Tag>(sql, new { name });
            await CloseConnection();
            return result;
        }
    }
}
