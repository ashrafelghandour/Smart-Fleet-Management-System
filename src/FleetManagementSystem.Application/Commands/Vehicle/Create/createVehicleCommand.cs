
using System.ComponentModel.DataAnnotations;
using MediatR;
using SFMSystem.Application.DTOs.Vehicle;

namespace FleetManagementSystem.Application.Commands.Vehicle.Create;

public sealed record CreateVehicleCommand :IRequest<VehicleResponse> 
{
    [Required]
    public string PlateNumber { get; set; } = string.Empty;
    [Required]
    public string Model { get; set; } = string.Empty;
    [Required]
    public int Year { get; set; }
    [Required]
    public double CurrentMileage { get; set; }
    [Required]
    public string LicenseNumber { get; set; } = string.Empty;
    [Required]
    public DateTime LicenseIssueDate { get; set; }
    [Required]
    public DateTime LicenseExpiryDate { get; set; }
}
