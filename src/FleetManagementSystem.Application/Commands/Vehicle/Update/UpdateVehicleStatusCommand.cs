using FleetManagementSystem.Domain.Enums;
using MediatR;
using SFMSystem.Application.DTOs.Vehicle;

namespace FleetManagementSystem.Application.Commands.Vehicle;

public class UpdateVehicleStatusCommand : IRequest<VehicleResponse>
{
    public int VehicleId { get; set; }
    public VehicleStatus NewStatus { get; set; }
}
