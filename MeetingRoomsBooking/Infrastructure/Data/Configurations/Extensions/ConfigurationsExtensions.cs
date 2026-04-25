using MeetingRoomsBooking.BuildingBlocks.Domain.Room.RoomId;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeetingRoomsBooking.Infrastructure.Data.Configurations.Extensions
{
    public static class ConfigurationsExtensions
    {
        public static PropertyBuilder<RoomId> HasRoomIdConversion(this PropertyBuilder<RoomId> property)
            => property.HasConversion(rId => rId.Value, value => RoomId.FromDatabase(value));
    }
}
