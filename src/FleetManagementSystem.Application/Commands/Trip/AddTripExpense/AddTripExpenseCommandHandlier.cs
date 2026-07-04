using AutoMapper;
using FleetManagementSystem.Domain.Entities;
using FleetManagementSystem.Application.DTOs.Trip;
using MediatR;
using FleetManagementSystem.Application.Commands.Trip.AddExpense;
using FleetManagementSystem.Application.Interface;

namespace SFMSystem.Application.Commands.Trip.AddExpense;

public class AddTripExpenseCommandHandler(
    IGenericRepository<FleetManagementSystem.Domain.Entities.Trip> _tripRepository,
    IGenericRepository<TripExpense> _expenseRepository,
    IUnitOfWork _unitOfWork,
    IMapper _mapper
   ) : IRequestHandler<AddTripExpenseCommand, TripResponse>
{
    public async Task<TripResponse> Handle(AddTripExpenseCommand request, CancellationToken cancellationToken)
    {
        var trip = await _tripRepository.GetByIdAsync(request.TripId);
        if (trip == null)
            throw new Exception("Trip not found");

        var expense = new TripExpense
        {
            TripId = request.TripId,
            Amount = request.Amount,
            Description = request.Description,
            ExpenseType = request.ExpenseType
        };

        await _expenseRepository.AddAsync(expense);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TripResponse>(trip);
    }
}