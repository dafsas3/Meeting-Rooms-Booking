using FluentValidation;

namespace MeetingRoomsBooking.Features.Rooms.Application.Commands.CreateRoom
{
    public sealed class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
    {
        public CreateRoomCommandValidator()
        {
            RuleFor(cmd => cmd.Name)
                .NotEmpty().WithMessage("The name cannot be empty.")
                .MinimumLength(5).WithMessage("The length of this name must be greater or equal to: 5.")
                .MaximumLength(30)
                .WithMessage("The length of this name must be less than or equal to: 30.");

            RuleFor(cmd => cmd.ReqCapacity)
                .GreaterThan(0).WithMessage("The capacity must be greater than: 0.")
                .LessThanOrEqualTo(20).WithMessage("The capacity must be less than or equal to: 20.");

            RuleFor(cmd => cmd.Location)
                .NotEmpty().WithMessage("The location cannot be empty.")
                .MinimumLength(4).WithMessage("The length of this location must be greater or equal to: 4.")
                .MaximumLength(30).WithMessage("The length of this location must be less or equal to: 30.");
        }
    }
}
