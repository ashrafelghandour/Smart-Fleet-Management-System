
namespace FleetManagementSystem.Application.DTOs.MaintenanceRecord;


public class MaintenanceRecordResponse 
{
    public int VehicleId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public string Status { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }

}