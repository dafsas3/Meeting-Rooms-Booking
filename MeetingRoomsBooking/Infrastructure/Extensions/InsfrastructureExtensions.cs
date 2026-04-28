using MeetingRoomsBooking.Features.Abstractions.Persistence.UnitOfWork;
using MeetingRoomsBooking.Features.Abstractions.Shared.Queries;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Queries;
using MeetingRoomsBooking.Features.Bookings.Application.Abstractions.Repositories;
using MeetingRoomsBooking.Features.Bookings.Application.Services;
using MeetingRoomsBooking.Features.Rooms.Application.Abstractions.Repositories;
using MeetingRoomsBooking.Infrastructure.Persistence.Data;
using MeetingRoomsBooking.Infrastructure.Persistence.Features.Bookings.Queries;
using MeetingRoomsBooking.Infrastructure.Persistence.Features.Bookings.Repositories;
using MeetingRoomsBooking.Infrastructure.Persistence.Features.Bookings.Services;
using MeetingRoomsBooking.Infrastructure.Persistence.Features.Rooms.Queries;
using MeetingRoomsBooking.Infrastructure.Persistence.Features.Rooms.Repositories;
using MeetingRoomsBooking.Infrastructure.Persistence.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomsBooking.Infrastructure.Extensions
{
    public static class InsfrastructureExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default");

            services.AddDbContext<BookingDbContext>(options =>
            options.UseNpgsql(connectionString));

            services.AddScoped<IUnitOfWork, EfUnitOfWork>();

            services.AddScoped<IRoomQueries, EfRoomQueries>();
            services.AddScoped<IRoomRepository, EfRoomRepository>();

            services.AddScoped<IBookingQueries, EfBookingQueries>();
            services.AddScoped<IBookingRepository, EfBookingRepository>();

            services.AddScoped<IProtectionConccurency, EfProtectionConccurency>();

            return services;
        }
    }
}
