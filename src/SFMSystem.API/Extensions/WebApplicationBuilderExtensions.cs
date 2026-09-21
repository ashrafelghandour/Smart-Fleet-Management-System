using System.Reflection;
using FleetManagementSystem.Application.Commands.Trip.Create;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.Application.LoggingBehavior;
using FleetManagementSystem.Application.Mappings;
using FleetManagementSystem.Application.User;
using FleetManagementSystem.Application.ValidationBehavior;
using FleetManagementSystem.Domain.Entities;
using FleetManagementSystem.Domain.Enums;
using FleetManagementSystem.Infrastructure.Authorization;
using FleetManagementSystem.Infrastructure.Authorization.Permission;
using FleetManagementSystem.Infrastructure.Data;
using FleetManagementSystem.Infrastructure.Repositories;
using FleetManagementSystem.Infrastructure.UnitOfWork;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SFMSystem.API.Extensions;

public static class WebApplicationBuilderExtensions
{
       public static void AddWebServices(this WebApplicationBuilder builder)
       {
            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token"
                });
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services
                .AddIdentityApiEndpoints<AppUser>()
                .AddRoles<IdentityRole<int>>()
                .AddClaimsPrincipalFactory<PermeationsUserClaimsPrincipalFactory>()
                .AddEntityFrameworkStores<AppDbContext>();


             builder.Services.AddAuthorization(options =>
{
                options.AddPolicy("AdminOnly", policy => policy.RequireRole(Roles.Admin));
                options.AddPolicy("ManagerOrAdmin", policy => policy.RequireRole(Roles.Admin, Roles.Manager));
                options.AddPolicy("DispatcherOrHigher", policy => policy.RequireRole(Roles.Admin, Roles.Manager, Roles.Dispatcher));

                // ===== Permission-based Policies =====
                // Trip Policies
                options.AddPolicy("TripsView", policy => policy.RequirePermission(Permissions.TripsView));
                options.AddPolicy("TripsCreate", policy => policy.RequirePermission(Permissions.TripsCreate));
                options.AddPolicy("TripsEdit", policy => policy.RequirePermission(Permissions.TripsEdit));
                options.AddPolicy("TripsDelete", policy => policy.RequirePermission(Permissions.TripsDelete));
                options.AddPolicy("TripsComplete", policy => policy.RequirePermission(Permissions.TripsComplete));
                options.AddPolicy("TripsCancel", policy => policy.RequirePermission(Permissions.TripsCancel));

                // Driver Policies
                options.AddPolicy("DriversView", policy => policy.RequirePermission(Permissions.DriversView));
                options.AddPolicy("DriversCreate", policy => policy.RequirePermission(Permissions.DriversCreate));
                options.AddPolicy("DriversEdit", policy => policy.RequirePermission(Permissions.DriversEdit));
                options.AddPolicy("DriversDelete", policy => policy.RequirePermission(Permissions.DriversDelete));
                options.AddPolicy("DriversManageStatus", policy => policy.RequirePermission(Permissions.DriversManageStatus));

                // Vehicle Policies
                options.AddPolicy("VehiclesView", policy => policy.RequirePermission(Permissions.VehiclesView));
                options.AddPolicy("VehiclesCreate", policy => policy.RequirePermission(Permissions.VehiclesCreate));
                options.AddPolicy("VehiclesEdit", policy => policy.RequirePermission(Permissions.VehiclesEdit));
                options.AddPolicy("VehiclesDelete", policy => policy.RequirePermission(Permissions.VehiclesDelete));
                options.AddPolicy("VehiclesManageStatus", policy => policy.RequirePermission(Permissions.VehiclesManageStatus));

                // User Policies
                options.AddPolicy("UsersView", policy => policy.RequirePermission(Permissions.UsersView));
                options.AddPolicy("UsersCreate", policy => policy.RequirePermission(Permissions.UsersCreate));
                options.AddPolicy("UsersEdit", policy => policy.RequirePermission(Permissions.UsersEdit));
                options.AddPolicy("UsersDelete", policy => policy.RequirePermission(Permissions.UsersDelete));
                options.AddPolicy("UsersManageRoles", policy => policy.RequirePermission(Permissions.UsersManageRoles));

                // Report Policies
                options.AddPolicy("ReportsView", policy => policy.RequirePermission(Permissions.ReportsView));
                options.AddPolicy("ReportsGenerate", policy => policy.RequirePermission(Permissions.ReportsGenerate));
                options.AddPolicy("ReportsExport", policy => policy.RequirePermission(Permissions.ReportsExport));

                // ===== Combined Policies =====
                options.AddPolicy("FullAccess", policy => 
                    policy.RequireRole(Roles.Admin)
                        .RequirePermission(Permissions.TripsCreate)
                        .RequirePermission(Permissions.DriversCreate)
                        .RequirePermission(Permissions.VehiclesCreate));

                options.AddPolicy("TripManagement", policy => 
                    policy.RequireRole(Roles.Admin, Roles.Manager, Roles.Dispatcher)
                        .RequirePermission(Permissions.TripsView)
                        .RequirePermission(Permissions.TripsCreate)
                        .RequirePermission(Permissions.TripsComplete));
            });

        // Register Authorization Handler
        _ = builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();


        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
          
            builder.Services.AddScoped<IUserContext, UserContext>();
            builder.Services.AddHttpContextAccessor();
        
            builder.Services.AddValidatorsFromAssembly(typeof(CreateTripCommand).Assembly);

            builder.Services.AddMediatR(cfg => 
            {
                cfg.RegisterServicesFromAssembly(Assembly.Load("FleetManagementSystem.Application"));

                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            builder.Services.AddAutoMapper(typeof(MappingProfile));

       }
}