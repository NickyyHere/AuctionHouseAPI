CREATE TABLE "AuctionItems" (
  "AuctionId" integer PRIMARY KEY NOT NULL,
  "Name" text NOT NULL,
  "Description" text NOT NULL,
  "CategoryId" integer NOT NULL
);

CREATE TABLE "AuctionOptions" (
  "AuctionId" integer PRIMARY KEY NOT NULL,
  "StartingPrice" numeric NOT NULL,
  "StartDateTime" timestamp NOT NULL,
  "FinishDateTime" timestamp NOT NULL,
  "IsIncreamentalOnLastMinuteBid" boolean NOT NULL,
  "MinutesToIncrement" integer NOT NULL,
  "MinimumOutbid" integer NOT NULL,
  "AllowBuyItNow" boolean NOT NULL,
  "BuyItNowPrice" numeric NOT NULL,
  "IsActive" boolean NOT NULL
);

CREATE TABLE "Auctions" (
  "Id" integer PRIMARY KEY NOT NULL,
  "OwnerId" integer NOT NULL
);

CREATE TABLE "Bids" (
  "Id" integer PRIMARY KEY NOT NULL,
  "UserId" integer NOT NULL,
  "AuctionId" integer NOT NULL,
  "Amount" numeric NOT NULL,
  "PlacedDateTime" timestamp NOT NULL
);

CREATE TABLE "Categories" (
  "Id" integer PRIMARY KEY NOT NULL,
  "Name" text NOT NULL,
  "Description" text NOT NULL
);

CREATE TABLE "ItemTags" (
  "AuctionItemId" integer NOT NULL,
  "TagId" integer NOT NULL,
  PRIMARY KEY ("AuctionItemId", "TagId")
);

CREATE TABLE "Tags" (
  "Id" integer PRIMARY KEY NOT NULL,
  "Name" text NOT NULL
);

CREATE TABLE "Users" (
  "Id" integer PRIMARY KEY NOT NULL,
  "Username" text NOT NULL UNIQUE,
  "Email" text NOT NULL UNIQUE,
  "Password" text NOT NULL,
  "FirstName" text NOT NULL,
  "LastName" text NOT NULL
);

CREATE INDEX "IX_AuctionItems_CategoryId" ON "AuctionItems" USING BTREE ("CategoryId");

CREATE INDEX "IX_AuctionItems_Name" ON "AuctionItems" USING BTREE ("Name");

CREATE INDEX "IX_Auctions_OwnerId" ON "Auctions" USING BTREE ("OwnerId");

CREATE INDEX "IX_Bids_AuctionId" ON "Bids" USING BTREE ("AuctionId");

CREATE INDEX "IX_Bids_UserId" ON "Bids" USING BTREE ("UserId");

CREATE INDEX "IX_ItemTags_TagId" ON "ItemTags" USING BTREE ("TagId");

CREATE INDEX "IX_Tags_Name" ON "Tags" USING BTREE ("Name");

ALTER TABLE "AuctionItems" ADD CONSTRAINT "FK_AuctionItems_Auctions_AuctionId" FOREIGN KEY ("AuctionId") REFERENCES "Auctions" ("Id") ON DELETE CASCADE;

ALTER TABLE "AuctionItems" ADD CONSTRAINT "FK_AuctionItems_Categories_CategoryId" FOREIGN KEY ("CategoryId") REFERENCES "Categories" ("Id");

ALTER TABLE "AuctionOptions" ADD CONSTRAINT "FK_AuctionOptions_Auctions_AuctionId" FOREIGN KEY ("AuctionId") REFERENCES "Auctions" ("Id") ON DELETE CASCADE;

ALTER TABLE "Auctions" ADD CONSTRAINT "FK_Auctions_Users_OwnerId" FOREIGN KEY ("OwnerId") REFERENCES "Users" ("Id") ON DELETE RESTRICT;

ALTER TABLE "Bids" ADD CONSTRAINT "FK_Bids_Auctions_AuctionId" FOREIGN KEY ("AuctionId") REFERENCES "Auctions" ("Id") ON DELETE CASCADE;

ALTER TABLE "Bids" ADD CONSTRAINT "FK_Bids_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT;

ALTER TABLE "ItemTags" ADD CONSTRAINT "FK_ItemTags_AuctionItems_AuctionItemId" FOREIGN KEY ("AuctionItemId") REFERENCES "AuctionItems" ("AuctionId") ON DELETE CASCADE;

ALTER TABLE "ItemTags" ADD CONSTRAINT "FK_ItemTags_Tags_TagId" FOREIGN KEY ("TagId") REFERENCES "Tags" ("Id") ON DELETE CASCADE;
