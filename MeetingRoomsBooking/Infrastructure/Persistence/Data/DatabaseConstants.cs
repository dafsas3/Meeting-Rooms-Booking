namespace MeetingRoomsBooking.Infrastructure.Persistence.Data
{
    public static class DatabaseConstants
    {
        public static class UniqueRoomIndexes
        {
            public const string RoomNameAndLocation = "UX_Rooms_Name_Location";
        }

        public static class UniqueBookingIndexes
        {
            public const string IdempotencyKey = "UX_BookingRequests_IdempotencyKey";
        }
        
        public static class BookingIndexes
        {
            public const string RoomStatusTimeSlot = "IX_BookingRequests_Room_Status_TimeSlot";
            public const string EmployeeAndStatus = "IX_BookingRequests_Employee_Status";
        }

        public static class BookingHistoryIndexes
        {
            public const string BookingRequestIdCreatedAtUtc = 
                "IX_BookingHistories_BookingRequestId_CreatedAtUtc";

            public const string BookingRequestId = "IX_BookingHistories_BookingRequestId";
        }
    }
}
