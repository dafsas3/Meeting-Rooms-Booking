using MeetingRoomsBooking.Features.Bookings.Domain.Entities;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingRequestId;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.UserId;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.MeetingPurpose;
using MeetingRoomsBooking.Infrastructure.Persistence.Data.Configurations.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetingRoomsBooking.Infrastructure.Persistence.Data.Configurations.BookingEntity
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

            entity.Property(b => b.EmployeeId)
                .HasConversion(eId => eId.Value, value => EmployeeId.FromDatabase(value))
                .IsRequired();

            entity.OwnsOne(b => b.TimeSlot, slot =>
            {
                slot.Property(s => s.StartAtUtc)
                .HasColumnName("StartAtUtc")
                .HasColumnType("datetime(6)")
                .IsRequired();

                slot.Property(s => s.EndAtUtc)
                .HasColumnType("EndAtUtc")
                .HasColumnType("datetime(6)")
                .IsRequired();
            });

            entity.Property(b => b.MeetingPurpose)
                .HasConversion(mp => mp.Value, value => MeetingPurpose.Create(value))
                .HasMaxLength(300)
                .IsRequired();

            entity.Property(b => b.Status)
                .IsRequired();
        }
    }
}
