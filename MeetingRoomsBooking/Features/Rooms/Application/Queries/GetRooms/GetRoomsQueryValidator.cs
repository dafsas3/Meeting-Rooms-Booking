using FluentValidation;

namespace MeetingRoomsBooking.Features.Rooms.Application.Queries.GetRooms
{
    public sealed class GetRoomsQueryValidator : AbstractValidator<GetRoomsQuery>
    {
        public GetRoomsQueryValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(5)
                .WithMessage("The length of this name must be greater than or equal to: 5.")
                .MaximumLength(30)
                .WithMessage("The length of this name must be less than or equal to: 30.")
                .When(x => !string.IsNullOrWhiteSpace(x.Name));

            RuleFor(x => x.Location)
                .MinimumLength(4)
                .WithMessage("The length of this location must be greater than or equal to: 4.")
                .MaximumLength(30)
                .WithMessage("The length of this name must be less than or equal to: 30.")
                .When(x => !string.IsNullOrWhiteSpace(x.Location));

            RuleFor(x => x.MinCapacity)
                .GreaterThan(0)
                .WithMessage("The capacity must be greater than zero.")
                .LessThanOrEqualTo(20)
                .WithMessage("The capacity must be less than: 20.")
                .When(x => x.MinCapacity is not null);
        }
    }
}
