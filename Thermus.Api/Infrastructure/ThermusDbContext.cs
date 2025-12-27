using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Thermus.Api.Entities;

namespace Thermus.Api.Infrastructure
{
    public class ThermusDbContext : DbContext
    {
        public ThermusDbContext(DbContextOptions<ThermusDbContext> options)
            : base(options)
        {

        }

        public DbSet<Device> Devices => Set<Device>();
        public DbSet<Reading> Readings => Set<Reading>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Device>(e =>
            {
                e.HasIndex(d => d.ExternalId).IsUnique();

                e.Property(d => d.ExternalId)
                    .IsRequired()
                    .HasMaxLength(100);

                e.Property(d => d.Name).HasMaxLength(200);
                e.Property(d => d.Location).HasMaxLength(200);
            });

            modelBuilder.Entity<Reading>(e =>
            {
                e.Property(r => r.Temperature).HasPrecision(6, 2);
                e.Property(r => r.Humidity).HasPrecision(6, 2);

                // Si tu propiedad es DateTime TakenAtUtc:
                e.Property(r => r.TakenAtUtc)
                    .HasColumnType("timestamptz");

                e.HasOne(r => r.Device)
                    .WithMany() // si después agregás Device.Readings -> .WithMany(d => d.Readings)
                    .HasForeignKey(r => r.DeviceId)
                    .OnDelete(DeleteBehavior.Cascade);

                e.HasIndex(r => new { r.DeviceId, r.TakenAtUtc });
            });
        }
    }
}