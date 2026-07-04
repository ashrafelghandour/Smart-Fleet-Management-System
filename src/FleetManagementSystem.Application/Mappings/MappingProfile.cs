using AutoMapper;
using FleetManagementSystem.Application.DTOs.Driver;
using FleetManagementSystem.Application.DTOs.Trip;
using FleetManagementSystem.Domain.Entities;
using SFMSystem.Application.DTOs.Vehicle;

namespace FleetManagementSystem.Application.Mappings;

 public class MappingProfile : Profile
{
      public MappingProfile()
    {
        
        CreateMap<Driver, DriverResponse>()
            .ForMember(dest => dest.UserName, 
                opt => opt.MapFrom(src => src.User != null ? src.User.UserName : string.Empty))
            .ForMember(dest => dest.Email, 
                opt => opt.MapFrom(src => src.User != null ? src.User.Email : string.Empty))
            .ForMember(dest => dest.HasValidLicense, 
                opt => opt.MapFrom(src => src.DriverLicense != null && src.DriverLicense.IsValid()));

        CreateMap<Vehicle, VehicleResponse>()
            .ForMember(dest => dest.HasValidLicense, 
                opt => opt.MapFrom(src => src.VehicleLicense != null && src.VehicleLicense.ExpirationDate <= DateTime.Now));

        CreateMap<Trip, TripResponse>()
            .ForMember(dest => dest.DriverName, 
                opt => opt.MapFrom(src => src.Driver != null && src.Driver.User != null 
                    ? src.Driver.User.UserName : string.Empty))
            .ForMember(dest => dest.VehiclePlateNumber, 
                opt => opt.MapFrom(src => src.Vehicle != null ? src.Vehicle.PlateNumber : string.Empty))
            .ForMember(dest => dest.Status, 
                opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
