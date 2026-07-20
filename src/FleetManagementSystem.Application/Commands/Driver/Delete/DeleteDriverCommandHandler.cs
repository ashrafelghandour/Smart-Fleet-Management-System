using AutoMapper;
using FleetManagementSystem.Application.DTOs.Driver;
using FleetManagementSystem.Application.Interface;
using MediatR;

namespace FleetManagementSystem.Application.Commands.Driver.Delete;

public class DeleteDriverCommandHandler(
    IGenericRepository<Domain.Entities.Driver> driverRepo
) : IRequestHandler<DeleteDriverCommand,bool>
{
    public async Task<bool> Handle(DeleteDriverCommand request, CancellationToken cancellationToken)
    {

     
       var driver = await driverRepo.GetByIdAsync(request.driverId);
       if(driver is null)
       throw new FileNotFoundException($"Not Found User With id {request.driverId}");
     
         driverRepo.Delete(driver);

         return true;
    }
}