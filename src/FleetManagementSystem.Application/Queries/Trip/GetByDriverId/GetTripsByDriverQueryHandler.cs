using System.Data;
using AutoMapper;
using FleetManagementSystem.Application.DTOs.Trip;
using FleetManagementSystem.Application.Interface;
using MediatR;

namespace FleetManagementSystem.Application.Queries.Trip.GetByDriver;

public sealed class GetTripsByDriverQueryHandler(
      IGenericRepository<Domain.Entities.Trip> tripRepository,
      IMapper mapper ) : IRequestHandler<GetTripsByDriverQuery,IEnumerable<TripResponse>>
{
 public async Task<IEnumerable<TripResponse>> Handle(GetTripsByDriverQuery request, CancellationToken cancellationToken)
  {
    var trips  =await tripRepository.FindAsync(tr=> tr.DriverId == request.driverId);

     if(trips is null)
     throw new Exception($"Not Found Any Trips With Driver id{request.driverId}");

        return mapper.Map<IEnumerable<TripResponse>>(trips);
  }
}