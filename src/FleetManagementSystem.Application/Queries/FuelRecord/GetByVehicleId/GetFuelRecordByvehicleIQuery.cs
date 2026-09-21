
using AutoMapper;
using FleetManagementSystem.Application.DTOs.FuelRecord;
using FleetManagementSystem.Application.Interface;
using MediatR;

namespace FleetManagementSystem.Application.Queries.FuelRecord.GetByVehicleId;


public record GetFuelRecordByvehicleIQuery(int VehicleId) : IRequest<IEnumerable<FuelRecordResponse>>;


public class GetFuelRecordsByVehicleQueryHandler(
IGenericRepository<Domain.Entities.FuelRecord> fuelRecordRepository,
 IMapper mapper) : IRequestHandler<GetFuelRecordByvehicleIQuery, IEnumerable<FuelRecordResponse>>
{
   

    public async Task<IEnumerable<FuelRecordResponse>> Handle(GetFuelRecordByvehicleIQuery request, CancellationToken cancellationToken)
    {
           var records = await fuelRecordRepository.FindAsync(f => f.VehicleId == request.VehicleId);
        return mapper.Map<IEnumerable<FuelRecordResponse>>(records);
    }
}