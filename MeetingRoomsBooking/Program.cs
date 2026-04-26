using FluentValidation;
using MeetingRoomsBooking.Api.Middleware;
using MeetingRoomsBooking.Api.Swagger;
using MeetingRoomsBooking.Features.Abstractions.Persistence.UnitOfWork;
using MeetingRoomsBooking.Features.Abstractions.Security;
using MeetingRoomsBooking.Features.Abstractions.Shared.Queries;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Queries;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Repositories;
using MeetingRoomsBooking.Features.Bookings.Application.Commands;
using MeetingRoomsBooking.Features.Rooms.Application.Abstractions.Repositories;
using MeetingRoomsBooking.Features.Rooms.Application.Commands.CreateRoom;
using MeetingRoomsBooking.Infrastructure.Persistence.Data;
using MeetingRoomsBooking.Infrastructure.Persistence.Features.Bookings.Queries;
using MeetingRoomsBooking.Infrastructure.Persistence.Features.Bookings.Repositories;
using MeetingRoomsBooking.Infrastructure.Persistence.Features.Rooms.Queries;
using MeetingRoomsBooking.Infrastructure.Persistence.Features.Rooms.Repositories;
using MeetingRoomsBooking.Infrastructure.Persistence.UnitOfWork;
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

builder.Services.AddScoped<IRoomQueries, EfRoomQueries>();
builder.Services.AddScoped<IRoomRepository, EfRoomRepository>();

builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();

builder.Services.AddScoped<IBookingQueries, EfBookingQueries>();
builder.Services.AddScoped<IBookingRepository, EfBookingRepository>();

builder.Services.AddScoped<CreateRoomHandler>();
builder.Services.AddScoped<CreateBookingRequestHandler>();

var connectionString = builder.Configuration.GetConnectionString("Default");

builder.Services.AddDbContext<BookingDbContext>(options =>
options.UseNpgsql(connectionString));

builder.Services.AddValidatorsFromAssemblyContaining<CreateRoomCommandValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookingRequestCommandValidator>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
