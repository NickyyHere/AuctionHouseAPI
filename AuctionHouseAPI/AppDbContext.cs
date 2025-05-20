using AuctionHouseAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionHouseAPI
{
    public class AppDbContext : DbContext
    {
        public DbSet<Auction> Auctions {  get; set; }
        public DbSet<AuctionItem> AuctionItems { get; set; }
        public DbSet<AuctionOptions> AuctionOptions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<AuctionItemTag> ItemTags { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Item)
                .WithOne(ai => ai.Auction)
                .HasForeignKey<AuctionItem>(ai => ai.AuctionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Options)
                .WithOne(ao => ao.Auction)
                .HasForeignKey<AuctionOptions>(ao => ao.AuctionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Auction>()
                .HasMany(a => a.Bids)
                .WithOne(b => b.Auction)
                .HasForeignKey(b => b.AuctionId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Bids)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Owner)
                .WithMany(u => u.Auctions)
                .HasForeignKey(a => a.OwnerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<AuctionItem>()
                .HasOne(ai => ai.Category)
                .WithMany(c => c.AuctionItems)
                .HasForeignKey(ai => ai.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<AuctionItemTag>()
                .HasKey(ait => new { ait.AuctionItemId, ait.TagId });

            modelBuilder.Entity<AuctionItemTag>()
                .HasOne(ait => ait.Tag)
                .WithMany(t => t.AuctionItems)
                .HasForeignKey(ait => ait.TagId);

            modelBuilder.Entity<AuctionItemTag>()
                .HasOne(ait => ait.AuctionItem)
                .WithMany(ai => ai.Tags)
                .HasForeignKey(ait => ait.AuctionItemId);
        }
    }
}
