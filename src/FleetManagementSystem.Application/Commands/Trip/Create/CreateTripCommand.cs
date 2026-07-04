using FleetManagementSystem.Application.DTOs.Trip;
using FleetManagementSystem.Domain.Enums;
using MediatR;

namespace FleetManagementSystem.Application.Commands.Trip.Create;

public record CreateTripCommand(
 int DriverId,
 int VehicleId,
 string StartLocation,
 string EndLocation,
 double Distance,
 double CargoWeight
) 
: IRequest<TripResponse>;
