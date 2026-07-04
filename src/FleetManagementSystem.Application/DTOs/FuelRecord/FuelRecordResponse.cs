using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FleetManagementSystem.Application.DTOs.FuelRecord;
public class FuelRecordResponse {
    public int VehicleId { get; set; }
    public double Amount { get; set; }
    public decimal Cost { get; set; }
    public double Mileage { get; set; }
}
