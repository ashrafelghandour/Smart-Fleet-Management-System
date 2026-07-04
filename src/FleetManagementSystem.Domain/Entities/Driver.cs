using FleetManagementSystem.Domain.Common;
using FleetManagementSystem.Domain.Enums;

namespace FleetManagementSystem.Domain.Entities;

public class Driver : BaseEntity
{
    public int UserId { get; set; }  
    public int DriverLicenseId {get;set;}
    public string PhoneNumber { get; set; } = string.Empty;
    public int NationalId { get; set; }
    public DriverStatus Status { get; set; } = DriverStatus.Available;
    public DateTime HireDate { get; set; } = DateTime.UtcNow;

    public virtual AppUser? User { get; set; }
    public virtual DriverLicense? DriverLicense { get; set; }
    public virtual ICollection<Trip> Trips { get; set; } = new List<Trip>();

    public bool IsAvailable() => Status == DriverStatus.Available;
    
    public void StartTrip()
    {
        if (!IsAvailable())
            throw new InvalidOperationException("Driver is not available");
        Status = DriverStatus.InTrip;
    }
    
    public void EndTrip()
    {
        Status = DriverStatus.Available;
    }
}
