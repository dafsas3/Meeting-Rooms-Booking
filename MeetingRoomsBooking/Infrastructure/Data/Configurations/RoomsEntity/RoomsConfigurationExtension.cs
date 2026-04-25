using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using MeetingRoomsBooking.Features.Rooms.Domain.Entities;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomCapacity;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomLocation;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomName;
using MeetingRoomsBooking.Infrastructure.Data.Configurations.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetingRoomsBooking.Infrastructure.Data.Configurations.RoomsEntity
{
    public static class RoomsConfigurationExtension
    {
        public static void ConfigureValueObjects(this EntityTypeBuilder<Room> entity)
        {
            entity.Property(r => r.Id)
                .HasRoomIdConversion()
                .IsRequired();

            entity.Property(r => r.Name)
                .HasConversion(name => name.Value, value => RoomName.Create(value))
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(r => r.Capacity)
                .HasConversion(capacity => capacity.Value, value => RoomCapacity.Create(value))
                .IsRequired();

            entity.Property(r => r.Location)
                .HasConversion(location => location.Value, value => RoomLocation.Create(value))
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(r => r.IsActive)
                .IsRequired();
        }


        public static void ConfigureIndexes(this EntityTypeBuilder<Room> entity)
        {
            entity.HasIndex(r => new { r.Name, r.Location })
                .IsUnique()
                .HasDatabaseName(DatabaseConstants.UniqueRoomIndexes.RoomNameAndLocation);
        }
    }
}
