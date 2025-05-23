﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YallaGo.DAL.Consts;
using YallaGo.DAL.Models;
namespace YallaGo.DAL
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tour> Tours { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<Booking> Bookings { get; set; }




        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



            modelBuilder.Entity<User>(cfg =>
            {
                cfg.Property(u => u.JoinedAt)
                    .HasDefaultValueSql("GETDATE()");

                cfg.Property(u => u.ImageURL)
              .HasDefaultValue("defaultUserImage.png");
            });
           

            modelBuilder.Entity<Tour>(cfg =>
            {

                cfg.Property(t => t.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Booking>(cfg =>
            {
                cfg.Property(b => b.BookingDate)
                    .HasDefaultValueSql("GETDATE()");

                cfg.Property(b => b.Status)
                    .HasConversion<string>()
                    .HasDefaultValue(BookingStatus.Pending);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}







