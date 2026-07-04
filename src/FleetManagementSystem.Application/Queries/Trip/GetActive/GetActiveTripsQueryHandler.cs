using AutoMapper;
using FleetManagementSystem.Application.DTOs.Trip;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.Domain.Enums;
using MediatR;

namespace FleetManagementSystem.Application.Queries.Trip.GetActive;

public sealed class GetActiveTripsQueryHandler(
        IGenericRepository<Domain.Entities.Trip> tripRepository,
        IMapper mapper) : IRequestHandler<GetActiveTripsQuery, IEnumerable<TripResponse>>
{
   public async Task<IEnumerable<TripResponse>> Handle(GetActiveTripsQuery request, CancellationToken cancellationToken)
   {
           var trips = await tripRepository.FindAsync(t => 
            t.Status == TripStatus.Started || t.Status == TripStatus.Scheduled);
        
        return mapper.Map<IEnumerable<TripResponse>>(trips);
   }
}