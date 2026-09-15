using FleetManagementSystem.Application.Mappings;
using FleetManagementSystem.Domain.Entities;
using FleetManagementSystem.Domain.Enums;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.UnitTests.Helpers;
using FleetManagementSystem.UnitTests.Mocks;
using Moq;
using FleetManagementSystem.Application;
using SFMSystem.Application.Commands.Trip.Complete;
using AutoMapper;
using FluentAssertions;

namespace SFMSystem.UnitTests.Commands.Trip;

public class CompleteTripCommandHandlerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IGenericRepository<FleetManagementSystem.Domain.Entities.Trip>> _mockTripRepo;
    private readonly Mock<IGenericRepository<Driver>> _mockDriverRepo;
    private readonly Mock<IGenericRepository<Vehicle>> _mockVehicleRepo;
    private readonly IMapper _mapper;
    private readonly CompleteTripCommandHandler _handler;

    public CompleteTripCommandHandlerTests()
    {
        // AutoMapper
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();

        // Repositories
        _mockTripRepo = new Mock<IGenericRepository<FleetManagementSystem.Domain.Entities.Trip>>();
        _mockDriverRepo = new Mock<IGenericRepository<Driver>>();
        _mockVehicleRepo = new Mock<IGenericRepository<Vehicle>>();

        // UnitOfWork
        _mockUnitOfWork = MockUnitOfWork.CreateMockUnitOfWork()
            .SetupTripRepository(_mockTripRepo)
            .SetupDriverRepository(_mockDriverRepo)
            .SetupVehicleRepository(_mockVehicleRepo);

        // Handler
        _handler = new CompleteTripCommandHandler(_mockTripRepo.Object,
           _mockDriverRepo.Object,
           _mockVehicleRepo.Object,
            _mockUnitOfWork.Object,
            _mapper
        );
    }

    // ========================================================================
    // 1. HAPPY PATH
    // ========================================================================

    [Fact]
    public async Task Handle_WithValidTripId_ShouldCompleteTripSuccessfully()
    {
        // Arrange
        var trip = TestDataFactory.CreateTripFaker().Generate();
        trip.Status = TripStatus.Started;
        trip.Distance = 220;

        var driver = TestDataFactory.CreateDriverFaker().Generate();
        driver.Status = DriverStatus.InTrip;

        var vehicle = TestDataFactory.CreateVehicleFaker().Generate();
        vehicle.Status = VehicleStatus.InTrip;
        vehicle.CurrentMileage = 1000;

        var command = new CompleteTripCommand(trip.Id );

        _mockTripRepo
            .Setup(r => r.GetByIdAsync(trip.Id))
            .ReturnsAsync(trip);

        _mockDriverRepo
            .Setup(r => r.GetByIdAsync(trip.DriverId))
            .ReturnsAsync(driver);

        _mockVehicleRepo
            .Setup(r => r.GetByIdAsync(trip.VehicleId))
            .ReturnsAsync(vehicle);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(trip.Id);
        result.Status.Should().Be("Completed");

        // Verify status updates
        driver.Status.Should().Be(DriverStatus.Available);
        vehicle.Status.Should().Be(VehicleStatus.Available);
        vehicle.CurrentMileage.Should().Be(1220); // 1000 + 220

        // Verify method calls
        _mockTripRepo.Verify(r => r.Update(It.IsAny<FleetManagementSystem.Domain.Entities.Trip>()), Times.Once);
        _mockDriverRepo.Verify(r => r.Update(It.IsAny<Driver>()), Times.Once);
        _mockVehicleRepo.Verify(r => r.Update(It.IsAny<Vehicle>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    // ========================================================================
    // 2. FAILURE TESTS
    // ========================================================================

    [Fact]
    public async Task Handle_WhenTripNotFound_ShouldThrowException()
    {
        // Arrange
        var command = new CompleteTripCommand(999);

        _mockTripRepo
            .Setup(r => r.GetByIdAsync(command.id))
            .ReturnsAsync((FleetManagementSystem.Domain.Entities.Trip?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            _handler.Handle(command, CancellationToken.None));
        
        
        exception.Message.Should().Be($"Trip not found");
    }

    [Fact]
    public async Task Handle_WhenTripNotStarted_ShouldThrowException()
    {
        // Arrange
        var trip = TestDataFactory.CreateTripFaker().Generate();
        trip.Status = TripStatus.Scheduled; // Not started

        var command = new CompleteTripCommand(trip.Id);

        _mockTripRepo
            .Setup(r => r.GetByIdAsync(trip.Id))
            .ReturnsAsync(trip);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Trip must be started to complete");
    }

    [Fact]
    public async Task Handle_WhenTripAlreadyCompleted_ShouldThrowException()
    {
        // Arrange
        var trip = TestDataFactory.CreateTripFaker().Generate();
        trip.Status = TripStatus.Completed; // Already completed

        var command = new CompleteTripCommand( trip.Id);

        _mockTripRepo
            .Setup(r => r.GetByIdAsync(trip.Id))
            .ReturnsAsync(trip);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Trip must be started to complete");
    }

    [Fact]
    public async Task Handle_WhenTripCancelled_ShouldThrowException()
    {
        // Arrange
        var trip = TestDataFactory.CreateTripFaker().Generate();
        trip.Status = TripStatus.Cancelled; // Cancelled

        var command = new CompleteTripCommand ( trip.Id);

        _mockTripRepo
            .Setup(r => r.GetByIdAsync(trip.Id))
            .ReturnsAsync(trip);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Trip must be started to complete");
    }

    [Fact]
    public async Task Handle_WhenDriverNotFound_ShouldThrowException()
    {
        // Arrange
        var trip = TestDataFactory.CreateTripFaker().Generate();
        trip.Status = TripStatus.Started;

        var command = new CompleteTripCommand(trip.Id);

        _mockTripRepo
            .Setup(r => r.GetByIdAsync(trip.Id))
            .ReturnsAsync(trip);

        _mockDriverRepo
            .Setup(r => r.GetByIdAsync(trip.DriverId))
            .ReturnsAsync((Driver?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Driver or Vehicle not found");
    }

    [Fact]
    public async Task Handle_WhenVehicleNotFound_ShouldThrowException()
    {
        // Arrange
        var trip = TestDataFactory.CreateTripFaker().Generate();
        trip.Status = TripStatus.Started;

        var driver = TestDataFactory.CreateDriverFaker().Generate();

        var command = new CompleteTripCommand( trip.Id);

        _mockTripRepo
            .Setup(r => r.GetByIdAsync(trip.Id))
            .ReturnsAsync(trip);

        _mockDriverRepo
            .Setup(r => r.GetByIdAsync(trip.DriverId))
            .ReturnsAsync(driver);

        _mockVehicleRepo
            .Setup(r => r.GetByIdAsync(trip.VehicleId))
            .ReturnsAsync((Vehicle?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Driver or Vehicle not found");
    }

    // ========================================================================
    // 3. EDGE CASES
    // ========================================================================

    [Fact]
    public async Task Handle_WithOneDistance_ShouldStillCompleteTrip()
    {
        // Arrange
        var trip = TestDataFactory.CreateTripFaker().Generate();
        trip.Status = TripStatus.Started;
        trip.Distance = 1; // One unit distance

        var driver = TestDataFactory.CreateDriverFaker().Generate();
        driver.Status = DriverStatus.InTrip;

        var vehicle = TestDataFactory.CreateVehicleFaker().Generate();
        vehicle.Status = VehicleStatus.InTrip;
        vehicle.CurrentMileage = 1000;

        var command = new CompleteTripCommand( trip.Id);

        _mockTripRepo
            .Setup(r => r.GetByIdAsync(trip.Id))
            .ReturnsAsync(trip);

        _mockDriverRepo
            .Setup(r => r.GetByIdAsync(trip.DriverId))
            .ReturnsAsync(driver);

        _mockVehicleRepo
            .Setup(r => r.GetByIdAsync(trip.VehicleId))
            .ReturnsAsync(vehicle);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Status.Should().Be("Completed");
        vehicle.CurrentMileage.Should().Be(1001); // 1000 + 1
    }

    [Fact]
    public async Task Handle_WithLargeDistance_ShouldUpdateMileageCorrectly()
    {
        // Arrange
        var trip = TestDataFactory.CreateTripFaker().Generate();
        trip.Status = TripStatus.Started;
        trip.Distance = 9999; // Large distance

        var driver = TestDataFactory.CreateDriverFaker().Generate();
        driver.Status = DriverStatus.InTrip;

        var vehicle = TestDataFactory.CreateVehicleFaker().Generate();
        vehicle.Status = VehicleStatus.InTrip;
        vehicle.CurrentMileage = 1;

        var command = new CompleteTripCommand( trip.Id);

        _mockTripRepo
            .Setup(r => r.GetByIdAsync(trip.Id))
            .ReturnsAsync(trip);

        _mockDriverRepo
            .Setup(r => r.GetByIdAsync(trip.DriverId))
            .ReturnsAsync(driver);

        _mockVehicleRepo
            .Setup(r => r.GetByIdAsync(trip.VehicleId))
            .ReturnsAsync(vehicle);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        vehicle.CurrentMileage.Should().Be(10000); // 1 + 9999
    }

    // ========================================================================
    // 4. TRANSACTION TESTS
    // ========================================================================

    [Fact]
    public async Task Handle_WhenTransactionFails_ShouldRollbackAndThrowException()
    {
        // Arrange
        var trip = TestDataFactory.CreateTripFaker().Generate();
        trip.Status = TripStatus.Started;

        var driver = TestDataFactory.CreateDriverFaker().Generate();
        driver.Status = DriverStatus.InTrip;

        var vehicle = TestDataFactory.CreateVehicleFaker().Generate();
        vehicle.Status = VehicleStatus.InTrip;

        var command = new CompleteTripCommand( trip.Id);

        _mockTripRepo
            .Setup(r => r.GetByIdAsync(trip.Id))
            .ReturnsAsync(trip);

        _mockDriverRepo
            .Setup(r => r.GetByIdAsync(trip.DriverId))
            .ReturnsAsync(driver);

        _mockVehicleRepo
            .Setup(r => r.GetByIdAsync(trip.VehicleId))
            .ReturnsAsync(vehicle);

        _mockUnitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Database error");

        // Verify rollback was called
        _mockUnitOfWork.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}