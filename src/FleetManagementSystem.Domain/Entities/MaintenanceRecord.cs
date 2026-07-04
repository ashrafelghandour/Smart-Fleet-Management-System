using FleetManagementSystem.Domain.Common;
using FleetManagementSystem.Domain.Enums;

namespace FleetManagementSystem.Domain.Entities;

public class MaintenanceRecord : BaseEntity
{
    public int VehicleId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public MaintenanceStatus Status { get; set; } = MaintenanceStatus.Scheduled;
    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }

    public virtual Vehicle? Vehicle { get; set; }
}