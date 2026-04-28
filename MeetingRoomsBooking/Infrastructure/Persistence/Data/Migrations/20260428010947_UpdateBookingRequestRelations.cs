using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingRoomsBooking.Infrastructure.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingRequestRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TimeSlot_EndAtUtc",
                table: "BookingRequests",
                newName: "EndAtUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EndAtUtc",
                table: "BookingRequests",
                newName: "TimeSlot_EndAtUtc");
        }
    }
}
