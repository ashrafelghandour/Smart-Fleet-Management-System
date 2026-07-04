
using MediatR;
using SFMSystem.Application.DTOs.Vehicle;

namespace FleetManagementSystem.Application.Queries.Vehicle.GetAvailable;

public record GetAvailableVehiclesQuery : IRequest<IEnumerable<VehicleResponse>>;
