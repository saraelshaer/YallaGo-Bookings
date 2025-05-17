using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YallaGo.DAL.Models;
namespace YallaGo.DAL
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<OfferTour> OfferTours { get; set; }
        public DbSet<WishList> wishLists { get; set; }
        public DbSet<WishListItems> wishListItems { get; set; } 


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OfferTour>()
                .HasKey(ot => new { ot.TourId, ot.OfferId });

            modelBuilder.Entity<User>()
                .Property(u => u.ImageURL)
                .HasDefaultValue("defaultUserImage.png");

            modelBuilder.Entity<Tour>(cfg =>
            {

                cfg.Property(t => t.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
            });

            base.OnModelCreating(modelBuilder);


        }


    }

}







