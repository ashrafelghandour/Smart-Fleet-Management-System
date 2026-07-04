using FleetManagementSystem.Application.DTOs.Trip;
using MediatR;

namespace FleetManagementSystem.Application.Queries.Trip.GetAll;

public sealed record  GetAllTripsQuery : IRequest<IEnumerable<TripResponse>>;
