using FleetManagementSystem.Application.DTOs.MaintenanceRecord;
using MediatR;

namespace FleetManagementSystem.Application.Commands.MaintenanceRecord.Update;


public class CompleteMaintenanceCommand : IRequest<MaintenanceRecordResponse>
{
    public int MaintenanceId { get; set; }
}
