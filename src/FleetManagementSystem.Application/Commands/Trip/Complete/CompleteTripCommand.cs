using System.ComponentModel.DataAnnotations;
using FleetManagementSystem.Application.DTOs.Trip;
using MediatR;
namespace SFMSystem.Application.Commands.Trip.Complete;

public sealed record  class CompleteTripCommand([Required]int id ) : IRequest<TripResponse>;
