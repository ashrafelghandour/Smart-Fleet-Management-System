using FleetManagementSystem.Domain.Common;
using FleetManagementSystem.Domain.Enums;

namespace FleetManagementSystem.Domain.Entities;

public class Trip : BaseEntity
{
    
    public int VehicleId { get; set; }
    public int DriverId { get; set; }
    
    public string StartLocation { get; set; } = string.Empty;
    public string EndLocation { get; set; } = string.Empty;
    public double Distance { get; set; }
    public double CargoWeight { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TripStatus Status { get; set; } = TripStatus.Scheduled;

    public virtual Vehicle? Vehicle { get; set; }
    public virtual Driver? Driver { get; set; }
    public virtual ICollection<TripExpense> Expenses { get; set; } = new List<TripExpense>();

    public void StartTrip()
    {
        if (Status != TripStatus.Scheduled)
            throw new InvalidOperationException("Trip must be scheduled to start");
        Status = TripStatus.Started;
        StartDate = DateTime.UtcNow;
    }

    public void CompleteTrip()
    {
        if (Status != TripStatus.Started)
            throw new InvalidOperationException("Trip must be started to complete");
        Status = TripStatus.Completed;
        EndDate = DateTime.UtcNow;
    }

    public void CancelTrip()
    {
        if (Status == TripStatus.Completed)
            throw new InvalidOperationException("Cannot cancel completed trip");
        Status = TripStatus.Cancelled;
    }
}
