using FleetManagementSystem.Domain.Common;
using FleetManagementSystem.Domain.Enums;

namespace FleetManagementSystem.Domain.Entities;

public class DriverLicense : BaseEntity
{
    public int DriverId { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string? LicenseImageUrl { get; set; }
    public LicenseType LicenseType { get; set; } = LicenseType.Private;

    public virtual Driver? Driver { get; set; }

    public bool IsExpired() => DateTime.UtcNow > ExpiryDate;
    public bool IsValid() => !IsExpired() && DateTime.UtcNow < ExpiryDate;
}
