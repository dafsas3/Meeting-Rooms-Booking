using MeetingRoomsBooking.Features.Bookings.Domain.Entities;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.MeetingPurpose;
using MeetingRoomsBooking.Features.Bookings.Domain.ValueObjects.ParticipantEmail;
using MeetingRoomsBooking.Infrastructure.Persistence.Data.Configurations.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetingRoomsBooking.Infrastructure.Persistence.Data.Configurations.BookingEntities
{
    public static class BookingConfigurationExtensions
    {
        public static void ConfigureValueObjects(this EntityTypeBuilder<BookingRequest> entity)
        {
            entity.Property(b => b.Id)
                .HasBookingRequestIdConversion()
                .IsRequired();

            entity.Property(b => b.RoomId)
                .HasRoomIdConversion()
                .IsRequired();

            entity.Property(b => b.EmployeeId)
                .HasEmployeeIdConversion()
                .IsRequired();

            entity.Property(b => b.IdempotencyKey)
                .HasIdempotencyKeyConversion()
                .IsRequired();

            entity.OwnsOne(b => b.TimeSlot, slot =>
            {
                slot.Property(s => s.StartAtUtc)
                .HasColumnName("StartAtUtc")
                .HasColumnType("timestamp with time zone")
                .IsRequired();

                slot.Property(s => s.EndAtUtc)
                .HasColumnType("EndAtUtc")
                .HasColumnType("timestamp with time zone")
                .IsRequired();
            });

            entity.Property(b => b.MeetingPurpose)
                .HasConversion(mp => mp.Value, value => MeetingPurpose.Create(value))
                .HasMaxLength(300)
                .IsRequired();

            entity.Property(b => b.Status)
                .IsRequired();

            entity.Ignore(b => b.ParticipantEmails);
            entity.Ignore(b => b.History);

            entity.OwnsMany<ParticipantEmail>("_participants", p =>
            {
                p.ToTable("BookingParticipants");

                p.WithOwner().HasForeignKey("BookingRequestId");

                p.Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired();

                p.HasKey("BookingRequestId", nameof(ParticipantEmail.Value));
            });

            entity.HasMany<BookingHistory>("_history")
                .WithOne()
                .HasForeignKey("BookingRequestId")
                .OnDelete(DeleteBehavior.Cascade);
        }

        
        public static void ConfigureIndexes(this EntityTypeBuilder<BookingRequest> entity)
        {
            entity.HasIndex(b => b.IdempotencyKey)
                .IsUnique()
                .HasDatabaseName(DatabaseConstants.UniqueBookingIndexes.IdempotencyKey);

            entity.HasIndex(b => new
            {
                b.RoomId,
                b.Status
            }).HasDatabaseName(DatabaseConstants.BookingIndexes.RoomStatusTimeSlot);

            entity.HasIndex(x => new
            {
                x.EmployeeId,
                x.Status
            }).HasDatabaseName(DatabaseConstants.BookingIndexes.EmployeeAndStatus);
        }
    }
}
