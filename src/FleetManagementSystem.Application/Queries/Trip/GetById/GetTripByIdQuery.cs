using System.ComponentModel.DataAnnotations;
using FleetManagementSystem.Application.DTOs.Trip;
using MediatR;

namespace FleetManagementSystem.Application.Queries.Trip.GetById;
public sealed record GetTripByIdQuery([Required] int id) :IRequest<TripResponse>;
