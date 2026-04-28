using MeetingRoomsBooking.Features.Abstractions.Common.Errors;

namespace MeetingRoomsBooking.Features.Bookings.Application.Errors
{
    public static class BookingError
    {
        public static ApiError RoomNotFound(int id) =>
            new("ROOM_NOT_FOUND", "The room ID not found.", id);

        public static ApiError TimeSlotBusy(DateTime start, DateTime end) =>
            new("TIME_SLOT_IS_BUSY", "This time slot is busy.", new[] { start, end });

        public static ApiError InvalidTimeRange =>
            new("INVALID_TIME_RANGE", "The entered time range is invalid.");

        public static ApiError RequestAlreadyExists(Guid IdempotencyKey) =>
            new("BOOKING_REQUEST_ALREADY_EXISTS", "This booking request is already exists.",
                IdempotencyKey);

        public static ApiError AccessDenied(string role) =>
            new("ACCESS_DENIED", "Only the employee role can create a reservation.", role);

        public static ApiError NotOwner(Guid userId, int bookingId) =>
            new("INVALID_NOT_OWNER",
                "Only the owner can cancel a reservation.",
                new Dictionary<string, object>
                {
                    ["UserId"] = userId,
                    ["BookingId"] = bookingId
                });

        public static ApiError RoomNotActive =>
            new("ROOM_NOT_ACTIVE", "This room is currently unavailable.");

        public static ApiError BookingNotFound(int id) =>
            new("BOOKING_NOT_FOUND", "The booking ID not found.", id);

        public static ApiError BookingStatusAlreadyChange =>
            new("BOOKING_STATUS_ALREADY_CHANGE", "The status of this booking has already been changed.");

        public static ApiError InvalidStatus =>
            new("INVALID_BOOKING_STATUS", "An incorrect booking status has been entered.");

        public static ApiError ManyParticipants =>
            new("INVALID_MANY_PARTICIPANTS",
                "The booking request indicates a number of participants greater than " +
                "the room can accommodate.");
    }
}
