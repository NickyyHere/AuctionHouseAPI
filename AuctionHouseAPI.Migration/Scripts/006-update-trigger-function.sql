CREATE OR REPLACE FUNCTION CloseAuction()
RETURNS TRIGGER AS $$
DECLARE
	r_auction_options RECORD;
BEGIN
	SELECT * INTO r_auction_options FROM "AuctionOptions" WHERE "AuctionId" = NEW."AuctionId" FOR UPDATE;

	IF r_auction_options."AllowBuyItNow" AND r_auction_options."BuyItNowPrice" <= NEW."Amount"
	THEN
		r_auction_options."FinishDateTime" := CURRENT_TIMESTAMP AT TIME ZONE 'UTC';
	END IF;

	UPDATE "AuctionOptions" SET "FinishDateTime" = r_auction_options."FinishDateTime" WHERE "AuctionId" = r_auction_options."AuctionId";
	RETURN NEW;
END;
$$ LANGUAGE plpgsql;