using AutoMapper;
using FleetManagementSystem.Application.DTOs.Trip;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.Domain.Entities;
using FleetManagementSystem.Domain.Enums;
using FluentValidation;
using MediatR;

namespace FleetManagementSystem.Application.Commands.Trip.Create;  
public class CreateTripCommandHandler( IGenericRepository<Domain.Entities.Trip> _tripRepository,
        IGenericRepository<Domain.Entities.Driver> _driverRepository,
        IGenericRepository<Vehicle> _vehicleRepository,
        IUnitOfWork _unitOfWork,
        IMapper _mapper,
        IValidator<CreateTripCommand> _validator) : IRequestHandler<CreateTripCommand, TripResponse>
{
  
    public async Task<TripResponse> Handle(CreateTripCommand request, CancellationToken cancellationToken)
    {
        // 1. Validate
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        // Driver validation
        var driver = await _driverRepository.GetByIdAsync(request.DriverId);
        if (driver == null)
            throw new Exception("Driver not found");

        if (driver.Status != DriverStatus.Available)
            throw new Exception("Driver is not available");

        if (driver.DriverLicense == null || driver.DriverLicense.IsExpired())
            throw new Exception("Driver license is expired or missing");

        // 3. Validate Vehicle
        var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
        if (vehicle == null)
            throw new Exception("Vehicle not found");

        if (vehicle.Status != VehicleStatus.Available)
            throw new Exception("Vehicle is not available");

        if (vehicle.VehicleLicense == null || vehicle.VehicleLicense.IsExpired())
            throw new Exception("Vehicle license is expired or missing");

        var hasActiveTrip = await _tripRepository.ExistsAsync(t => 
            t.VehicleId == request.VehicleId && t.Status == TripStatus.Started);
        
        if (hasActiveTrip)
            throw new Exception("Vehicle already in active trip");

        // 4. Create Trip
        var trip = new Domain.Entities.Trip
        {
            DriverId = request.DriverId,
            VehicleId = request.VehicleId,
            StartLocation = request.StartLocation,
            EndLocation = request.EndLocation,
            Distance = request.Distance,
            CargoWeight = request.CargoWeight,
            Status = TripStatus.Started,
            StartDate = DateTime.UtcNow
        };

        // 5. Update Statuses
        driver.Status = DriverStatus.InTrip;
        vehicle.Status = VehicleStatus.InTrip;

        // 6. Save with Transaction
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            await _tripRepository.AddAsync(trip);
            _driverRepository?.Update(driver);
            _vehicleRepository?.Update(vehicle);
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            var createdTrip = await _tripRepository.GetByIdAsync(trip.Id);
            return _mapper.Map<TripResponse>(createdTrip);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
 }


}
