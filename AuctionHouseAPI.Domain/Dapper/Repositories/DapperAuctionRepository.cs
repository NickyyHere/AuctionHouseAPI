using AuctionHouseAPI.Domain.Interfaces;
using AuctionHouseAPI.Domain.Models;
using Dapper;
using System;
using System.Data;

namespace AuctionHouseAPI.Domain.Dapper.Repositories
{
    public class DapperAuctionRepository : DapperBaseRepository<Auction>, IAuctionRepository
    {
        public DapperAuctionRepository(DapperContext dapperContext) : base(dapperContext) {}

        public override async Task<int> CreateAsync(Auction entity)
        {
            await OpenConnection();
            var insertAuctionSql = """INSERT INTO "Auctions" ("OwnerId") VALUES (@OwnerId) RETURNING "Id";""";

            var auctionId = await _connection!.ExecuteScalarAsync<int>(insertAuctionSql, new { entity.OwnerId }, _currentTransaction);

            entity.Item!.AuctionId = entity.Options!.AuctionId = auctionId;

            var item = entity.Item;
            var insertItemSql = """INSERT INTO "AuctionItems" ("AuctionId", "Name", "Description", "CategoryId") VALUES (@AuctionId, @Name, @Description, @CategoryId);""";
            await _connection!.ExecuteAsync(insertItemSql, new { item.AuctionId, item.Name, item.Description, item.CategoryId }, _currentTransaction);

            foreach(var tag in item.Tags)
            {
                var insertItemTagSql = """INSERT INTO "ItemTags" ("AuctionItemId", "TagId") VALUES (@AuctionItemId, @TagId);""";
                await _connection!.ExecuteAsync(insertItemTagSql, new { AuctionItemId = item.AuctionId, tag.TagId }, _currentTransaction);

            }

            var options = entity.Options;
            var insertOptionsSql = """
                
                INSERT INTO "AuctionOptions" (
                    "AuctionId",
                    "StartingPrice",
                    "StartDateTime",
                    "FinishDateTime",
                    "IsIncreamentalOnLastMinuteBid",
                    "MinutesToIncrement",
                    "MinimumOutbid",
                    "AllowBuyItNow",
                    "BuyItNowPrice",
                    "IsActive"
                ) VALUES (
                    @AuctionId,
                    @StartingPrice,
                    @StartDateTime,
                    @FinishDateTime,
                    @IsIncreamentalOnLastMinuteBid,
                    @MinutesToIncrement,
                    @MinimumOutbid,
                    @AllowBuyItNow,
                    @BuyItNowPrice,
                    @IsActive
                );
                """;
            await _connection!.ExecuteAsync(insertOptionsSql, new
            {
                options.AuctionId,
                options.StartingPrice,
                options.StartDateTime,
                options.FinishDateTime,
                options.IsIncreamentalOnLastMinuteBid,
                options.MinutesToIncrement,
                options.MinimumOutbid,
                options.AllowBuyItNow,
                options.BuyItNowPrice,
                options.IsActive
            }, _currentTransaction);
            await CloseConnection();
            return auctionId;
        }

        public override async Task DeleteAsync(Auction entity)
        {
            await OpenConnection();
            var sql = """DELETE FROM "Auctions" WHERE "Id" = @Id;""";
            await _connection!.ExecuteAsync(sql, new { entity.Id }, _currentTransaction);
            await CloseConnection();
        }

        public async Task<IEnumerable<Auction>> GetActiveAsync()
        {
            await OpenConnection();
            var sql = """
                SELECT 
                    a.*, 
                    i."AuctionId" AS "ItemId", i."AuctionId", i."Name", i."Description", i."CategoryId",
                    o."AuctionId" AS "OptionsId", o."StartingPrice", o."StartDateTime", o."FinishDateTime", 
                    o."IsIncreamentalOnLastMinuteBid", o."MinutesToIncrement", o."MinimumOutbid",
                    o."AllowBuyItNow", o."BuyItNowPrice", o."IsActive",
                    u."Id" AS "OwnerId", u."FirstName", u."LastName"
                FROM "Auctions" a
                JOIN "AuctionItems" i ON a."Id" = i."AuctionId"
                JOIN "AuctionOptions" o ON a."Id" = o."AuctionId"
                JOIN "Users" u ON a."OwnerId" = u."Id"
                WHERE o."IsActive" = true;
                """;

            var result = await _connection!.QueryAsync<Auction, AuctionItem, AuctionOptions, User, Auction>(
                sql,
                (a, item, options, user) =>
                {
                    a.Item = item;
                    a.Options = options;
                    a.Owner = user;
                    return a;
                },
                splitOn: "ItemId,OptionsId,OwnerId"
            );

            var auctions = result.ToList();

            foreach (var auction in auctions)
            {
                var tagSql = """
                    SELECT t.*
                    FROM "Tags" t
                    JOIN "ItemTags" it ON t."Id" = it."TagId"
                    WHERE it."AuctionItemId" = @AuctionItemId
                    """;

                var tagResult = await _connection!.QueryAsync<Tag>(tagSql, new { AuctionItemId = auction.Id });

                foreach (var tag in tagResult)
                {
                    auction.Item!.Tags.Add(new AuctionItemTag { AuctionItem = auction.Item, AuctionItemId = auction.Item.AuctionId, Tag = tag, TagId = tag.Id });
                }
            }
            await CloseConnection();
            return auctions;
        }

        public override async Task<IEnumerable<Auction>> GetAllAsync()
        {
            await OpenConnection();
            var sql = """
                SELECT 
                    a.*, 
                    i."AuctionId" AS "ItemId", i."AuctionId", i."Name", i."Description", i."CategoryId",
                    o."AuctionId" AS "OptionsId", o."StartingPrice", o."StartDateTime", o."FinishDateTime", 
                    o."IsIncreamentalOnLastMinuteBid", o."MinutesToIncrement", o."MinimumOutbid",
                    o."AllowBuyItNow", o."BuyItNowPrice", o."IsActive",
                    u."Id" AS "OwnerId", u."FirstName", u."LastName"
                FROM "Auctions" a
                JOIN "AuctionItems" i ON a."Id" = i."AuctionId"
                JOIN "AuctionOptions" o ON a."Id" = o."AuctionId"
                JOIN "Users" u ON a."OwnerId" = u."Id";
            """;

            var result = await _connection!.QueryAsync<Auction, AuctionItem, AuctionOptions, User, Auction>(
                sql,
                (a, item, options, user) =>
                {
                    a.Item = item;
                    a.Options = options;
                    a.Owner = user;
                    return a;
                },
                splitOn: "ItemId,OptionsId,OwnerId"
            );

            var auctions = result.ToList();

            foreach (var auction in auctions)
            {
                var tagSql = """
                    
                    SELECT t.*
                    FROM "Tags" t
                    JOIN "ItemTags" it ON t."Id" = it."TagId"
                    WHERE it."AuctionItemId" = @AuctionItemId
                    """;

                var tagResult = await _connection!.QueryAsync<Tag>(tagSql, new { AuctionItemId = auction.Id });

                foreach (var tag in tagResult)
                {
                    auction.Item!.Tags.Add(new AuctionItemTag { AuctionItem = auction.Item, AuctionItemId = auction.Item.AuctionId, Tag = tag, TagId = tag.Id });
                }
            }
            await CloseConnection();
            return auctions;
        }


        public async Task<IEnumerable<Auction>> GetByCategoryIdAsync(int categoryId)
        {
            await OpenConnection();
            var sql = """
                SELECT 
                    a.*, 
                    i."AuctionId" AS "ItemId", i."AuctionId", i."Name", i."Description", i."CategoryId",
                    o."AuctionId" AS "OptionsId", o."StartingPrice", o."StartDateTime", o."FinishDateTime", 
                    o."IsIncreamentalOnLastMinuteBid", o."MinutesToIncrement", o."MinimumOutbid",
                    o."AllowBuyItNow", o."BuyItNowPrice", o."IsActive",
                    u."Id" AS "OwnerId", u."FirstName", u."LastName"
                FROM "Auctions" a
                JOIN "AuctionItems" i ON a."Id" = i."AuctionId"
                JOIN "AuctionOptions" o ON a."Id" = o."AuctionId"
                JOIN "Users" u ON a."OwnerId" = u."Id"
                WHERE i."CategoryId" = @categoryId;
                """;

            var result = await _connection!.QueryAsync<Auction, AuctionItem, AuctionOptions, User, Auction>(
                sql,
                (a, item, options, user) =>
                {
                    a.Item = item;
                    a.Options = options;
                    a.Owner = user;
                    return a;
                },
                new { categoryId },
                splitOn: "ItemId,OptionsId,OwnerId"
            );

            var auctions = result.ToList();

            foreach (var auction in auctions)
            {
                var tagSql = """
                    SELECT t.*
                    FROM "Tags" t
                    JOIN "ItemTags" it ON t."Id" = it."TagId"
                    WHERE it."AuctionItemId" = @AuctionItemId
                    """;

                var tagResult = await _connection!.QueryAsync<Tag>(tagSql, new { AuctionItemId = auction.Id });

                foreach (var tag in tagResult)
                {
                    auction.Item!.Tags.Add(new AuctionItemTag { AuctionItem = auction.Item, AuctionItemId = auction.Item.AuctionId, Tag = tag, TagId = tag.Id });
                }
            }
            await CloseConnection();
            return auctions;
        }

        public async override Task<Auction?> GetByIdAsync(int id)
        {
            using var connection = _context.CreateConnection();
            if (connection.State != ConnectionState.Open && connection is Npgsql.NpgsqlConnection pgSqlConnection)
            {
                await pgSqlConnection.OpenAsync();
            }
            else
            {
                connection.Open();
            }

            var sql = """
                SELECT 
                    a.*, 
                    i."AuctionId" AS "ItemId", i."AuctionId", i."Name", i."Description", i."CategoryId",
                    o."AuctionId" AS "OptionsId", o."StartingPrice", o."StartDateTime", o."FinishDateTime", 
                    o."IsIncreamentalOnLastMinuteBid", o."MinutesToIncrement", o."MinimumOutbid",
                    o."AllowBuyItNow", o."BuyItNowPrice", o."IsActive",
                    u."Id" AS "OwnerId", u."FirstName", u."LastName"
                FROM "Auctions" a
                JOIN "AuctionItems" i ON a."Id" = i."AuctionId"
                JOIN "AuctionOptions" o ON a."Id" = o."AuctionId"
                JOIN "Users" u ON a."OwnerId" = u."Id"
                WHERE a."Id" = @id;
            """;

            var result = await connection.QueryAsync<Auction, AuctionItem, AuctionOptions, User, Auction>(
                sql,
                (a, item, options, user) =>
                {
                    a.Item = item;
                    a.Options = options;
                    a.Owner = user;
                    return a;
                },
                new { id },
                splitOn: "ItemId,OptionsId,OwnerId"
            );

            var auctions = result.ToList();

            foreach (var auction in auctions)
            {
                var tagSql = """
                    SELECT t.*
                    FROM "Tags" t
                    JOIN "ItemTags" it ON t."Id" = it."TagId"
                    WHERE it."AuctionItemId" = @AuctionItemId
                    """;

                var tagResult = await connection.QueryAsync<Tag>(tagSql, new { AuctionItemId = auction.Id });

                foreach (var tag in tagResult)
                {
                    auction.Item!.Tags.Add(new AuctionItemTag { AuctionItem = auction.Item, AuctionItemId = auction.Item.AuctionId, Tag = tag, TagId = tag.Id });
                }
            }

            return auctions.FirstOrDefault();
        }

        public async Task<IEnumerable<Auction>> GetByTagAsync(string tag)
        {
            using var connection = _context.CreateConnection();
            if (connection.State != ConnectionState.Open && connection is Npgsql.NpgsqlConnection pgSqlConnection)
            {
                await pgSqlConnection.OpenAsync();
            }
            else
            {
                connection.Open();
            }

            var sql = """
                SELECT 
                    a.*, 
                    i."AuctionId" AS "ItemId", i."AuctionId", i."Name", i."Description", i."CategoryId",
                    o."AuctionId" AS "OptionsId", o."StartingPrice", o."StartDateTime", o."FinishDateTime", 
                    o."IsIncreamentalOnLastMinuteBid", o."MinutesToIncrement", o."MinimumOutbid",
                    o."AllowBuyItNow", o."BuyItNowPrice", o."IsActive",
                    u."Id" AS "OwnerId", u."FirstName", u."LastName"
                FROM "Auctions" a
                JOIN "AuctionItems" i ON a."Id" = i."AuctionId"
                JOIN "AuctionOptions" o ON a."Id" = o."AuctionId"
                JOIN "Users" u ON a."OwnerId" = u."Id"
                LEFT JOIN "ItemTags" it ON it."AuctionItemId" = i."AuctionId"
                LEFT JOIN "Tags" t ON it."TagId" = t."Id"
                WHERE t."Name" = @tag;
                """;

            var result = await connection.QueryAsync<Auction, AuctionItem, AuctionOptions, User, Auction>(
                sql,
                (a, item, options, user) =>
                {
                    a.Item = item;
                    a.Options = options;
                    a.Owner = user;
                    return a;
                },
                new { tag },
                splitOn: "ItemId,OptionsId,OwnerId"
            );

            var auctions = result.ToList();

            foreach (var auction in auctions)
            {
                var tagSql = """
                    SELECT t.*
                    FROM "Tags" t
                    JOIN "ItemTags" it ON t."Id" = it."TagId"
                    WHERE it."AuctionItemId" = @AuctionItemId
                    """;

                var tagResult = await connection.QueryAsync<Tag>(tagSql, new { AuctionItemId = auction.Id });

                foreach (var t in tagResult)
                {
                    auction.Item!.Tags.Add(new AuctionItemTag { AuctionItem = auction.Item, AuctionItemId = auction.Item.AuctionId, Tag = t, TagId = t.Id });
                }
            }

            return auctions;
        }

        public async Task<IEnumerable<Auction>> GetByUserAsync(int userId)
        {
            await OpenConnection();
            var sql = """
                SELECT 
                    a.*, 
                    i."AuctionId" AS "ItemId", i."AuctionId", i."Name", i."Description", i."CategoryId",
                    o."AuctionId" AS "OptionsId", o."StartingPrice", o."StartDateTime", o."FinishDateTime", 
                    o."IsIncreamentalOnLastMinuteBid", o."MinutesToIncrement", o."MinimumOutbid",
                    o."AllowBuyItNow", o."BuyItNowPrice", o."IsActive",
                    u."Id" AS "OwnerId", u."FirstName", u."LastName"
                FROM "Auctions" a
                JOIN "AuctionItems" i ON a."Id" = i."AuctionId"
                JOIN "AuctionOptions" o ON a."Id" = o."AuctionId"
                JOIN "Users" u ON a."OwnerId" = u."Id"
                WHERE a."OwnerId" = @userId;
                """;

            var result = await _connection!.QueryAsync<Auction, AuctionItem, AuctionOptions, User, Auction>(
                sql,
                (a, item, options, user) =>
                {
                    a.Item = item;
                    a.Options = options;
                    a.Owner = user;
                    return a;
                },
                new { userId },
                splitOn: "ItemId,OptionsId,OwnerId"
            );

            var auctions = result.ToList();

            foreach (var auction in auctions)
            {
                var tagSql = """
                    SELECT t.*
                    FROM "Tags" t
                    JOIN "ItemTags" it ON t."Id" = it."TagId"
                    WHERE it."AuctionItemId" = @AuctionItemId
                    """;

                var tagResult = await _connection!.QueryAsync<Tag>(tagSql, new { AuctionItemId = auction.Id });

                foreach (var tag in tagResult)
                {
                    auction.Item!.Tags.Add(new AuctionItemTag { AuctionItem = auction.Item, AuctionItemId = auction.Item.AuctionId, Tag = tag, TagId = tag.Id });
                }
            }
            await CloseConnection();
            return auctions;
        }

        public async Task UpdateAuctionItemAsync(AuctionItem item)
        {
            await OpenConnection();
            var sql = """
                UPDATE "AuctionItems"
                SET "Name" = @Name,
                "Description" = @Description,
                "CategoryId" = @CategoryId
                WHERE "AuctionId" = @AuctionId
                """;
            await _connection!.ExecuteAsync(sql, new { item.Name, item.Description, item.CategoryId, item.AuctionId }, _currentTransaction);
            await CloseConnection();
        }

        public async Task UpdateAuctionOptionsAsync(AuctionOptions options)
        {
            await OpenConnection();
            var sql = """
                
                UPDATE "AuctionOptions"
                SET "StartingPrice" = @StartingPrice,
                "StartDateTime" = @StartDateTime,
                "FinishDateTime" = @FinishDateTime,
                "IsIncreamentalOnLastMinuteBid" = @IsIncreamentalOnLastMinuteBid,
                "MinutesToIncrement" = @MinutesToIncrement,
                "MinimumOutbid" = @MinimumOutbid,
                "AllowBuyItNow" = @AllowBuyItNow,
                "BuyItNowPrice" = @BuyItNowPrice,
                "IsActive" = @IsActive
                WHERE "AuctionId" = @AuctionId;
                """;
            await _connection!.ExecuteAsync(sql, new
            {
                options.StartingPrice,
                options.StartDateTime,
                options.FinishDateTime,
                options.IsIncreamentalOnLastMinuteBid,
                options.MinutesToIncrement,
                options.MinimumOutbid,
                options.AllowBuyItNow,
                options.BuyItNowPrice,
                options.IsActive,
                options.AuctionId
            }, _currentTransaction);
            await CloseConnection();
        }
    }
}
