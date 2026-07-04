using AutoMapper;
using FleetManagementSystem.Application.DTOs.Trip;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.Domain.Entities;
using FleetManagementSystem.Domain.Enums;
using FluentValidation;
using MediatR;

namespace FleetManagementSystem.Application.Commands.Trip.Cancel;

public sealed record CancelTripCommand(int id ) : IRequest<TripResponse>;

public sealed class CancelTripCommandValidator : AbstractValidator<CancelTripCommand>
{
    public CancelTripCommandValidator()
    {
        RuleFor(ex=> ex.id)
        .GreaterThan(0);
    }
}



public class CancelTripCommandHandler(IGenericRepository<Domain.Entities.Trip> _tripRepository,
    IGenericRepository<Domain.Entities.Driver> _driverRepository,
    IGenericRepository<Vehicle> _vehicleRepository,
    IUnitOfWork _unitOfWork,
    IMapper _mapper) : IRequestHandler<CancelTripCommand, TripResponse>
{
   

    public async Task<TripResponse> Handle(CancelTripCommand request, CancellationToken cancellationToken)
    {
        var trip = await _tripRepository.GetByIdAsync(request.id);
        if (trip == null)
            throw new Exception("Trip not found");

        if (trip.Status == TripStatus.Completed)
            throw new Exception("Cannot cancel completed trip");

        var driver = await _driverRepository.GetByIdAsync(trip.DriverId);
        var vehicle = await _vehicleRepository.GetByIdAsync(trip.VehicleId);

        // Update
        trip.CancelTrip();
        
        if (driver != null && driver.Status == DriverStatus.InTrip)
            driver.Status = DriverStatus.Available;
        
        if (vehicle != null && vehicle.Status == VehicleStatus.InTrip)
            vehicle.MarkAsAvailable();

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            _tripRepository.Update(trip);
            if (driver != null) _driverRepository.Update(driver);
            if (vehicle != null) _vehicleRepository.Update(vehicle);

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