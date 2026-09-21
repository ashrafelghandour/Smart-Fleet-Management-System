using AutoMapper;
using FleetManagementSystem.Application.Interface;
using MediatR;
using SFMSystem.Application.DTOs.Vehicle;

namespace FleetManagementSystem.Application.Commands.Vehicle;

public class UpdateVehicleStatusCommandHandler : IRequestHandler<UpdateVehicleStatusCommand, VehicleResponse>
{
    private readonly IGenericRepository<Domain.Entities.Vehicle> _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateVehicleStatusCommandHandler(
        IGenericRepository<Domain.Entities.Vehicle> vehicleRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<VehicleResponse> Handle(UpdateVehicleStatusCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
        if (vehicle == null)
            throw new Exception("Vehicle not found");

        vehicle.Status = request.NewStatus;
        _vehicleRepository.Update(vehicle);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<VehicleResponse>(vehicle);
    }
}