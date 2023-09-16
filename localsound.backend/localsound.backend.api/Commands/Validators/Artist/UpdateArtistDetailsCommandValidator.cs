using FluentValidation;
using localsound.backend.api.Commands.Artist;

namespace localsound.backend.api.Commands.Validators.Artist
{
    public class UpdateArtistDetailsCommandValidator:AbstractValidator<UpdateArtistPersonalDetailsCommand>
    {
        public UpdateArtistDetailsCommandValidator()
        {
            RuleFor(m => m.UserId)
                .NotEmpty()
                .WithMessage("User ID cannot be blank/null/empty");

            RuleFor(m => m.MemberId)
                .NotEmpty()
                .WithMessage("Member ID cannot be blank/null/empty");

            RuleFor(m => m.UpdateArtistDto)
                .NotEmpty()
                .WithMessage("Update artist DTO cannot be null");

            When(x => x.UpdateArtistDto != null, () =>
            {
                RuleFor(m => m.UpdateArtistDto.Name)
                .NotEmpty()
                .WithMessage("Artist name cannot be blank");

                RuleFor(m => m.UpdateArtistDto.Address)
                .NotEmpty()
                .WithMessage("Address cannot be blank/null/empty");

                RuleFor(m => m.UpdateArtistDto.PhoneNumber)
                .NotEmpty()
                .WithMessage("Mobile number cannot be blank/null/empty");
            });
        }
    }
}
