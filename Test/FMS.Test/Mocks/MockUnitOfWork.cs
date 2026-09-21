using Moq;
using FleetManagementSystem.Domain.Entities;
using FleetManagementSystem.Domain;
using FleetManagementSystem.Domain.Common;
using FleetManagementSystem.Application.Interface;

namespace FleetManagementSystem.UnitTests.Mocks;

public static class MockUnitOfWork
{
    public static Mock<IUnitOfWork> CreateMockUnitOfWork()
    {
        return new Mock<IUnitOfWork>();
    }

    public static Mock<IUnitOfWork> SetupTripRepository(
        this Mock<IUnitOfWork> mock,
        Mock<IGenericRepository<Trip>> mockRepo)
    {
        mock.Setup(u => u.Repository<Trip>())
            .Returns(mockRepo.Object);
        return mock;
    }

    public static Mock<IUnitOfWork> SetupDriverRepository(
        this Mock<IUnitOfWork> mock,
        Mock<IGenericRepository<Driver>> mockRepo)
    {
        mock.Setup(u => u.Repository<Driver>())
            .Returns(mockRepo.Object);
        return mock;
    }

    public  static Mock<IUnitOfWork> SetupVehicleRepository(
        this Mock<IUnitOfWork> mock,
        Mock<IGenericRepository<Vehicle>> mockRepo)
    {
        mock.Setup(u => u.Repository<Vehicle>())
            .Returns(mockRepo.Object);
        return mock;
    }

    public static Mock<IUnitOfWork> SetupTripExpenseRepository(
        this Mock<IUnitOfWork> mock,
        Mock<IGenericRepository<TripExpense>> mockRepo)
    {
        mock.Setup(u => u.Repository<TripExpense>())
            .Returns(mockRepo.Object);
        return mock;
    }
}