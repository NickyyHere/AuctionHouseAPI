using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using Dapper;
using System.Data;

namespace AuctionHouseAPI.Domain.Dapper.Repositories
{
    public class DapperCategoryRepository : DapperBaseRepository<Category>, ICategoryRepository
    {
        public DapperCategoryRepository(DapperContext dapperContext) : base(dapperContext){}

        public override async Task<int> CreateAsync(Category entity)
        {
            await OpenConnection();
            var sql = """INSERT INTO "Categories" ("Name", "Description") VALUES (@Name, @Description) RETURNING "Id";""";
            var categoryId = await _connection!.ExecuteScalarAsync<int>(sql, new { entity.Name, entity.Description }, _currentTransaction);
            await CloseConnection();
            return categoryId;
        }

        public override async Task DeleteAsync(Category entity)
        {
            await OpenConnection();
            var sql = """DELETE FROM "Categories" WHERE "Id" = @Id;""";
            await _connection!.ExecuteAsync(sql, new { entity.Id }, _currentTransaction);
            await CloseConnection();
        }

        public override async Task<IEnumerable<Category>> GetAllAsync()
        {
            await OpenConnection();
            var sql = """SELECT * FROM "Categories";""";
            var result = await _connection!.QueryAsync<Category>(sql);
            await CloseConnection();
            return result.ToList();
        }

        public override async Task<Category?> GetByIdAsync(int id)
        {
            await OpenConnection();
            var sql = """SELECT * FROM "Categories" WHERE "Id" = @id""";
            var result = await _connection!.QueryFirstOrDefaultAsync<Category>(sql, new { id });
            await CloseConnection();
            return result;
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            await OpenConnection();
            var sql = """
                UPDATE "Categories"
                SET "Name" = @Name,
                "Description" = @Description
                WHERE "Id" = @Id;
            """;
            await _connection!.ExecuteAsync(sql, new { category.Name, category.Description, category.Id }, _currentTransaction);
            await CloseConnection();
        }
    }
}
