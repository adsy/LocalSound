using FluentValidation;
using localsound.backend.api.Commands.Bookings;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace localsound.backend.api.Commands.Validators.Bookings
{
    public class AcceptBookingCommandValidator : AbstractValidator<AcceptBookingCommand>
    {
        private readonly LocalSoundDbContext _context;

        public AcceptBookingCommandValidator(LocalSoundDbContext context)
        {
            _context = context;
            RuleFor(m => m.AppUserId)
                .NotEmpty()
                .WithMessage("There was an error accepting your booking, please try again...");

            RuleFor(m => m.MemberId)
                .NotEmpty()
                .WithMessage("There was an error accepting your booking, please try again...");

            RuleFor(m => m.BookingId)
                .NotEmpty()
                .WithMessage("There was an error accepting your booking, please try again...");

            RuleFor(m => m)
                .MustAsync(async (m, x) => await BookingBelongsToUser(m))
                .WithMessage("There was an error accepting your booking, please try again...");

            RuleFor(m => m)
                .MustAsync(async (m, x) => await BookingIsPending(m))
                .WithMessage("You can only accept upcoming bookings.");

        }

        private async Task<bool> BookingIsPending(AcceptBookingCommand m)
        {
            return (await _context.ArtistBooking.FirstOrDefaultAsync(x => x.BookingId == m.BookingId && x.BookingDate >= new DateTime() && x.BookingConfirmed == null)) != null;
        }

        private async Task<bool> BookingBelongsToUser(AcceptBookingCommand m)
        {
            return (await _context.ArtistBooking.FirstOrDefaultAsync(x => x.ArtistId == m.AppUserId && x.BookingId == m.BookingId)) != null;
        }
    }
}