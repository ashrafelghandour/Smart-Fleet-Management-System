using Microsoft.EntityFrameworkCore;
using FleetManagementSystem.Infrastructure.Data;
using FleetManagementSystem.Infrastructure.Repositories;
using FleetManagementSystem.Application.Interface;
using FleetManagementSystem.Infrastructure.UnitOfWork;
using MediatR;
using FleetManagementSystem.Application.Commands.Trip.Create;
using FluentValidation;
using FleetManagementSystem.Application.ValidationBehavior;
using FleetManagementSystem.Application.LoggingBehavior;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


builder.Services.AddValidatorsFromAssembly(typeof(CreateTripCommand).Assembly);

builder.Services.AddMediatR(cfg => 
{
    cfg.RegisterServicesFromAssembly(typeof(CreateTripCommand).Assembly);

     cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
    cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
});

builder.Services.AddAutoMapper(typeof(Program));


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();


app.Run();

