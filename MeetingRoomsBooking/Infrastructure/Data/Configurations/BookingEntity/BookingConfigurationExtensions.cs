using MeetingRoomsBooking.Features.Bookings.Domain.Entities;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingRequestId;
using MeetingRoomsBooking.Infrastructure.Data.Configurations.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetingRoomsBooking.Infrastructure.Data.Configurations.BookingEntity
{
    public static class BookingConfigurationExtensions 
    {
        public static void ConfigureValueObjects(this EntityTypeBuilder<BookingRequest> entity)
        {
            entity.Property(b => b.Id)
                .HasConversion(bId => bId.Value, value => BookingRequestId.FromDatabase(value))
                .IsRequired();

            entity.Property(b => b.RoomId)
                .HasRoomIdConversion()
                .IsRequired();

            entity.OwnsOne(b => b.TimeSlot, slot =>
            {
                slot.Property(s => s.StartAtUtc)
                .HasColumnName("StartAtUtc")
                .IsRequired();

                slot.Property(s => s.EndAtUtc)
                .HasColumnType("EndAtUtc")
                .IsRequired();
            });
        }
    }
}
