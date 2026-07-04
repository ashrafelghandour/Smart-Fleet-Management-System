
using System.ComponentModel.DataAnnotations;
using FleetManagementSystem.Application.DTOs.Driver;
using FleetManagementSystem.Domain.Enums;
using FluentValidation;
using MediatR;

namespace FleetManagementSystem.Application.Commands.Driver.Create;

public sealed record CreateDriverCommand(
    [Required]
    int UserId , 
    [Required]
    string PhoneNumber,
    [Required]
    int NationalIde,
    [Required]
    string LicenseNumber,
    [Required]  
    DateTime LicenseIssueDate,
    [Required]
    DateTime LicenseExpiryDate,
     LicenseType LicenseType

    ): IRequest<DriverResponse>;
