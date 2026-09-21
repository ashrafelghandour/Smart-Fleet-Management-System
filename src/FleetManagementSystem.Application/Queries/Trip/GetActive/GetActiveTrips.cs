using FleetManagementSystem.Application.DTOs.Trip;
using MediatR;

namespace FleetManagementSystem.Application.Queries.Trip.GetActive;
public sealed record GetActiveTripsQuery:IRequest<IEnumerable<TripResponse>>;
