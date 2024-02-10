using FluentValidation;
using Sidkenu.Servicio.DTOs.Seguridad.TipoResponsabilidad;

namespace Sidkenu.Servicio.Validator.Seguridad
{
    public class TipoResponsabilidadValidator : AbstractValidator<TipoResponsabilidadPersistenciaDTO>
    {
        public TipoResponsabilidadValidator()
        {
            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(250).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");
        }
    }
}

