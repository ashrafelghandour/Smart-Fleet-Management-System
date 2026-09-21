namespace FleetManagementSystem.Application.DTOs.Trip;


public class TripResponse
{
    public int Id { get; set; }
    public int DriverId { get; set; }
    public string DriverName { get; set; } = string.Empty;
    public int VehicleId { get; set; }
    public string VehiclePlateNumber { get; set; } = string.Empty;
    public string StartLocation { get; set; } = string.Empty;
    public string EndLocation { get; set; } = string.Empty;
    public double Distance { get; set; }
    public double CargoWeight { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}