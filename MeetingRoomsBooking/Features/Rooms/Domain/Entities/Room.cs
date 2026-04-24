using MeetingRoomsBooking.Features.Rooms.Domain.Ids;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomCapacity;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomLocation;
using MeetingRoomsBooking.Features.Rooms.Domain.ValueObjects.RoomName;

namespace MeetingRoomsBooking.Features.Rooms.Domain.Entities
{
    public sealed class Room
    {
        public RoomId Id { get; private set; }
        public RoomName Name { get; private set; } = null!;
        public RoomCapacity Capacity { get; private set; } = null!;
        public RoomLocation Location { get; private set; } = null!;
        public bool IsActive { get; private set; }


        private Room() { }

        private Room(
            RoomName name,
            RoomCapacity capacity,
            RoomLocation location,
            bool isActive)
        {
            Name = name;
            Capacity = capacity;
            Location = location;
            IsActive = isActive;
        }


        public static Room Create(
            RoomName name,
            RoomCapacity capacity,
            RoomLocation location,
            bool isActive)
        {
            return new(name, capacity, location, isActive);
        }


        public void Activate() => IsActive = true;
        public void DeActivate() => IsActive = false;
        public bool CanFit(int participantsCount) => participantsCount <= Capacity.Value;

    }
}
