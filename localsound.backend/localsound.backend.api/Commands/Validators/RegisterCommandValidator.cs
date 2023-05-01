using FluentValidation;
using localsound.backend.api.Commands.Account;
using localsound.backend.Domain.Enum;

namespace localsound.backend.api.Commands.Validators
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(m => m.RegistrationDetails)
                .NotEmpty();

            When(m => m.RegistrationDetails != null, () =>
            {
                RuleFor(m => m.RegistrationDetails.CustomerType)
                    .NotEmpty();

                When(m => m.RegistrationDetails.CustomerType == CustomerType.Artist, () =>
                {
                    RuleFor(m => m.RegistrationDetails.RegistrationDto)
                        .NotEmpty();

                    When(m => m.RegistrationDetails.RegistrationDto != null, () =>
                    {
                        RuleFor(m => m.RegistrationDetails.RegistrationDto.Email)
                            .NotEmpty();
                        RuleFor(m => m.RegistrationDetails.RegistrationDto.Password)
                            .NotEmpty();
                        RuleFor(m => m.RegistrationDetails.RegistrationDto.Address)
                            .NotEmpty();
                        RuleFor(m => m.RegistrationDetails.RegistrationDto.Name)
                            .NotEmpty();
                        RuleFor(m => m.RegistrationDetails.RegistrationDto.PhoneNumber)
                            .NotEmpty();
                        RuleFor(m => m.RegistrationDetails.RegistrationDto.SoundcloudUrl)
                            .NotEmpty();
                        RuleFor(m => m.RegistrationDetails.RegistrationDto.SpotifyUrl)
                            .NotEmpty();
                        RuleFor(m => m.RegistrationDetails.RegistrationDto.YoutubeUrl)
                            .NotEmpty();
                    });
                });

                When(m => m.RegistrationDetails.CustomerType == CustomerType.NonArtist, () =>
                {
                    RuleFor(m => m.RegistrationDetails.RegistrationDto)
                        .NotEmpty();

                    When(m => m.RegistrationDetails.RegistrationDto != null, () =>
                    {
                        RuleFor(m => m.RegistrationDetails.RegistrationDto.Email)
                            .NotEmpty();
                        RuleFor(m => m.RegistrationDetails.RegistrationDto.Password)
                            .NotEmpty();
                        RuleFor(m => m.RegistrationDetails.RegistrationDto.Address)
                            .NotEmpty();
                        RuleFor(m => m.RegistrationDetails.RegistrationDto.FirstName)
                            .NotEmpty();
                        RuleFor(m => m.RegistrationDetails.RegistrationDto.LastName)
                            .NotEmpty();
                        RuleFor(m => m.RegistrationDetails.RegistrationDto.PhoneNumber)
                            .NotEmpty();
                    });
                });
            });
        }
    }
}
