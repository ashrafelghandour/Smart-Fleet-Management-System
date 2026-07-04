
using AutoMapper;
using FleetManagementSystem.Application.DTOs.MaintenanceRecord;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.Domain.Enums;
using MediatR;

namespace FleetManagementSystem.Application.Commands.MaintenanceRecord.Add;

public class AddMaintenanceRecordCommandHandler (
      IGenericRepository<Domain.Entities.MaintenanceRecord> maintenanceRepository,
        IGenericRepository<Domain.Entities.Vehicle> vehicleRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper): IRequestHandler<AddMaintenanceRecordCommand, MaintenanceRecordResponse>
{
    public async Task<MaintenanceRecordResponse> Handle(AddMaintenanceRecordCommand request, CancellationToken cancellationToken)
    {
        var vehicle = await vehicleRepository.GetByIdAsync(request.VehicleId);
        if (vehicle == null)
            throw new Exception("Vehicle not found");

        //  mark vehicle as UnderMaintenance
        if (vehicle.Status == VehicleStatus.Available)
        {
            vehicle.Status = VehicleStatus.UnderMaintenance;
        }

        var maintenanceRecord = new Domain.Entities.MaintenanceRecord
        {
            VehicleId = request.VehicleId,
            Type = request.Type,
            Description = request.Description,
            Cost = request.Cost,
            Status = MaintenanceStatus.Scheduled,
            ScheduledDate = request.ScheduledDate
        };

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await maintenanceRepository.AddAsync(maintenanceRecord);
            vehicleRepository.Update(vehicle);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return mapper.Map<MaintenanceRecordResponse>(maintenanceRecord);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
