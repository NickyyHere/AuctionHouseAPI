CREATE OR REPLACE FUNCTION IncrementTime()
RETURNS TRIGGER AS $$
DECLARE
	r_auction_options RECORD;
BEGIN
	SELECT * INTO r_auction_options FROM "AuctionOptions" WHERE "AuctionId" = NEW."AuctionId" FOR UPDATE;
	IF r_auction_options."IsIncreamentalOnLastMinuteBid" AND (r_auction_options."FinishDateTime" - (CURRENT_TIMESTAMP AT TIME ZONE 'UTC')) <= INTERVAL '1 minute' 
	THEN
	UPDATE "AuctionOptions" SET "FinishDateTime" = (r_auction_options."FinishDateTime" + (r_auction_options."MinutesToIncrement" || ' minutes')::INTERVAL)  WHERE "AuctionId" = r_auction_options."AuctionId";
	END IF;
	RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER IncrementTimeOnLastMinuteIfAllowed
AFTER INSERT ON "Bids"
FOR EACH ROW
EXECUTE FUNCTION IncrementTime();