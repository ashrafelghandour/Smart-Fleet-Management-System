using AutoMapper;
using FleetManagementSystem.Application.DTOs.Trip;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.Application.Queries.Trip.GetAll;
using FluentAssertions;
using Moq;
namespace FMS.Test.Queries.Trip;

public class GetAllTripsQueryHandlerTest
{
     private readonly Mock<IGenericRepository<FleetManagementSystem.Domain.Entities.Trip>> _mockTripRepository;
     private readonly Mock<IMapper> _mockMapper;


     public GetAllTripsQueryHandlerTest()
     {
            _mockTripRepository = new Mock<IGenericRepository<FleetManagementSystem.Domain.Entities.Trip>>();
            _mockMapper = new Mock<IMapper>();
     }

     [Fact]
     public async Task Handle_ShouldReturnMappedTripResponses()
    {


        // arrange
        
        var command = new GetAllTripsQuery();

        var trips = new List<FleetManagementSystem.Domain.Entities.Trip>()
        {
            new FleetManagementSystem.Domain.Entities.Trip
            {
                Id = 1,
                DriverId = 1,
                StartLocation = "Location A",
                EndLocation = "Location B",
                StartDate = new DateTime(2024, 1, 1, 8, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2024, 1, 1, 9, 0, 0, DateTimeKind.Utc),
                Distance = 100.5,
                CargoWeight = 500.0,
            },
            new FleetManagementSystem.Domain.Entities.Trip
            {
                Id = 2,
                DriverId = 2,
                StartLocation = "Location C",
                EndLocation = "Location D",
                StartDate = new DateTime(2024, 1, 2, 8, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(2024, 1, 2, 10, 0, 0, DateTimeKind.Utc),
                Distance = 200.5,
                CargoWeight = 1000.0,
            }
        };

        var mappedTrips = new List<TripResponse>
        {
            new() { Id = 1, DriverId = 1, StartLocation = "Location A", EndLocation = "Location B" },
            new() { Id = 2, DriverId = 2, StartLocation = "Location C", EndLocation = "Location D" }
        };

        _mockTripRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(trips);
        _mockMapper.Setup(mapper => mapper.Map<IEnumerable<TripResponse>>(trips)).Returns(mappedTrips);

        // act
        var handler = new GetAllTripsQueryHandler(_mockTripRepository.Object, _mockMapper.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        // assert
        _mockTripRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        _mockMapper.Verify(mapper => mapper.Map<IEnumerable<TripResponse>>(trips), Times.Once);
        result.Should().BeEquivalentTo(mappedTrips);
        result.First().Id.Should().Be(1);
        
        result.First().StartLocation.Should().Be("Location A");
    }
}