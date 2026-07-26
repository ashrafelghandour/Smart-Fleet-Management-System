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
             // 1. التحقق من وجود المستخدم
        var user = await userRepo.FindByIdAsync(request.UserId.ToString());
        if (user == null)
            throw new Exception("User not found");

        // 2. التحقق من وجود السائق برقم الهاتف فقط (أفضل)
        var existingDriver = await driverRepo.ExistsAsync(d => d.PhoneNumber == request.PhoneNumber);
        if (existingDriver)
            throw new Exception("Driver with this phone number already exists");

        // 3. التحقق من وجود رخصة بنفس الرقم (اختياري)
        var existingLicense = await driverLicenseRepo.ExistsAsync(l => l.LicenseNumber == request.LicenseNumber);
        if (existingLicense)
            throw new Exception("License number already exists");

        
        var driver = new Domain.Entities.Driver
        {
            UserId = request.UserId,
            PhoneNumber = request.PhoneNumber,
            NationalId = request.NationalIde,
            Status = DriverStatus.Available,
            HireDate = DateTime.UtcNow
        };

   
        var license = new DriverLicense
        {
            
            LicenseNumber = request.LicenseNumber,
            IssueDate = request.LicenseIssueDate,
            ExpiryDate = request.LicenseExpiryDate,
            LicenseType = request.LicenseType,
            Driver = driver 
        };


        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            driver = await driverRepo.AddAsync(driver);
            
            license.DriverId = driver.Id; 
            
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
    
 