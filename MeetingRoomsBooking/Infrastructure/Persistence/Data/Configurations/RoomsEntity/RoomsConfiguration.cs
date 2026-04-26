using MeetingRoomsBooking.Features.Rooms.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetingRoomsBooking.Infrastructure.Persistence.Data.Configurations.RoomsEntity
{
    public sealed class RoomsConfiguration : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> entity)
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.Id).ValueGeneratedOnAdd();

            entity.ConfigureValueObjects();
            entity.ConfigureIndexes();
        }
    }
}
