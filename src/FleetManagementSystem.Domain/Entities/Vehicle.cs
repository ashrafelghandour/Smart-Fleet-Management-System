using FleetManagementSystem.Domain.Common;
using FleetManagementSystem.Domain.Enums;

namespace FleetManagementSystem.Domain.Entities;

public class Vehicle : BaseEntity
{
    public string PlateNumber { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public double CurrentMileage { get; set; }
    public VehicleStatus Status { get; set; } = VehicleStatus.Available;

    public virtual VehicleLicense? VehicleLicense { get; set; }
    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();
    public virtual ICollection<FuelRecord> FuelRecords { get; set; } = new List<FuelRecord>();
    public virtual ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();


    public bool IsAvailable() => Status == VehicleStatus.Available;
    
    
    public void MarkAsInTrip()
    {
        if (!IsAvailable())
            throw new InvalidOperationException("Vehicle is not available");
        Status = VehicleStatus.InTrip;
    }
    
    public void MarkAsAvailable()
    {
        Status = VehicleStatus.Available;
    }
    
    public void UpdateMileage(double distance)
    {
        if (distance <= 0)
            throw new ArgumentException("Distance must be greater than zero");
        CurrentMileage += distance;
    }
}
