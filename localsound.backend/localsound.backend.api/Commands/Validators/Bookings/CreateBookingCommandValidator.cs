using FluentValidation;
using localsound.backend.api.Commands.Bookings;

namespace localsound.backend.api.Commands.Validators.Bookings
{
    public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(m => m.AppUserId)
                .NotEmpty()
                .WithMessage("There was an error creating your booking, please try again...");

            RuleFor(m => m.MemberId)
                .NotEmpty()
                .WithMessage("There was an error creating your booking, please try again...");

            RuleFor(m => m.BookingDto)
                .NotEmpty()
                .WithMessage("There was an error creating your booking, please try again...");

            When(x => x.BookingDto != null, () =>
            {
                RuleFor(m => m.BookingDto.ArtistId)
                    .NotEmpty()
                    .WithMessage("There was an error creating your booking, please try again...");

                RuleFor(m => m.BookingDto.PackageId)
                    .NotEmpty()
                    .WithMessage("There was an error creating your booking, please try again...");

                RuleFor(m => m.BookingDto.BookingAddress)
                    .NotEmpty()
                    .WithMessage("There was an error creating your booking, please try again...");

                RuleFor(m => m.BookingDto.BookingLength)
                    .NotEmpty()
                    .WithMessage("There was an error creating your booking, please try again...");

                RuleFor(m => m.BookingDto.EventTypeId)
                    .NotEmpty()
                    .WithMessage("There was an error creating your booking, please try again...");

                RuleFor(m => m.BookingDto.BookingDate)
                    .NotEmpty()
                    .WithMessage("There was an error creating your booking, please try again...");
            });
        }
    }
}
