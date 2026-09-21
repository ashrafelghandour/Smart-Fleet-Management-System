using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FleetManagementSystem.Application.DTOs.MaintenanceRecord;
using FleetManagementSystem.Application.Interface;
using MediatR;

namespace FleetManagementSystem.Application.Queries.MaintenanceRecord;

 public record GetMaintenanceRecordsByVehicleQuery(int VehicleId) : IRequest<IEnumerable<MaintenanceRecordResponse>>;

public class GetMaintenanceRecordsByVehicleQueryHandler(
    IGenericRepository<Domain.Entities.MaintenanceRecord> maintenanceRecordRepository,
    IMapper mapper
) : IRequestHandler<GetMaintenanceRecordsByVehicleQuery, IEnumerable<MaintenanceRecordResponse>>
{
 public async Task<IEnumerable<MaintenanceRecordResponse>> Handle(GetMaintenanceRecordsByVehicleQuery request, CancellationToken cancellationToken)
    {
        
        var records = await maintenanceRecordRepository.FindAsync(f => f.VehicleId == request.VehicleId);
        return mapper.Map<IEnumerable<MaintenanceRecordResponse>>(records);
    }
}