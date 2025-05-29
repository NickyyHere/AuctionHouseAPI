using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using Dapper;
using System.Data;

namespace AuctionHouseAPI.Domain.Dapper.Repositories
{
    public class DapperBidRepository : DapperBaseRepository<Bid>, IBidRepository
    {
        public DapperBidRepository(DapperContext dapperContext) : base(dapperContext) {}

        public override async Task<int> CreateAsync(Bid entity)
        {
            await OpenConnection();
            var sql = "INSERT INTO \"Bids\" (\"UserId\", \"AuctionId\", \"Amount\", \"PlacedDateTime\") VALUES (@UserId, @AuctionId, @Amount, @PlacedDateTime) RETURNING \"Id\";";
            var bidId = await _connection!.ExecuteScalarAsync<int>(sql, new { entity.UserId, entity.AuctionId, entity.Amount, entity.PlacedDateTime}, _currentTransaction);
            await CloseConnection();
            return bidId;
        }

        public override async Task DeleteAsync(Bid entity)
        {
            await OpenConnection();
            var sql = "DELETE FROM \"Bids\" WHERE \"Id\" = @Id;";
            await _connection!.ExecuteAsync(sql, new { entity.Id }, _currentTransaction);
            await CloseConnection();
        }

        public override async Task<IEnumerable<Bid>> GetAllAsync()
        {
            await OpenConnection();
            var sql = "SELECT * FROM \"Bids\";";
            var result = await _connection!.QueryAsync<Bid>(sql);
            await CloseConnection();
            return result.ToList();
        }

        public async Task<IEnumerable<Bid>> GetByAuctionAsync(int auctionId)
        {
            await OpenConnection();
            var sql = "SELECT * FROM \"Bids\" WHERE \"AuctionId\" = @auctionId;";
            var result = await _connection!.QueryAsync<Bid>(sql, new { auctionId });
            await CloseConnection();
            return result.ToList();
        }

        public override async Task<Bid?> GetByIdAsync(int id)
        {
            await OpenConnection();
            var sql = "SELECT * FROM \"Bids\" WHERE \"Id\" = @id";
            var result = await _connection!.QueryFirstOrDefaultAsync<Bid>(sql, new { id });
            await CloseConnection();
            return result;
        }

        public async Task<IEnumerable<Bid>> GetByUserAndAuctionAsync(int userId, int auctionId)
        {
            await OpenConnection();
            var sql = "SELECT * FROM \"Bids\" WHERE \"AuctionId\" = @auctionId AND \"UserId\" = @userId;";
            var result = await _connection!.QueryAsync<Bid>(sql, new { auctionId, userId });
            await CloseConnection();
            return result.ToList();
        }

        public async Task<IEnumerable<Bid>> GetByUserAsync(int userId)
        {
            await OpenConnection();
            var sql = "SELECT * FROM \"Bids\" WHERE \"UserId\" = @userId;";
            var result = await _connection!.QueryAsync<Bid>(sql, new { userId });
            await CloseConnection();
            return result.ToList();
        }
    }
}
