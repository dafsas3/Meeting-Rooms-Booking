using FluentValidation;
using MeetingRoomsBooking.Features.Bookings.Application.Commands.Cancel;
using MeetingRoomsBooking.Features.Bookings.Application.Commands.Confirm;
using MeetingRoomsBooking.Features.Bookings.Application.Commands.CreateBookingRequest;
using MeetingRoomsBooking.Features.Bookings.Application.Commands.Decline;
using MeetingRoomsBooking.Features.Bookings.Application.Commands.Submit;
using MeetingRoomsBooking.Features.Bookings.Application.Queries.GetBookings;
using MeetingRoomsBooking.Features.Bookings.Application.Queries.GetDetails;
using MeetingRoomsBooking.Features.Rooms.Application.Commands.CreateRoom;
using MeetingRoomsBooking.Features.Rooms.Application.Queries.GetRooms;

namespace MeetingRoomsBooking.Features.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<CreateRoomHandler>();
            services.AddScoped<GetRoomsHandler>();

            services.AddScoped<CreateBookingRequestHandler>();
            services.AddScoped<SubmitBookingHandler>();
            services.AddScoped<ConfirmBookingHandler>();
            services.AddScoped<BookingCancelHandler>();
            services.AddScoped<BookingDeclineHandler>();

            services.AddScoped<GetBookingsHandler>();
            services.AddScoped<GetBookingDetailsHandler>();

            services.AddValidatorsFromAssemblyContaining<CreateRoomCommandValidator>();

            return services;
        }
    }
}
