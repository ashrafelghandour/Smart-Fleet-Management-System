using MediatR;
using SFMSystem.Application.DTOs.Vehicle;

namespace FleetManagementSystem.Application.Queries.Vehicle.GetById;



public class GetVehicleByIdQuery : IRequest<VehicleResponse>
{
    public int Id { get; set; }
}
