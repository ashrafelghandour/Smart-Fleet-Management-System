namespace SFMSystem.Application.DTOs.Vehicle;
public class VehicleResponse{

    public int Id { get; set; }
    public string PlateNumber { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public double CurrentMileage { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool HasValidLicense { get; set; }
}
