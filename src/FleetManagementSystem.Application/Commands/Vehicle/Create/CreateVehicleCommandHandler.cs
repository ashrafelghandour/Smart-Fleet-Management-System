using AutoMapper;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.Domain.Entities;
using FleetManagementSystem.Domain.Enums;
using MediatR;
using SFMSystem.Application.DTOs.Vehicle;

namespace FleetManagementSystem.Application.Commands.Vehicle.Create;

public class CreateVehicleCommandHandler(
    IGenericRepository<Domain.Entities.Vehicle> vehicleRepo,
    IGenericRepository<VehicleLicense> vehicleLicenseRepo,
    IUnitOfWork unitOfWork,
    IMapper mapper

) : IRequestHandler<CreateVehicleCommand, VehicleResponse>
{
 public async Task<VehicleResponse> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
    {
        if(vehicleRepo.ExistsAsync(v=>v.PlateNumber == request.PlateNumber).Result)
                    throw new Exception("Vehicle with this plate number already exists");

        var newVehicle = new Domain.Entities.Vehicle
        {
            PlateNumber = request.PlateNumber,
            CurrentMileage =request.CurrentMileage,
             Model = request.Model,
            Year = request.Year,
            Status = VehicleStatus.Available
        };

        var newVehilceLicense  = new VehicleLicense
        {
            VehicleId = newVehicle.Id,
             LicenseNumber = request.LicenseNumber,
            ExpirationDate = request.LicenseExpiryDate
        };

        await unitOfWork.BeginTransactionAsync();
        try
        {
            await Task.WhenAll(vehicleRepo.AddAsync(newVehicle),
            vehicleLicenseRepo.AddAsync(newVehilceLicense),
            unitOfWork.SaveChangesAsync(),
            unitOfWork.CommitTransactionAsync());

            return mapper.Map<VehicleResponse>(vehicleRepo);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}