using FluentValidation;
using Sidkenu.Servicio.DTOs.Core.TipoGasto;

namespace Sidkenu.Servicio.Validator.Core
{
    public class TipoGastoValidator : AbstractValidator<TipoGastoPersistenciaDTO>
    {
        public TipoGastoValidator()
        {
            RuleFor(x => x.Codigo)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(10).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(250).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");
        }
    }
}