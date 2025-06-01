CREATE OR REPLACE FUNCTION CloseAuction()
RETURNS TRIGGER AS $$
DECLARE
	r_auction_options RECORD;
BEGIN
	SELECT * INTO r_auction_options FROM AuctionOptions WHERE Id = NEW.AuctionId FOR UPDATE;

	IF r_auction_options.AllowBuyItNow AND r_auction_options.BuyItNowPrice <= NEW.Amount
	THEN
		r_auction_options.IsActive := false;
	END IF;

	UPDATE AuctionOptions SET IsActive = r_auction_options.IsActive WHERE Id = r_auction_options.Id;
	RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER CheckIfBidIsValidForBuyItNow
AFTER INSERT ON "Bids"
FOR EACH ROW
EXECUTE FUNCTION CloseAuction();