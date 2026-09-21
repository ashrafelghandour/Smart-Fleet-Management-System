using AutoMapper;
using FleetManagementSystem.Application.DTOs.Trip;
using FleetManagementSystem.Application.Interface;
using MediatR;

namespace FleetManagementSystem.Application.Queries.Trip.GetByVehicle;

public sealed class GetTripsByVehicleQueryHandler(
      IGenericRepository<Domain.Entities.Trip> tripRepository,
      IMapper mapper ) : IRequestHandler<GetTripsByVehicleQuery,IEnumerable<TripResponse>>
{
 public async Task<IEnumerable<TripResponse>> Handle(GetTripsByVehicleQuery request, CancellationToken cancellationToken)
  {
    var trips  =await tripRepository.FindAsync(tr=> tr.VehicleId == request.vehicleId);

     if(trips is null)
     throw new Exception($"Not Found Any Trips With Vehicle id{request.vehicleId}");

        return mapper.Map<IEnumerable<TripResponse>>(trips);
  }
}
