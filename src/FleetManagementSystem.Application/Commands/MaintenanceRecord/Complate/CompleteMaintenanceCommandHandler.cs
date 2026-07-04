
using AutoMapper;
using FleetManagementSystem.Application.DTOs.MaintenanceRecord;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.Domain.Enums;
using MediatR;

namespace FleetManagementSystem.Application.Commands.MaintenanceRecord.Update;

public class CompleteMaintenanceCommandHandler(
        IGenericRepository<Domain.Entities.MaintenanceRecord> maintenanceRepository,
        IGenericRepository<Domain.Entities.Vehicle> vehicleRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper) : IRequestHandler<CompleteMaintenanceCommand, MaintenanceRecordResponse>
{
    public async Task<MaintenanceRecordResponse> Handle(CompleteMaintenanceCommand request, CancellationToken cancellationToken)
    {
        var maintenance = await maintenanceRepository.GetByIdAsync(request.MaintenanceId);
        if (maintenance == null)
            throw new Exception("Maintenance record not found");

        maintenance.CompletedDate = DateTime.UtcNow;
        maintenance.Status = MaintenanceStatus.Completed;
        maintenance.UpdatedAt = DateTime.UtcNow;

        var vehicle = await vehicleRepository.GetByIdAsync(maintenance.VehicleId);
        if (vehicle != null && vehicle.Status == VehicleStatus.UnderMaintenance)
        {
            vehicle.MarkAsAvailable();
        }

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            maintenanceRepository.Update(maintenance);
            if (vehicle != null)  vehicleRepository.Update(vehicle);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return mapper.Map<MaintenanceRecordResponse>(maintenance);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}