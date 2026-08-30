using LensBook.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace LensBook.DATA
{
    public class ApplicationDbContext
       : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }


        // Domain Entities

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Photographer> Photographers { get; set; }

        public DbSet<SessionType> SessionTypes { get; set; }

        public DbSet<Booking> Bookings { get; set; }

        public DbSet<ExternalSchedule> ExternalSchedules { get; set; }


        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Customer - ApplicationUser
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            // Photographer - ApplicationUser
            modelBuilder.Entity<Photographer>()
                .HasOne(p => p.User)
                .WithOne(u => u.Photographer)
                .HasForeignKey<Photographer>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            // Customer - Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Customer)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);


            // Photographer - Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Photographer)
                .WithMany(p => p.Bookings)
                .HasForeignKey(b => b.PhotographerId)
                .OnDelete(DeleteBehavior.Restrict);


            // SessionType - Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.SessionType)
                .WithMany(s => s.Bookings)
                .HasForeignKey(b => b.SessionTypeId)
                .OnDelete(DeleteBehavior.Restrict);


            // Photographer - ExternalSchedule
            modelBuilder.Entity<ExternalSchedule>()
                .HasOne(e => e.Photographer)
                .WithMany(p => p.ExternalSchedules)
                .HasForeignKey(e => e.PhotographerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
