using FluentValidation;
using localsound.backend.api.Queries.Genre;

namespace localsound.backend.api.Queries.Validators.Genre
{
    public class SearchGenreQueryValidator : AbstractValidator<SearchGenreQuery>
    {
        public SearchGenreQueryValidator()
        {
            RuleFor(m => m.Name)
                .NotEmpty()
                .WithMessage("There was an error while searching for the genre.");
        }
    }
}
