using FluentValidation;
using localsound.backend.api.Commands.Bookings;
using localsound.backend.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace localsound.backend.api.Commands.Validators.Bookings
{
    public class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
    {
        private readonly LocalSoundDbContext _context;
        
        public CancelBookingCommandValidator(LocalSoundDbContext context)
        {
            _context = context;

            RuleFor(m => m.AppUserId)
                .NotEmpty()
                .WithMessage("There was an error cancelling your booking, please try again...");

            RuleFor(m => m.MemberId) 
                .NotEmpty()
                .WithMessage("There was an error cancelling your booking, please try again...");

            RuleFor(m => m.BookingId)
                .NotEmpty()
                .WithMessage("There was an error cancelling your booking, please try again...");

            RuleFor(m => m)
                .MustAsync(async (m, x) => await BookingBelongsToUser(m))
                .WithMessage("There was an error cancelling your booking, please try again...");

            RuleFor(m => m)
                .MustAsync(async (m, x) => await BookingIsPendingOrUpcoming(m))
                .WithMessage("You can only cancel upcoming or pending bookings.");

        }

        private async Task<bool> BookingIsPendingOrUpcoming(CancelBookingCommand m)
        {
            return (await _context.ArtistBooking.FirstOrDefaultAsync(x => x.BookingId == m.BookingId && x.BookingDate >= new DateTime() && (x.BookingConfirmed == true || x.BookingConfirmed == null))) != null;
        }

        private async Task<bool> BookingBelongsToUser(CancelBookingCommand m)
        {
            return (await _context.ArtistBooking.FirstOrDefaultAsync(x => (x.BookerId == m.AppUserId || x.ArtistId == m.AppUserId) && x.BookingId == m.BookingId)) != null;
        }
    }
}
