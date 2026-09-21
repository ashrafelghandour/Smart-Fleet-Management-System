using AutoMapper;
using FleetManagementSystem.Application.DTOs.Driver;
using FleetManagementSystem.Application.Interface;
using MediatR;

namespace FleetManagementSystem.Application.Commands.Driver.UpdateStatus;

public sealed class UpdateDriverStatusCommandHandler(
    IGenericRepository<Domain.Entities.Driver> driverRepo,
    IUnitOfWork unitOf,
    IMapper mapper

) : IRequestHandler<UpdateDriverStatusCommand, DriverResponse>
{
 public async Task<DriverResponse> Handle(UpdateDriverStatusCommand request, CancellationToken cancellationToken)
    {
        
        var driver= await driverRepo.GetByIdAsync(request.DriverId);
        if(driver is null )
        throw new  Exception($"Driver With id {request.DriverId} Not Found");

        driver.Status = request.NewStatus;
        await driverRepo.Update(driver);
        await unitOf.SaveChangesAsync();

        return  mapper.Map<DriverResponse>(driver);
 
    }
}