using AutoMapper;
using FleetManagementSystem.Application.DTOs.Trip;
using FleetManagementSystem.Application.Interface;
using MediatR;

namespace FleetManagementSystem.Application.Queries.Trip.GetById;

public sealed class GetTripByIdQueryHandler(
    
        IGenericRepository<Domain.Entities.Trip> tripRepository,
        IMapper mapper
) : IRequestHandler<GetTripByIdQuery, TripResponse>
{
 public async Task<TripResponse> Handle(GetTripByIdQuery request, CancellationToken cancellationToken)
    {
        var trip = await tripRepository.GetByIdAsync(request.id);
        return mapper.Map<TripResponse>(trip);
    }
}