using MeetingRoomsBooking.Features.Abstractions.Common.Result;

namespace MeetingRoomsBooking.Features.Bookings.Application.Commands
{
    public class CreateBookingRequestHandler
    {

        public async Task<Result<CreateBookingRequestResponse>> Handle(
            CreateBookingRequestCommand cmd, CancellationToken ct)
        {

        }

    }
}
