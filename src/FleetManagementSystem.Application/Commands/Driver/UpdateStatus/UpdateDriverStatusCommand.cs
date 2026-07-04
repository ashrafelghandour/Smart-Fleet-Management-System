using FleetManagementSystem.Application.DTOs.Driver;
using FleetManagementSystem.Domain.Enums;
using MediatR;

namespace FleetManagementSystem.Application.Commands.Driver.UpdateStatus;
public sealed record UpdateDriverStatusCommand : IRequest<DriverResponse>
{
    public int DriverId { get; set; }
    public DriverStatus NewStatus { get; set; }
}
