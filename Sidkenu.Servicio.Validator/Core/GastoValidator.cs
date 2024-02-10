using FluentValidation;
using Sidkenu.Servicio.DTOs.Core.Gasto;

namespace Sidkenu.Servicio.Validator.Core
{
    public class GastoValidator : AbstractValidator<GastosPersistenciaDTO>
    {
        public GastoValidator()
        {
            RuleFor(x => x.TipoGastoId);

            RuleFor(x => x.Monto)
                .NotNull().WithMessage("El {PropertyName} es obligatorio");

            RuleFor(x => x.Fecha)
                .NotNull().WithMessage("La {PropertyName} es obligatoria");

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(500).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");
        }
    }
}