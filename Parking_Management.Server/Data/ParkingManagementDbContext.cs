using Microsoft.EntityFrameworkCore;
using Parking_Management.Server.Models;

namespace Parking_Management.Server.Data
{
    public class ParkingManagementDbContext :DbContext
    {
        public ParkingManagementDbContext(DbContextOptions<ParkingManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<ParkingSpace> ParkingSpaces { get; set; } = null!;
        public DbSet<Subscription> Subscriptions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Email)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(x => x.PasswordHash)
                    .IsRequired();

                entity.HasIndex(x => x.Email)
                    .IsUnique();
            });

            // Parking Space configuration
            modelBuilder.Entity<ParkingSpace>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.SpaceNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(x => x.SpaceType)
                    .IsRequired();

                entity.HasIndex(x => x.SpaceNumber)
                    .IsUnique();
            });

            // Booking configuration
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Amount)
                    .HasPrecision(18, 2);

                entity.Property(x => x.Status)
                    .IsRequired();

                entity.HasOne(x => x.User)
                    .WithMany(x => x.Bookings)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.ParkingSpace)
                    .WithMany(x => x.Bookings)
                    .HasForeignKey(x => x.ParkingSpaceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Subscription configuration
            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Amount)
                    .HasPrecision(18, 2);

                entity.Property(x => x.Status)
                    .IsRequired();

                entity.HasOne(x => x.User)
                    .WithMany(x => x.Subscriptions)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.ParkingSpace)
                    .WithMany(x => x.Subscriptions)
                    .HasForeignKey(x => x.ParkingSpaceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
