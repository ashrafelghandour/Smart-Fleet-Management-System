using AutoMapper;
using FleetManagementSystem.Application.DTOs.Driver;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.Domain.Entities;
using FleetManagementSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace FleetManagementSystem.Application.Commands.Driver.Create;

public class CreateDriverCommandHandler(
    IGenericRepository<Domain.Entities.Driver> driverRepo,
    IGenericRepository<DriverLicense> driverLicenseRepo,
   UserManager<AppUser> userRepo,
    IUnitOfWork unitOfWork,
    
    IMapper mapper) :
     IRequestHandler<CreateDriverCommand, DriverResponse>
{

    public async Task<DriverResponse> Handle(CreateDriverCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepo.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            throw new Exception("User not found");

        var existingDriver = await driverRepo.ExistsAsync(d => d.Id == request.UserId);
        if (existingDriver)
            throw new Exception("Driver already exists for this user");

        //  Create new Driver
        var driver = new Domain.Entities.Driver
        {
            UserId = request.UserId,
            PhoneNumber = request.PhoneNumber,
            NationalId = request.NationalIde,
            Status = DriverStatus.Available,
            HireDate = DateTime.UtcNow
        };

        //  Driver License
        var license = new DriverLicense
        {
            DriverId = driver.Id,
            LicenseNumber = request.LicenseNumber,
            IssueDate = request.LicenseIssueDate,
            ExpiryDate = request.LicenseExpiryDate,
            LicenseType = request.LicenseType
        };

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
           //wait all Task Completed
           driver = await  driverRepo.AddAsync(driver);

           await driverLicenseRepo.AddAsync(license);
          
           await unitOfWork.SaveChangesAsync(cancellationToken);
           await unitOfWork.CommitTransactionAsync(cancellationToken);

            return mapper.Map<DriverResponse>(driver);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}