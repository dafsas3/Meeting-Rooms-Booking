using FluentValidation;

namespace MeetingRoomsBooking.Features.Bookings.Application.Commands.CreateBookingRequest
{
    public class CreateBookingRequestCommandValidator : AbstractValidator<CreateBookingRequestCommand>
    {
        private const int MaxBookingHours = 4;

        public CreateBookingRequestCommandValidator()
        {
            RuleFor(cmd => cmd.RoomId)
               .GreaterThan(0).WithMessage("The room ID must be greater than zero.");

            RuleFor(cmd => cmd.StartedAtUtc)
            .NotEmpty()
            .WithMessage("Start time is required.");

            RuleFor(cmd => cmd.EndAtUtc)
                .NotEmpty()
                .WithMessage("End time is required.")
                .GreaterThan(cmd => cmd.StartedAtUtc)
                .WithMessage("End time must be greater than start time.");

            RuleFor(cmd => cmd)
                .Must(cmd => cmd.EndAtUtc - cmd.StartedAtUtc <= TimeSpan.FromHours(MaxBookingHours))
                .WithMessage($"Booking duration must not exceed {MaxBookingHours} hours.");

            RuleFor(cmd => cmd.StartedAtUtc)
                .Must(BeUtc)
                .WithMessage("Start time must be in UTC.");

            RuleFor(cmd => cmd.EndAtUtc)
                .Must(BeUtc)
                .WithMessage("End time must be in UTC.");

            RuleFor(cmd => cmd.Purpose)
                .NotEmpty()
                .WithMessage("Meeting purpose is required.")
                .MinimumLength(7)
                .WithMessage("Meeting purpose must be at least 7 characters long.")
                .MaximumLength(300)
                .WithMessage("Meeting purpose must be less than or equal to 300 characters.");

            RuleFor(cmd => cmd.Emails)
                .NotNull()
                .WithMessage("Participants list is required.")
                .Must(emails => emails.Count >= 1)
                .WithMessage("At least one participant email is required.")
                .Must(emails => emails.Count <= 20)
                .WithMessage("Participants list must contain no more than 20 emails.")
                .Must(emails => emails.Distinct(StringComparer.OrdinalIgnoreCase).Count() == emails.Count)
                .WithMessage("Participant emails must be unique.");

            RuleForEach(cmd => cmd.Emails)
                .NotEmpty()
                .WithMessage("Participant email cannot be empty.")
                .EmailAddress()
                .WithMessage("Participant email has invalid format.")
                .MaximumLength(254)
                .WithMessage("Participant email must be less than or equal to 254 characters.");
        }

        private static bool BeUtc(DateTime dateTime)
        => dateTime.Kind == DateTimeKind.Utc;
    }
}
