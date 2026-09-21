using FleetManagementSystem.Application.DTOs.Trip;
using MediatR;

namespace FleetManagementSystem.Application.Queries.Trip.GetByDriver;
  public sealed record GetTripsByDriverQuery(int driverId):IRequest<IEnumerable<TripResponse>>;
