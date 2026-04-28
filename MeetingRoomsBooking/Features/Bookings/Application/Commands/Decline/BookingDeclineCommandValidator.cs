using FluentValidation;

namespace MeetingRoomsBooking.Features.Bookings.Application.Commands.Decline
{
    public sealed class BookingDeclineCommandValidator : AbstractValidator<BookingDeclineCommand>
    {
        public BookingDeclineCommandValidator()
        {
            RuleFor(cmd => cmd.BookingRequestId)
                .NotEmpty().WithMessage("Booking request ID is required.")
                .GreaterThan(0).WithMessage("Booking request ID must be greater than zero.");

            RuleFor(cmd => cmd.Reason)
                .NotEmpty().WithMessage("Reason for booking rejection is required.")
                .MinimumLength(10)
                .WithMessage("The length of the reason must be greater than or equal to: 10.")
                .MaximumLength(200)
                .WithMessage("The length of the reason must be greater than or equal to: 200.");
        }
    }
}
