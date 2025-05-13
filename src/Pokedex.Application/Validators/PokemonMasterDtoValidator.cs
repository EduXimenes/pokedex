using FluentValidation;
using Pokedex.Domain.Dto;

namespace Pokedex.Application.Validators
{
    public class PokemonMasterDtoValidator : AbstractValidator<PokemonMasterDto>
    {
        public PokemonMasterDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MinimumLength(3).WithMessage("Name must have at least 3 characters")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("A valid email address is required");

            RuleFor(x => x.Document)
                .NotEmpty().WithMessage("Document is required")
                .Matches(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$").WithMessage("Document must be in the format: 123.456.789-00");

            RuleFor(x => x.Age)
                .NotEmpty().WithMessage("Age is required")
                .GreaterThanOrEqualTo(10).WithMessage("Age must be at least 10 years old")
                .LessThanOrEqualTo(100).WithMessage("Age must not exceed 100 years");
        }
    }
} 