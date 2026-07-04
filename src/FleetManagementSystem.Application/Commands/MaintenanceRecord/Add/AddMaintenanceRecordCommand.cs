using FleetManagementSystem.Application.DTOs.MaintenanceRecord;
using MediatR;

namespace FleetManagementSystem.Application.Commands.MaintenanceRecord.Add;



public class AddMaintenanceRecordCommand : IRequest<MaintenanceRecordResponse>
{
    public int VehicleId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public DateTime ScheduledDate { get; set; }
}
