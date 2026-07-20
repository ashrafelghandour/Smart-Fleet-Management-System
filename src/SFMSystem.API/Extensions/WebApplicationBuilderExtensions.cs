using System.Reflection;
using FleetManagementSystem.Application.Commands.Trip.Create;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.Application.LoggingBehavior;
using FleetManagementSystem.Application.ValidationBehavior;
using FleetManagementSystem.Domain.Entities;
using FleetManagementSystem.Infrastructure.Data;
using FleetManagementSystem.Infrastructure.Repositories;
using FleetManagementSystem.Infrastructure.UnitOfWork;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace SFMSystem.API.Extensions;

public static class WebApplicationBuilderExtensions
{
       public static void AddWebServices(this WebApplicationBuilder builder)
       {
        builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your token\n\nExample: Bearer abc123"
                });

            
            });


            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services
                .AddIdentityApiEndpoints<AppUser>()
                .AddRoles<IdentityRole<int>>()
                .AddEntityFrameworkStores<AppDbContext>();
                    
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            builder.Services.AddValidatorsFromAssembly(typeof(CreateTripCommand).Assembly);

            builder.Services.AddMediatR(cfg => 
            {
                cfg.RegisterServicesFromAssembly(Assembly.Load("FleetManagementSystem.Application"));

                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            });

            builder.Services.AddAutoMapper(typeof(Program));

       }
}