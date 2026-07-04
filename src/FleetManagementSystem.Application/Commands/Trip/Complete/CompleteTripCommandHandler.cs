using AutoMapper;
using FleetManagementSystem.Application.DTOs.Trip;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.Domain.Entities;
using FleetManagementSystem.Domain.Enums;
using MediatR;
namespace SFMSystem.Application.Commands.Trip.Complete;

public class CompleteTripCommandHandler(
       IGenericRepository<FleetManagementSystem.Domain.Entities.Trip> _tripRepository,
        IGenericRepository<Driver> _driverRepository,
        IGenericRepository<Vehicle> _vehicleRepository,
        IUnitOfWork _unitOfWork,
        IMapper _mapper
) : IRequestHandler<CompleteTripCommand, TripResponse>
{
    
    public async Task<TripResponse> Handle(CompleteTripCommand request, CancellationToken cancellationToken)
    {
        var trip = await _tripRepository.GetByIdAsync(request.id);
        if (trip == null)
            throw new Exception("Trip not found");

        if (trip.Status != TripStatus.Started)
            throw new Exception("Trip must be started to complete");

        var driver = await _driverRepository.GetByIdAsync(trip.DriverId);
        var vehicle = await _vehicleRepository.GetByIdAsync(trip.VehicleId);

        if (driver == null || vehicle == null)
            throw new Exception("Driver or Vehicle not found");

        trip.CompleteTrip();
        vehicle.UpdateMileage(trip.Distance);
        vehicle.MarkAsAvailable();
        driver.Status = DriverStatus.Available;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _tripRepository?.Update(trip);
            _vehicleRepository?.Update(vehicle);
            _driverRepository?.Update(driver);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return _mapper.Map<TripResponse>(trip);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}