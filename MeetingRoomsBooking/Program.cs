using FluentValidation;
using MeetingRoomsBooking.Api.Middleware;
using MeetingRoomsBooking.Api.Swagger;
using MeetingRoomsBooking.Features.Abstractions.Security;
using MeetingRoomsBooking.Features.Rooms.Application.Commands.CreateRoom;
using MeetingRoomsBooking.Infrastructure.Data;
using MeetingRoomsBooking.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<UserHeadersOperationFilter>();
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<BookingDbContext>(options =>
options.UseNpgsql(connectionString));

builder.Services.AddValidatorsFromAssemblyContaining<CreateRoomCommandValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
