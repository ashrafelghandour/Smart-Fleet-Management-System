using AutoMapper;
using FleetManagementSystem.Application.Commands.Trip.AddExpense;
using FleetManagementSystem.Application.Commands.Trip.AddTripExpense;
using FleetManagementSystem.Application.Commands.Trip.Create;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.Application.Mappings;
using FleetManagementSystem.Application.ValidationBehavior;
using FleetManagementSystem.Domain.Entities;
using FleetManagementSystem.Domain.Enums;
using FleetManagementSystem.UnitTests.Mocks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using SFMSystem.Application.Commands.Trip.AddExpense;
using System.Runtime.InteropServices;
namespace FMS.Test.Commands.Trip.AddExpense;


//
// {
//     public async Task<TripResponse> Handle(AddTripExpenseCommand request, CancellationToken cancellationToken)
//     {
//         var trip = await _tripRepository.GetByIdAsync(request.TripId);
//         if (trip == null)
//             throw new Exception("Trip not found");

//         var expense = new TripExpense
//         {
//             TripId = request.TripId,
//             Amount = request.Amount,
//             Description = request.Description,
//             ExpenseType = request.ExpenseType
//         };

//         await _expenseRepository.AddAsync(expense);
//         await _unitOfWork.SaveChangesAsync(cancellationToken);

//         return _mapper.Map<TripResponse>(trip);
//     }
public class AddTripExpenseCommandHandlerTest
{
    private readonly Mock<IGenericRepository<FleetManagementSystem.Domain.Entities.Trip>> _mocktripRepository;
    private readonly Mock<IGenericRepository<TripExpense>> _mockExpenseRepository;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly IMapper _mapper;
    private readonly AddTripExpenseCommandHandler _handler;
    private readonly IMediator _mediator;

    public AddTripExpenseCommandHandlerTest()
        
        
    {
        // Arrange - Repositories
        _mocktripRepository =
            new Mock<IGenericRepository<FleetManagementSystem.Domain.Entities.Trip>>();

        _mockExpenseRepository =
            new Mock<IGenericRepository<TripExpense>>();

        // Arrange - UnitOfWork
        _mockUnitOfWork =
            MockUnitOfWork.CreateMockUnitOfWork()
                .SetupTripRepository(_mocktripRepository)
                .SetupTripExpenseRepository(_mockExpenseRepository);

        // Arrange - Mapper
        var config = new MapperConfiguration(cfg =>
            cfg.AddProfile<MappingProfile>());

        _mapper = config.CreateMapper();

        // Arrange - Services
        var services = new ServiceCollection();

        services.AddLogging();

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblyContaining<AddTripExpenseCommandHandler>();
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        // Register real validators
        services.AddValidatorsFromAssemblyContaining<AddTripExpenseCommandValidator>();

        // Register repositories
        services.AddSingleton(_mocktripRepository.Object);
        services.AddSingleton(_mockExpenseRepository.Object);

        // Register UnitOfWork
        services.AddSingleton(_mockUnitOfWork.Object);

        // Register Mapper
        services.AddSingleton(_mapper);

        var provider = services.BuildServiceProvider();

        _mediator = provider.GetRequiredService<IMediator>();
}

    
     [Fact]
public async Task Handle_WhenTripNotFound_ShouldThrowException()
{
    // Arrange
    var command = new AddTripExpenseCommand
    {
        TripId = 999,
        Amount = 100,
        Description = "Fuel",
        ExpenseType = ExpenseType.Other
    };

    _mocktripRepository
        .Setup(repo => repo.GetByIdAsync(command.TripId))
        .ReturnsAsync((FleetManagementSystem.Domain.Entities.Trip?)null);

    // Act
    Func<Task> act = async () => await _mediator.Send(command);

    // Assert
    await act.Should()
        .ThrowAsync<Exception>()
        .WithMessage("Trip not found");

    _mockExpenseRepository.Verify(
        repo => repo.AddAsync(It.IsAny<TripExpense>()),
        Times.Never);

    _mockUnitOfWork.Verify(
        uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()),
        Times.Never);
}

    [Fact]
    public async Task Handle_ForValidCommand_ShouldAddExpenseAndReturnTripResponse()
    {
        // Arrange
        var command = new AddTripExpenseCommand
        {
            TripId = 1,
            Amount = 100.0m,
            Description = "Fuel",
            ExpenseType = ExpenseType.Other
        };

        var trip = new FleetManagementSystem.Domain.Entities.Trip
        {
            Id = 1,
            DriverId = 1,
            StartLocation = "Location A",
            EndLocation = "Location B",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddHours(1),
            Distance = 100.5,
            CargoWeight = 500.0
        };

        _mocktripRepository.Setup(repo => repo.GetByIdAsync(command.TripId)).ReturnsAsync(trip);

        // Act
        var result = await _mediator.Send(command);
       

        // Assert
        _mocktripRepository.Verify(repo => repo.GetByIdAsync(command.TripId), Times.Once);
        _mockExpenseRepository.Verify(repo => repo.AddAsync(It.IsAny<TripExpense>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        result.Should().NotBeNull();
        result.Id.Should().Be(trip.Id);
    }

    [Theory]
    [InlineData(-1, 1900)]
    [InlineData(1, 0)]
public async Task Handle_WhenCommandIsInvalid_ShouldThrowValidationException(int tripId, decimal amount)
{
    // Arrange

    var command = new AddTripExpenseCommand
    {
        TripId = tripId,
        Amount = amount,
        Description = "",
        ExpenseType = ExpenseType.Other
    };

    // Act

    Func<Task> act = async () =>
        await _mediator.Send(command);

    // Assert

    await act.Should()
        .ThrowAsync<ValidationException>();

    // Handler should never be reached

    _mocktripRepository.Verify(
        repo => repo.GetByIdAsync(
            It.IsAny<int>()),
        Times.Never);

    _mockExpenseRepository.Verify(
        repo => repo.AddAsync(
            It.IsAny<TripExpense>()),
        Times.Never);

    _mockUnitOfWork.Verify(
        uow => uow.SaveChangesAsync(
            It.IsAny<CancellationToken>()),
        Times.Never);
}
}


