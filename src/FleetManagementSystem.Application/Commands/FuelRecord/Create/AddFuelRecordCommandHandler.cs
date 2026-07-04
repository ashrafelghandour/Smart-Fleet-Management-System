using AutoMapper;
using FleetManagementSystem.Application.DTOs.FuelRecord;
using FleetManagementSystem.Application.Interface;
using MediatR;

namespace FleetManagementSystem.Application.Commands.FuelRecord.Create;

public class AddFuelRecordCommandHandler : IRequestHandler<AddFuelRecordCommand, FuelRecordResponse>
{
    private readonly IGenericRepository<Domain.Entities.FuelRecord> _fuelRecordRepository;
    private readonly IGenericRepository<Domain.Entities.Vehicle> _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AddFuelRecordCommandHandler(
        IGenericRepository<Domain.Entities.FuelRecord> fuelRecordRepository,
        IGenericRepository<Domain.Entities.Vehicle> vehicleRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _fuelRecordRepository = fuelRecordRepository;
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<FuelRecordResponse> Handle(AddFuelRecordCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
        if (vehicle == null)
            throw new Exception("Vehicle not found");

        var fuelRecord = new Domain.Entities.FuelRecord
        {
            VehicleId = request.VehicleId,
            Amount = request.Amount,
            Cost = request.Cost,
            Mileage = request.Mileage
        };

        await _fuelRecordRepository.AddAsync(fuelRecord);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<FuelRecordResponse>(fuelRecord);
    }
}