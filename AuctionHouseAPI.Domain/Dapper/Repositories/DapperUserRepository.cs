using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using Dapper;
using System.Data;

namespace AuctionHouseAPI.Domain.Dapper.Repositories
{
    public class DapperUserRepository : DapperBaseRepository<User>, IUserRepository
    {
        public DapperUserRepository(DapperContext dapperContext) : base(dapperContext) {}

        public override async Task<int> CreateAsync(User entity)
        {
            await OpenConnection();
            var sql = """INSERT INTO "Users" ("Username", "Email", "Password", "FirstName", "LastName", "Role") VALUES (@Username, @Email, @Password, @FirstName, @LastName, @Role) RETURNING "Id";""";
            var userId = await _connection!.ExecuteScalarAsync<int>(sql, new { entity.Username, entity.Email, entity.Password, entity.FirstName, entity.LastName, Role = entity.Role.ToString() }, _currentTransaction);
            await CloseConnection();
            return userId;
        }

        public override async Task DeleteAsync(User entity)
        {
            await OpenConnection();
            var sql = """DELETE FROM "Users" WHERE "Id" = @Id;""";
            await _connection!.ExecuteAsync(sql, new { entity.Id }, _currentTransaction);
            await CloseConnection();
        }

        public override async Task<IEnumerable<User>> GetAllAsync()
        {
            await OpenConnection();
            var sql = """SELECT * FROM "Users";""";
            var result = await _connection!.QueryAsync<User>(sql);
            await CloseConnection();
            return result.ToList();
        }

        public override async Task<User?> GetByIdAsync(int id)
        {
            await OpenConnection();
            var sql = """SELECT * FROM "Users" WHERE "Id" = @id""";
            var result = await _connection!.QueryFirstOrDefaultAsync<User>(sql, new { id });
            await CloseConnection();
            return result;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            await OpenConnection();
            var sql = """SELECT * FROM "Users" WHERE "Username" = @username""";
            var result = await _connection!.QueryFirstOrDefaultAsync<User>(sql, new { username });
            await CloseConnection();
            return result;
        }

        public async Task UpdateUserAsync(User user)
        {
            await OpenConnection();
            var sql = """
                UPDATE "Users
                "
                SET "Email" = @Email,
                "Password" = @Password,
                "FirstName" = @FirstName,
                "LastName" = @LastName
                WHERE "Id" = @Id;
            """;
            
            await _connection!.ExecuteAsync(sql, new { user.Email, user.Password, user.FirstName, user.LastName, user.Id }, _currentTransaction);
            await CloseConnection();
        }
    }
}
