using FleetManagementSystem.Application.DTOs.Trip;
using MediatR;

namespace FleetManagementSystem.Application.Queries.Trip.GetByVehicle;
 public sealed record  GetTripsByVehicleQuery(int vehicleId):IRequest<IEnumerable<TripResponse>>;
