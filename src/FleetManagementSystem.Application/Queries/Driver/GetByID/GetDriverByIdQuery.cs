using MediatR;
using FleetManagementSystem.Application.DTOs.Driver;
using System.ComponentModel.DataAnnotations;

namespace FleetManagementSystem.Application.Queries.Driver.GetById;

public class GetDriverByIdQuery : IRequest<DriverResponse>
{ 
    public int Id { get; set; }
}

