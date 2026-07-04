using FleetManagementSystem.Domain.Common;

namespace FleetManagementSystem.Domain.Entities;

public class VehicleLicense : BaseEntity
{
    public string LicenseNumber { get; set; } = string.Empty;
    public DateTime ExpirationDate { get; set; }
    public int VehicleId { get; set; }
    public virtual Vehicle Vehicle { get; set; } = null!;

    public bool IsExpired() =>  DateTime.UtcNow > ExpirationDate;
}
