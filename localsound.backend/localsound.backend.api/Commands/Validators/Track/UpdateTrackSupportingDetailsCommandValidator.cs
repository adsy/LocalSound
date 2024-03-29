﻿using FluentValidation;
using localsound.backend.api.Commands.Track;

namespace localsound.backend.api.Commands.Validators.Track
{
    public class UpdateTrackSupportingDetailsCommandValidator : AbstractValidator<UpdateTrackSupportingDetailsCommand>
    {
        public UpdateTrackSupportingDetailsCommandValidator()
        {
            RuleFor(m => m.UserId)
                .NotEmpty()
                .WithMessage("User ID cannot be blank/null/empty");

            RuleFor(m => m.MemberId)
                .NotEmpty()
                .WithMessage("Member ID cannot be blank/null/empty");

            RuleFor(m => m.TrackId)
                .NotEmpty()
                .WithMessage("Track ID cannot be null");

            RuleFor(m => m.TrackData)
                .NotEmpty()
                .WithMessage("Track data DTO cannot be null");
        }
    }
}
