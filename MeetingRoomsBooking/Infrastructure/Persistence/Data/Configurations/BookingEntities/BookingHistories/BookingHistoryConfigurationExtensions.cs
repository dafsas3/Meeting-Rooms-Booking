using MeetingRoomsBooking.Features.Bookings.Domain.Entities;
using MeetingRoomsBooking.Features.Bookings.Domain.Ids.BookingHistoryId;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.StatusTransferReason;
using MeetingRoomsBooking.Infrastructure.Persistence.Data.Configurations.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetingRoomsBooking.Infrastructure.Persistence.Data.Configurations.BookingEntities.BookingHistories
{
    public static class BookingHistoryConfigurationExtensions
    {
        public static void ConfigureValueObjects(this EntityTypeBuilder<BookingHistory> entity)
        {
            entity.Property(h => h.Id)
                .HasConversion(id => id.Value, value => BookingHistoryId.FromDatabase(value))
                .IsRequired();

            entity.Property(h => h.BookingRequestId)
                .HasBookingRequestIdConversion()
                .IsRequired();

            entity.Property(h => h.EmployeeId)
                .HasEmployeeIdConversion()
                .IsRequired();

            entity.Property(h => h.ActorRole).IsRequired();

            entity.Property(h => h.FromStatus).IsRequired();

            entity.Property(h => h.ToStatus).IsRequired();

            entity.Property(h => h.Reason)
                .HasConversion(
                reason => reason.Value,
                value => StatusTransferReason.Create(value))
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(h => h.CreatedAtUtc)
                .HasColumnType("timestamp with time zone")
                .IsRequired();
        }

        public static void ConfigureIndexes(this EntityTypeBuilder<BookingHistory> entity)
        {
            entity.HasIndex(h => h.BookingRequestId)
                .HasDatabaseName(DatabaseConstants.BookingHistoryIndexes.BookingRequestId);

            entity.HasIndex(h => new { h.BookingRequestId, h.CreatedAtUtc })
                .HasDatabaseName(DatabaseConstants.BookingHistoryIndexes.BookingRequestIdCreatedAtUtc);
        }
    }
}
