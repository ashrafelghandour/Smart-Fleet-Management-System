using FleetManagementSystem.Domain.Entities;
using SFMSystem.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.AddWebServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapGroup("api/identity")
.WithTags("Identity")
.MapIdentityApi<AppUser>();

app.MapControllers();


app.Run();

