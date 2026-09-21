using AutoMapper;
using FleetManagementSystem.Application.DTOs.Driver;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.Domain.Enums;
using MediatR;

namespace FleetManagementSystem.Application.Queries.Driver.GetAvailable;

public class GetAvailableDriversQueryHandler : IRequestHandler<GetAvailableDriversQuery, IEnumerable<DriverResponse>>
{
    private readonly IGenericRepository<Domain.Entities.Driver> _driverRepository;
    private readonly IMapper _mapper;

    public GetAvailableDriversQueryHandler(
        IGenericRepository<Domain.Entities.Driver> driverRepository,
        IMapper mapper)
    {
        _driverRepository = driverRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DriverResponse>> Handle(GetAvailableDriversQuery request, CancellationToken cancellationToken)
    {
        var drivers = await _driverRepository.FindAsync(d => d.Status == DriverStatus.Available);
        return _mapper.Map<IEnumerable<DriverResponse>>(drivers);
    }
}

