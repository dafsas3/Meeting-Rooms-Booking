using MeetingRoomsBooking.Features.Abstractions.Common.Errors;

namespace MeetingRoomsBooking.Features.Bookings.Application.Errors
{
    public static class BookingError
    {
        public static ApiError RoomNotFound(int id) =>
            new("ROOM_BY_ID_NOT_FOUND", "The room id not found.", id);

        public static ApiError TimeSlotBusy(DateTime start, DateTime end) =>
            new("TIME_SLOT_IS_BUSY", "This time slot is busy.", new[] {start, end});

        public static ApiError RequestAlreadyExists(Guid IdempotencyKey) =>
            new("BOOKING_REQUEST_ALREADY_EXISTS", "This booking request is already exists.",
                IdempotencyKey);

        public static ApiError AccessDenied(string role) =>
            new("ACCESS_DENIED", "Only the employee role can create a reservation.", role);

        public static ApiError RoomNotActive =>
            new("ROOM_NOT_ACTIVE", "This room is currently unavailable.");
    }
}
