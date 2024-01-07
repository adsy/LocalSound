using FluentValidation;
using localsound.backend.api.Queries.Bookings;

namespace localsound.backend.api.Queries.Validators.Bookings
{
    public class GetUserBookingsQueryValidator : AbstractValidator<GetUserBookingsQuery>
    {
        public GetUserBookingsQueryValidator()
        {
            RuleFor(m => m.AppUserId)
                .NotEmpty()
                .WithMessage("An error occured getting your bookings, please try again...");

            RuleFor(m => m.MemberId) 
                .NotEmpty()
                .WithMessage("An error occured getting your bookings, please try again...");
            
            RuleFor(m => m.Page)
                .NotEmpty()
                .WithMessage("An error occured getting your bookings, please try again...");

            RuleFor(m => m.BookingConfirmed)
                .NotEmpty()
                .WithMessage("An error occured getting your bookings, please try again...");
        }
    }
}
