using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using MeetingRoomsBooking.BuildingBlocks.Domain.ValueObjects.IdempotencyKey;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingRequestId;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.EmployeeId;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetingRoomsBooking.Infrastructure.Persistence.Data.Configurations.Extensions
{
    public static class ConfigurationsExtensions
    {
        public static PropertyBuilder<RoomId> HasRoomIdConversion(this PropertyBuilder<RoomId> property)
            => property.HasConversion(rId => rId.Value, value => RoomId.FromDatabase(value));

        public static PropertyBuilder<IdempotencyKey> HasIdempotencyKeyConversion(
            this PropertyBuilder<IdempotencyKey> property) =>
            property.HasConversion(key => key.Value, value => IdempotencyKey.Create(value));

        public static PropertyBuilder<BookingRequestId> HasBookingRequestIdConversion(
            this PropertyBuilder<BookingRequestId> property) =>
            property.HasConversion(id => id.Value, value => BookingRequestId.FromDatabase(value));

        public static PropertyBuilder<EmployeeId> HasEmployeeIdConversion(
            this PropertyBuilder<EmployeeId> property) =>
            property.HasConversion(id => id.Value, value => EmployeeId.FromDatabase(value));
    }
}
