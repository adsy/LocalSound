using FluentValidation;
using localsound.backend.api.Queries.Bookings;

namespace localsound.backend.api.Queries.Validators.Bookings
{
    public class GetCompletedBookingsQueryValidator : AbstractValidator<GetCompletedBookingsQuery>
    {
        public GetCompletedBookingsQueryValidator()
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
        }
    }
}
