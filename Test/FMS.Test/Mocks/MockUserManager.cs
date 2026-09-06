using FleetManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace SFMSystem.UnitTests.Mocks;

public static class MockUserManager
{
    public static Mock<UserManager<AppUser>> CreateMockUserManager()
    {
        var store = new Mock<IUserStore<AppUser>>();
        var options = new Mock<IOptions<IdentityOptions>>();
        var passwordHasher = new Mock<IPasswordHasher<AppUser>>();
        var userValidators = new List<IUserValidator<AppUser>>();
        var passwordValidators = new List<IPasswordValidator<AppUser>>();
        var keyNormalizer = new Mock<ILookupNormalizer>();
        var errors = new Mock<IdentityErrorDescriber>();
        var services = new Mock<IServiceProvider>();
        var logger = new Mock<ILogger<UserManager<AppUser>>>();

        var userManager = new Mock<UserManager<AppUser>>(
            store.Object,
            options.Object,
            passwordHasher.Object,
            userValidators,
            passwordValidators,
            keyNormalizer.Object,
            errors.Object,
            services.Object,
            logger.Object
        );

        return userManager;
    }
}