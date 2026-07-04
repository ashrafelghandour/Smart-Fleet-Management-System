
using AutoMapper;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.Domain.Enums;
using MediatR;
using SFMSystem.Application.DTOs.Vehicle;

namespace FleetManagementSystem.Application.Queries.Vehicle.GetAvailable;

public class GetAvailableVehiclesQueryHandler : IRequestHandler<GetAvailableVehiclesQuery, IEnumerable<VehicleResponse>>
{
    private readonly IGenericRepository<Domain.Entities.Vehicle> _vehicleRepository;
    private readonly IMapper _mapper;

    public GetAvailableVehiclesQueryHandler(
        IGenericRepository<Domain.Entities.Vehicle> vehicleRepository,
        IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<VehicleResponse>> Handle(GetAvailableVehiclesQuery request, CancellationToken cancellationToken)
    {
        var vehicles = await _vehicleRepository.FindAsync(v => v.Status == VehicleStatus.Available);
        return _mapper.Map<IEnumerable<VehicleResponse>>(vehicles);
    }
}
