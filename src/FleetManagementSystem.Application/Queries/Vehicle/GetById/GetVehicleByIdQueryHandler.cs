
using AutoMapper;
using FleetManagementSystem.Application.Interface;
using MediatR;
using SFMSystem.Application.DTOs.Vehicle;

namespace FleetManagementSystem.Application.Queries.Vehicle.GetById;

public class GetVehicleByIdQueryHandler : IRequestHandler<GetVehicleByIdQuery, VehicleResponse>
{
    private readonly IGenericRepository<Domain.Entities.Vehicle> _vehicleRepository;
    private readonly IMapper _mapper;

    public GetVehicleByIdQueryHandler(
        IGenericRepository<Domain.Entities.Vehicle> vehicleRepository,
        IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }

    public async Task<VehicleResponse> Handle(GetVehicleByIdQuery request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.Id);
        
        if (vehicle == null)
            throw new Exception($"Vehicle with ID {request.Id} not found");

        return _mapper.Map<VehicleResponse>(vehicle);
    }
}