
using AutoMapper;
using FleetManagementSystem.Application.Interface;
using MediatR;
using SFMSystem.Application.DTOs.Vehicle;

namespace FleetManagementSystem.Application.Queries.Vehicle.GetAll;

public class GetAllVehiclesQueryHandler(
    IGenericRepository<Domain.Entities.Vehicle> _vehicleRepository,
    IMapper _mapper) : IRequestHandler<GetAllVehiclesQuery, IEnumerable<VehicleResponse>>
{
    public async Task<IEnumerable<VehicleResponse>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _vehicleRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<VehicleResponse>>(vehicles);
    }
}