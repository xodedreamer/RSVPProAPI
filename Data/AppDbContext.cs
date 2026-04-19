using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RSVPProAPI.Models;
using Microsoft.EntityFrameworkCore.Design;

namespace RSVPProAPI.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
       // public AppDbContext(DbContextOptions<AppDbContext> options) :  DbContext(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Rsvp> Rsvps => Set<Rsvp>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Fluent API configurations
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

            // RSVP Relationships
            modelBuilder.Entity<Rsvp>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rsvps)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<Rsvp>()
                .HasOne(r => r.Event)
                .WithMany(e => e.Rsvps)
                .HasForeignKey(r => r.EventId);

            // Optional: Prevent duplicate RSVPs
            modelBuilder.Entity<Rsvp>()
                .HasIndex(r => new { r.UserId, r.EventId })
                .IsUnique();
        }
    }
}
