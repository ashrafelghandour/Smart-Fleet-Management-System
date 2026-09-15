using FluentValidation;
using FluentValidation.Results;
using FleetManagementSystem.Application.Commands.Trip.Create;
using FleetManagementSystem.Application.Mappings;
using FleetManagementSystem.Domain.Entities;
using FleetManagementSystem.Domain.Enums;
using FleetManagementSystem.UnitTests.Helpers;
using FleetManagementSystem.UnitTests.Mocks;
using Moq;
using FleetManagementSystem.Application.Interface;
using AutoMapper;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using FleetManagementSystem.Application.ValidationBehavior;
using FleetManagementSystem.Application.LoggingBehavior;
using System.Linq.Expressions;
using FluentAssertions.Extensions;

namespace FleetManagementSystem.UnitTests.Commands.Trip;

public class CreateTripCommandHandlerTests
{
    private IMediator _mediator;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IGenericRepository<Driver>> _mockDriverRepo;
    private readonly Mock<IGenericRepository<Vehicle>> _mockVehicleRepo;
    private readonly Mock<IGenericRepository<Domain.Entities.Trip>> _mockTripRepo;
    private readonly Mock<IValidator<CreateTripCommand>> _mockValidator;
    private readonly IMapper _mapper;
    private readonly CreateTripCommandHandler _handler;

    public CreateTripCommandHandlerTests()
    {
        // Arrange - AutoMapper
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
       
        // Arrange - Repositories
        _mockDriverRepo = new Mock<IGenericRepository<Driver>>();
        _mockVehicleRepo = new Mock<IGenericRepository<Vehicle>>();
        _mockTripRepo = new Mock<IGenericRepository<Domain.Entities.Trip>>();

        // Arrange - UnitOfWork

        _mockUnitOfWork = MockUnitOfWork.CreateMockUnitOfWork()
            .SetupDriverRepository(_mockDriverRepo)
            .SetupVehicleRepository(_mockVehicleRepo)
            .SetupTripRepository(_mockTripRepo);

        // Arrange - Validator (always valid by default)
        _mockValidator = new Mock<IValidator<CreateTripCommand>>();
        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateTripCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());



    var services = new ServiceCollection();

services.AddLogging();

services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<CreateTripCommandHandler>();
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

services.AddValidatorsFromAssemblyContaining<CreateTripCommandValidator>();

services.AddSingleton(_mockDriverRepo.Object);
services.AddSingleton(_mockVehicleRepo.Object);
services.AddSingleton(_mockTripRepo.Object);

var provider = services.BuildServiceProvider();

_mediator = provider.GetRequiredService<IMediator>();

        // Act - Create Handler
        _handler = new CreateTripCommandHandler(
            _mockTripRepo.Object,
            _mockDriverRepo.Object,
            _mockVehicleRepo.Object,
            _mockUnitOfWork.Object,
            _mapper
        );
    }

    // ========================================================================
    // 1. HAPPY PATH TESTS (السيناريو الناجح)
    // ========================================================================

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateTripSuccessfully()
    {
        // Arrange
        var command = TestDataFactory.CreateValidTripCommand();
        var driver = TestDataFactory.CreateAvailableDriver();
        var vehicle = TestDataFactory.CreateAvailableVehicle();
        var trip = new Domain.Entities.Trip
        {
            Id = 1,
            DriverId = command.DriverId,
            VehicleId = command.VehicleId,
            StartLocation = command.StartLocation,
            EndLocation = command.EndLocation,
            Distance = command.Distance,
            CargoWeight = command.CargoWeight,
            Status = TripStatus.Started,
            StartDate = DateTime.UtcNow
        };

        // Setup mocks
        _mockTripRepo
        .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
        .ReturnsAsync(trip);

        _mockDriverRepo
            .Setup(r => r.GetByIdAsync(command.DriverId))
            .ReturnsAsync(driver);

        _mockVehicleRepo
            .Setup(r => r.GetByIdAsync(command.VehicleId))
            .ReturnsAsync(vehicle);

        _mockTripRepo
            .Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.Trip, bool>>>()))
            .ReturnsAsync(false);

        _mockTripRepo
            .Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Trip>()))
            .ReturnsAsync(trip);
        
        _mockUnitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.DriverId.Should().Be(command.DriverId);
        result.VehicleId.Should().Be(command.VehicleId);
        result.StartLocation.Should().Be(command.StartLocation);
        result.EndLocation.Should().Be(command.EndLocation);
        result.Distance.Should().Be(command.Distance);
        result.CargoWeight.Should().Be(command.CargoWeight);
        result.Status.Should().Be("Started");

        // Verify method calls
        _mockDriverRepo.Verify(r => r.Update(It.IsAny<Driver>()), Times.Once);
        _mockVehicleRepo.Verify(r => r.Update(It.IsAny<Vehicle>()), Times.Once);
        _mockTripRepo.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Trip>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    // ========================================================================
    // 2. VALIDATION TESTS (فشل التحقق من صحة البيانات)
    // ========================================================================

    [Fact]
    public async Task Handle_WhenValidationFails_ShouldThrowValidationException()
    {
        // Arrange
        var command = TestDataFactory.CreateInvalidTripCommand();

        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateTripCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult
            {
                Errors = new List<ValidationFailure>
                {
                    new ValidationFailure("DriverId", "Driver ID must be greater than 0")
                }
            });

    // Act & Assert

await Assert.ThrowsAsync<ValidationException>(() =>
    _mediator.Send(command, CancellationToken.None));

// Verify no repository methods were called

_mockDriverRepo.Verify(
    r => r.GetByIdAsync(-11),
    Times.Never);

_mockVehicleRepo.Verify(
    r => r.GetByIdAsync(-12),
    Times.Never);

_mockTripRepo.Verify(
    r => r.AddAsync(It.IsAny<Domain.Entities.Trip>()),
    Times.Never);
    }

    // ========================================================================
    // 3. DRIVER VALIDATION TESTS
    // ========================================================================

    [Fact]
    public async Task Handle_WhenDriverNotFound_ShouldThrowException()
    {
        // Arrange
        var command = TestDataFactory.CreateValidTripCommand();

        _mockDriverRepo
            .Setup(r => r.GetByIdAsync(command.DriverId))
            .ReturnsAsync((Driver?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be($"Driver not found");

        // Verify no vehicle was fetched
        _mockVehicleRepo.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenDriverNotAvailable_ShouldThrowException()
    {
        // Arrange
        var command = TestDataFactory.CreateValidTripCommand();
        var driver = TestDataFactory.CreateBusyDriver(); // Not available

        _mockDriverRepo
            .Setup(r => r.GetByIdAsync(command.DriverId))
            .ReturnsAsync(driver);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Driver is not available");

        // Verify no vehicle was fetched
        _mockVehicleRepo.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenDriverLicenseExpired_ShouldThrowException()
    {
        // Arrange
        var command = TestDataFactory.CreateValidTripCommand();
        var driver = TestDataFactory.CreateAvailableDriver();
        driver.DriverLicense = TestDataFactory.CreateExpiredDriverLicense();

        _mockDriverRepo
            .Setup(r => r.GetByIdAsync(command.DriverId))
            .ReturnsAsync(driver);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Driver license is expired or missing");

        // Verify no vehicle was fetched
        _mockVehicleRepo.Verify(r => r.GetByIdAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WhenDriverHasNoLicense_ShouldThrowException()
    {
        // Arrange
        var command = TestDataFactory.CreateValidTripCommand();
        var driver = TestDataFactory.CreateDriverWithoutLicense();

        _mockDriverRepo
            .Setup(r => r.GetByIdAsync(command.DriverId))
            .ReturnsAsync(driver);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Driver license is expired or missing");
    }

    // ========================================================================
    // 4. VEHICLE VALIDATION TESTS
    // ========================================================================

    [Fact]
    public async Task Handle_WhenVehicleNotFound_ShouldThrowException()
    {
        // Arrange
        var command = TestDataFactory.CreateValidTripCommand();
        var driver = TestDataFactory.CreateAvailableDriver();

        _mockDriverRepo
            .Setup(r => r.GetByIdAsync(command.DriverId))
            .ReturnsAsync(driver);

        _mockVehicleRepo
            .Setup(r => r.GetByIdAsync(command.VehicleId))
            .ReturnsAsync((Vehicle?)null);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be($"Vehicle not found");
    }

    [Fact]
    public async Task Handle_WhenVehicleNotAvailable_ShouldThrowException()
    {
        // Arrange
        var command = TestDataFactory.CreateValidTripCommand();
        var driver = TestDataFactory.CreateAvailableDriver();
        var vehicle = TestDataFactory.CreateBusyVehicle();

        _mockDriverRepo
            .Setup(r => r.GetByIdAsync(command.DriverId))
            .ReturnsAsync(driver);

        _mockVehicleRepo
            .Setup(r => r.GetByIdAsync(command.VehicleId))
            .ReturnsAsync(vehicle);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Vehicle is not available");
    }

    [Fact]
    public async Task Handle_WhenVehicleLicenseExpired_ShouldThrowException()
    {
        // Arrange
        var command = TestDataFactory.CreateValidTripCommand();
        var driver = TestDataFactory.CreateAvailableDriver();
        var vehicle = TestDataFactory.CreateAvailableVehicle();
        vehicle.VehicleLicense = TestDataFactory.CreateExpiredVehicleLicense();

        _mockDriverRepo
            .Setup(r => r.GetByIdAsync(command.DriverId))
            .ReturnsAsync(driver);

        _mockVehicleRepo
            .Setup(r => r.GetByIdAsync(command.VehicleId))
            .ReturnsAsync(vehicle);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Vehicle license is expired or missing");
    }

    [Fact]
    public async Task Handle_WhenVehicleHasNoLicense_ShouldThrowException()
    {
        // Arrange
        var command = TestDataFactory.CreateValidTripCommand();
        var driver = TestDataFactory.CreateAvailableDriver();
        var vehicle = TestDataFactory.CreateVehicleWithoutLicense();

        _mockDriverRepo
            .Setup(r => r.GetByIdAsync(command.DriverId))
            .ReturnsAsync(driver);

        _mockVehicleRepo
            .Setup(r => r.GetByIdAsync(command.VehicleId))
            .ReturnsAsync(vehicle);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Vehicle license is expired or missing");
    }

    [Fact]
    public async Task Handle_WhenVehicleInActiveTrip_ShouldThrowException()
    {
        // Arrange
        var command = TestDataFactory.CreateValidTripCommand();
        var driver = TestDataFactory.CreateAvailableDriver();
        var vehicle = TestDataFactory.CreateAvailableVehicle();

        _mockDriverRepo
            .Setup(r => r.GetByIdAsync(command.DriverId))
            .ReturnsAsync(driver);

        _mockVehicleRepo
            .Setup(r => r.GetByIdAsync(command.VehicleId))
            .ReturnsAsync(vehicle);

        _mockTripRepo
            .Setup(r => r.ExistsAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Domain.Entities.Trip, bool>>>()))
            .ReturnsAsync(true); // Vehicle has active trip

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() => 
            _handler.Handle(command, CancellationToken.None));

        exception.Message.Should().Be("Vehicle already in active trip");
    }

    // ========================================================================
    // 5. EDGE CASES (الحالات الحدية)
    // ========================================================================

    [Theory]
    [InlineData(0, 1, "Cairo", "Alexandria", 100, 50)]
    [InlineData(-1, 1, "Cairo", "Alexandria", 100, 50)]
    public async Task Handle_WithInvalidDriverId_ShouldThrowException(
        int driverId, int vehicleId, string start, string end, double distance, double weight)
    {
        // Arrange
        var command = new CreateTripCommand
        (
            driverId,
            vehicleId,
            start,
             end,
             distance,
             weight
       );

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    [Theory]
    [InlineData("", "Alexandria", "Start location is required")]
    [InlineData(null, "Alexandria", "Start location is required")]
    [InlineData("Cairo", "", "End location is required")]
    [InlineData("Cairo", null, "End location is required")]
    public async Task Handle_WithInvalidLocation_ShouldThrowValidationException(
        string? start, string? end, string expectedError)
    {
        // Arrange
       CreateTripCommand command = TestDataFactory.CreateValidTripCommand() with
       {
           StartLocation = start,
          EndLocation = end
        };

        _mockValidator
            .Setup(v => v.ValidateAsync(It.IsAny<CreateTripCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult
            {
                Errors = new List<ValidationFailure>
                {
                    new ValidationFailure(
                        string.IsNullOrEmpty(start) ? "StartLocation" : "EndLocation",
                        expectedError)
                }
            });

        // Act & Assert
        
    
        var exception = await Assert.ThrowsAsync<ValidationException>(() => 
            _mediator.Send(command, CancellationToken.None));

        exception.Message.Should().Contain(expectedError);
    }
}