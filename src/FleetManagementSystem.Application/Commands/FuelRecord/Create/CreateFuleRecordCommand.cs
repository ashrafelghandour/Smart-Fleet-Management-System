using FleetManagementSystem.Application.DTOs.FuelRecord;
using MediatR;

namespace FleetManagementSystem.Application.Commands.FuelRecord.Create;

public class AddFuelRecordCommand : IRequest<FuelRecordResponse>
{
    public int VehicleId { get; set; }
    public double Amount { get; set; }
    public decimal Cost { get; set; }
    public double Mileage { get; set; }
}
