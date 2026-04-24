using MeetingRoomsBooking.Features.Rooms.Domain.Entities;
using MeetingRoomsBooking.Features.Rooms.Domain.Ids;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomCapacity;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomLocation;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomName;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetingRoomsBooking.Infrastructure.Data.Configurations.RoomsEntity
{
    public static class RoomsConfigurationExtension
    {
        public static void ConfigureValueObjects(this EntityTypeBuilder<Room> entity)
        {
            entity.Property(r => r.Id)
                .HasConversion(rId => rId.Value, value => RoomId.FromDatabase(value))
                .IsRequired();

            entity.Property(r => r.Name)
                .HasConversion(name => name.Value, value => RoomName.Create(value))
                .IsRequired();

            entity.Property(r => r.Capacity)
                .HasConversion(capacity => capacity.Value, value => RoomCapacity.Create(value))
                .IsRequired();

            entity.Property(r => r.Location)
                .HasConversion(location => location.Value, value => RoomLocation.Create(value))
                .IsRequired();

            entity.Property(r => r.IsActive)
                .IsRequired();
        }
    }
}
