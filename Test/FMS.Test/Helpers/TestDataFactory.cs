using Bogus;
using FleetManagementSystem.Application.Commands.Trip.Create;
using FleetManagementSystem.Domain.Entities;
using FleetManagementSystem.Domain.Enums;

namespace FleetManagementSystem.UnitTests.Helpers;

public static class TestDataFactory
{
   // ===== Driver Factory =====
    public static Faker<Driver> CreateDriverFaker()
    {
        return new Faker<Driver>()
            .RuleFor(d => d.Id, f => f.IndexFaker + 1)
            .RuleFor(d => d.UserId, f => f.Random.Int())
            .RuleFor(d => d.PhoneNumber, f => f.Phone.PhoneNumber())
            .RuleFor(d => d.NationalId, f => f.Random.Int())
            .RuleFor(d => d.Status, DriverStatus.Available)
            .RuleFor(d => d.HireDate, f => f.Date.Past(2));
    }



    public static Driver CreateAvailableDriver()
    {
        var driver = CreateDriverFaker().Generate();
        driver.Status = DriverStatus.Available;
        driver.DriverLicense = CreateValidDriverLicense();
        return driver;
    }

    public static Driver CreateBusyDriver()
    {
        var driver = CreateDriverFaker().Generate();
        driver.Status = DriverStatus.InTrip;
        return driver;
    }

    public static Driver CreateDriverWithoutLicense()
    {
        var driver = CreateDriverFaker().Generate();
        driver.Status = DriverStatus.Available;
        driver.DriverLicense = null;
        return driver;
    }

    // ===== Vehicle Factory =====
    public static Faker<Vehicle> CreateVehicleFaker()
    {
        return new Faker<Vehicle>()
            .RuleFor(v => v.Id, f => f.IndexFaker + 1)
            .RuleFor(v => v.PlateNumber, f => f.Random.String2(6, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"))
            .RuleFor(v => v.Model, f => f.Vehicle.Model())
            .RuleFor(v => v.Year, f => f.Date.Past(10).Year)
            .RuleFor(v => v.CurrentMileage, f => f.Random.Double(0, 100000))
            .RuleFor(v => v.Status, VehicleStatus.Available);
    }

    public static Vehicle CreateAvailableVehicle()
    {
        var vehicle = CreateVehicleFaker().Generate();
        vehicle.Status = VehicleStatus.Available;
        vehicle.VehicleLicense = CreateValidVehicleLicense();
        return vehicle;
    }

    public static Vehicle CreateBusyVehicle()
    {
        var vehicle = CreateVehicleFaker().Generate();
        vehicle.Status = VehicleStatus.InTrip;
        return vehicle;
    }

    public static Vehicle CreateVehicleWithoutLicense()
    {
        var vehicle = CreateVehicleFaker().Generate();
        vehicle.Status = VehicleStatus.Available;
        vehicle.VehicleLicense = null;
        return vehicle;
    }

    // ===== Licenses =====
    public static DriverLicense CreateValidDriverLicense()
    {
        return new DriverLicense
        {
            Id = 1,
            LicenseNumber = "DL" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
            IssueDate = DateTime.UtcNow.AddYears(-2),
            ExpiryDate = DateTime.UtcNow.AddYears(3),
            LicenseType = LicenseType.Private
        };
    }

    public static DriverLicense CreateExpiredDriverLicense()
    {
        return new DriverLicense
        {
            Id = 1,
            LicenseNumber = "DL" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
            IssueDate = DateTime.UtcNow.AddYears(-5),
            ExpiryDate = DateTime.UtcNow.AddDays(-1),
            LicenseType = LicenseType.Private
        };
    }

    public static VehicleLicense CreateValidVehicleLicense()
    {
        return new VehicleLicense
        {
            Id = 1,
            LicenseNumber = "VL" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
            ExpirationDate = DateTime.UtcNow.AddYears(4)
        };
    }

    public static VehicleLicense CreateExpiredVehicleLicense()
    {
        return new VehicleLicense
        {
            Id = 1,
            LicenseNumber = "VL" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
            ExpirationDate = DateTime.UtcNow.AddDays(-1)
        };
    }

    // ===== Trip =====
    public static Faker<Domain.Entities.Trip> CreateTripFaker()
    {
        return new Faker<Domain.Entities.Trip>()
            .RuleFor(t => t.Id, f => f.IndexFaker + 1)
            .RuleFor(t => t.DriverId, f => f.Random.Int(1, 100))
            .RuleFor(t => t.VehicleId, f => f.Random.Int(1, 100))
            .RuleFor(t => t.StartLocation, f => f.Address.City())
            .RuleFor(t => t.EndLocation, f => f.Address.City())
            .RuleFor(t => t.Distance, f => f.Random.Double(50, 1000))
            .RuleFor(t => t.CargoWeight, f => f.Random.Double(0, 500))
            .RuleFor(t => t.Status, TripStatus.Started)
            .RuleFor(t => t.StartDate, f => f.Date.Recent());
    }

    // ===== Commands =====
    public static CreateTripCommand CreateValidTripCommand()
    {
        return new CreateTripCommand
        (
             1,
             1,
             "Cairo",
             "Alexandria",
             220,
             50       
        );
    }

    public static CreateTripCommand CreateInvalidTripCommand()
    {
        return new CreateTripCommand
        (
            0,
             0,
             "",
             "",
             -100,
             -50
        );
    }
}