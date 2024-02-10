using FluentValidation;
using Sidkenu.Servicio.DTOs.Seguridad.IngresoBruto;

namespace Sidkenu.Servicio.Validator.Seguridad
{
    public class IngresoBrutoValidator : AbstractValidator<IngresoBrutoPersistenciaDTO>
    {
        public IngresoBrutoValidator()
        {
            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(250).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");
        }
    }
}
