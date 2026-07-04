using AutoMapper;
using FleetManagementSystem.Application.DTOs.Trip;
using FleetManagementSystem.Application.Interface;
using MediatR;

namespace FleetManagementSystem.Application.Queries.Trip.GetAll;

public sealed class GetAllTripsQueryHandler(
     IGenericRepository<Domain.Entities.Trip> _tripRepository,
     IMapper _mapper) : IRequestHandler<GetAllTripsQuery, IEnumerable<TripResponse>>
{
 public async Task<IEnumerable<TripResponse>> Handle(GetAllTripsQuery request, CancellationToken cancellationToken)
    {
    
        var trips = await _tripRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<TripResponse>>(trips);
    
    }
}