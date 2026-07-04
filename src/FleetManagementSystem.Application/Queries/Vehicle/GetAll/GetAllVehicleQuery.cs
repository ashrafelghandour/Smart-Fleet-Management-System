using MediatR;
using SFMSystem.Application.DTOs.Vehicle;

namespace FleetManagementSystem.Application.Queries.Vehicle.GetAll;


public class GetAllVehiclesQuery : IRequest<IEnumerable<VehicleResponse>>
{
}
