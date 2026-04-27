using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MeetingRoomsBooking.Infrastructure.Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddXminConcurrencyToBookingRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "BookingRequests",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "BookingRequests");
        }
    }
}
