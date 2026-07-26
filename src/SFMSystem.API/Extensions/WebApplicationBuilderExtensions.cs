using System.Reflection;
using System.Text;
using FleetManagementSystem.Application.Commands.Trip.Create;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.Application.LoggingBehavior;
using FleetManagementSystem.Application.Mappings;
using FleetManagementSystem.Application.User;
using FleetManagementSystem.Application.ValidationBehavior;
using FleetManagementSystem.Domain.Entities;
using FleetManagementSystem.Infrastructure.Data;
using FleetManagementSystem.Infrastructure.Repositories;
using FleetManagementSystem.Infrastructure.UnitOfWork;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace SFMSystem.API.Extensions;

public static class WebApplicationBuilderExtensions
{
       public static void AddWebServices(this WebApplicationBuilder builder)
       {
            builder.Services.AddAuthentication();
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
                .AddEntityFrameworkStores<AppDbContext>();
                    



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