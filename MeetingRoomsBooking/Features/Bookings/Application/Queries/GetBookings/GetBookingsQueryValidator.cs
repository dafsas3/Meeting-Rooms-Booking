using FluentValidation;
using MeetingRoomsBooking.Features.Bookings.Domain.Enums;

namespace MeetingRoomsBooking.Features.Bookings.Application.Queries.GetBookings
{
    public sealed class GetBookingsQueryValidator : AbstractValidator<GetBookingsQuery>
    {
        public GetBookingsQueryValidator()
        {
            RuleFor(q => q.RoomId)
                .GreaterThan(0).WithMessage("The room ID must be greater than zero.")
                .When(q => q.RoomId is not null);

            RuleFor(q => q)
                .Must(q => (q.From == null && q.To == null) ||
                           (q.From != null && q.To != null))
                .WithMessage("Both \"from\" and \"to\" must be provided together.");

            RuleFor(q => q.Status)
                .Must(status => status == null || Enum.TryParse<BookingStatus>(status, true, out _))
                .WithMessage("Invalid booking status.");
        }
    }
}
