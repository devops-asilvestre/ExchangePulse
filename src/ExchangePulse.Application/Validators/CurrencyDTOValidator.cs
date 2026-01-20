using FluentValidation;
using ExchangePulse.Application.DTOs;

namespace ExchangePulse.Application.Validators
{
    public class CurrencyDTOValidator : AbstractValidator<CurrencyDTO>
    {
        public CurrencyDTOValidator()
        {
            RuleFor(c => c.Code)
                .NotEmpty().WithMessage("O código da moeda é obrigatório.")
                .Length(3).WithMessage("O código da moeda deve ter exatamente 3 caracteres (ISO 4217).")
                .Matches("^[A-Z]{3}$").WithMessage("O código da moeda deve estar em letras maiúsculas (ex: USD, BRL).");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("O nome da moeda é obrigatório.")
                .MaximumLength(100).WithMessage("O nome da moeda deve ter no máximo 100 caracteres.");

            RuleFor(c => c.Country)
                .NotEmpty().WithMessage("O país é obrigatório.")
                .MaximumLength(100).WithMessage("O país deve ter no máximo 100 caracteres.");
        }
    }
}
