
using MediatR;
using FleetManagementSystem.Application.DTOs.Trip;
using FleetManagementSystem.Domain.Enums;

namespace FleetManagementSystem.Application.Commands.Trip.AddExpense;

public class AddTripExpenseCommand : IRequest<TripResponse>
{
    public int TripId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public ExpenseType ExpenseType { get; set; }
}

