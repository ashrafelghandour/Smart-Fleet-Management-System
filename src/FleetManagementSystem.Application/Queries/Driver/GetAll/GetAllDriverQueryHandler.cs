using AutoMapper;
using FleetManagementSystem.Application.DTOs.Driver;
using FleetManagementSystem.Application.Interface;
using MediatR;

namespace FleetManagementSystem.Application.Queries.Driver.GetAlls;

public sealed class GetAllDriverQueryHandler(
  IGenericRepository<Domain.Entities.Driver> driverRepo,
  IMapper mapper
) : IRequestHandler<GetAllDriverQuery, IEnumerable<DriverResponse>>
{
   public async Task<IEnumerable<DriverResponse>> Handle(GetAllDriverQuery request, CancellationToken cancellationToken)
 {
     var drivers  =await  driverRepo.GetAllAsync();
         return mapper.Map<IEnumerable<DriverResponse>>(drivers);
 }
}