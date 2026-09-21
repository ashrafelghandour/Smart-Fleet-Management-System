
using FleetManagementSystem.Domain.Common;

namespace FleetManagementSystem.Domain.Entities;

public class FuelRecord : BaseEntity
{
    public int VehicleId { get; set; }
    public double Amount { get; set; }  
    public decimal Cost { get; set; }
    public double Mileage { get; set; }  

    public virtual Vehicle? Vehicle { get; set; }
}