using MediatR;
using AutoMapper;
using FleetManagementSystem.Application.DTOs.Driver;
using FleetManagementSystem.Application.Interface;

namespace FleetManagementSystem.Application.Queries.Driver.GetById;
public class GetDriverByIdQueryHandler : IRequestHandler<GetDriverByIdQuery, DriverResponse>
{
    private readonly IGenericRepository<Domain.Entities.Driver> _driverRepository;
    private readonly IMapper _mapper;

    public GetDriverByIdQueryHandler(
        IGenericRepository<Domain.Entities.Driver> driverRepository,
        IMapper mapper)
    {
        _driverRepository = driverRepository;
        _mapper = mapper;
    }

    public async Task<DriverResponse> Handle(GetDriverByIdQuery request, CancellationToken cancellationToken)
    {
        var driver = await _driverRepository.GetByIdAsync(request.Id);
        
        if (driver == null)
            throw new Exception($"Driver with ID {request.Id} not found");

        return _mapper.Map<DriverResponse>(driver);
    }
    
    }