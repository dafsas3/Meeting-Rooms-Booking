using MeetingRoomsBooking.Features.Bookings.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetingRoomsBooking.Infrastructure.Persistence.Data.Configurations.BookingEntities.BookingHistories
{
    public sealed class BookingHistoryConfiguration : IEntityTypeConfiguration<BookingHistory>
    {
        public void Configure(EntityTypeBuilder<BookingHistory> entity)
        {
            entity.HasKey(h => h.Id);
            entity.Property(h => h.Id).ValueGeneratedOnAdd();

            entity.ConfigureValueObjects();
            entity.ConfigureIndexes();
        }
    }
}
