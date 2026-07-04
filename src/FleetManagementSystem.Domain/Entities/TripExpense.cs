using FleetManagementSystem.Domain.Common;
using FleetManagementSystem.Domain.Enums;

namespace FleetManagementSystem.Domain.Entities;

public class TripExpense : BaseEntity
{
    public int TripId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public ExpenseType ExpenseType { get; set; }

    public virtual Trip? Trip { get; set; }
}