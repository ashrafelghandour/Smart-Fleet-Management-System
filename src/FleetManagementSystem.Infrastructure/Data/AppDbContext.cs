

using FleetManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FleetManagementSystem.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) :
 IdentityDbContext<IdentityUser<int>,IdentityRole<int>,int>(options)
{
     public DbSet<Driver> Drivers { get; set; }
    public DbSet<DriverLicense> DriverLicenses { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<VehicleLicense> VehicleLicenses { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<FuelRecord> FuelRecords { get; set; }
    public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; }
    public DbSet<TripExpense> TripExpenses { get; set; }


   protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
          
          modelBuilder.Entity<Driver>()
          .HasOne(d => d.User)
          .WithOne(u => u.Driver)
          .HasForeignKey<Driver>(d => d.UserId)
            .OnDelete(DeleteBehavior.Restrict); 
       
        modelBuilder.Entity<DriverLicense>()
            .HasOne(dl => dl.Driver)
            .WithOne(d => d.DriverLicense)
            .HasForeignKey<DriverLicense>(dl => dl.DriverId)
            .OnDelete(DeleteBehavior.Cascade); 
       

        modelBuilder.Entity<VehicleLicense>()
            .HasOne(vl => vl.Vehicle)
            .WithOne(v => v.VehicleLicense)
            .HasForeignKey<VehicleLicense>(vl => vl.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);
       

     
        modelBuilder.Entity<Trip>()
            .HasOne(t => t.Vehicle)
            .WithMany(v => v.Trips)
            .HasForeignKey(t => t.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        // 5. Driver -> Trips (1:Many)
        modelBuilder.Entity<Trip>()
            .HasOne(t => t.Driver)
            .WithMany(d => d.Trips)
            .HasForeignKey(t => t.DriverId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FuelRecord>()
            .HasOne(f => f.Vehicle)
            .WithMany(v => v.FuelRecords)
            .HasForeignKey(f => f.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MaintenanceRecord>()
            .HasOne(m => m.Vehicle)
            .WithMany(v => v.MaintenanceRecords)
            .HasForeignKey(m => m.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TripExpense>()
            .HasOne(te => te.Trip)
            .WithMany(t => t.Expenses)
            .HasForeignKey(te => te.TripId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Driver>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Vehicle>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Trip>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<DriverLicense>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<VehicleLicense>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<FuelRecord>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<MaintenanceRecord>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<TripExpense>().HasQueryFilter(e => !e.IsDeleted);

    }
  
}