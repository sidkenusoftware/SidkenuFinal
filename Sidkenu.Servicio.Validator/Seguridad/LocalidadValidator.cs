using FluentValidation;
using Sidkenu.Servicio.DTOs.Seguridad.Localidad;

namespace Sidkenu.Servicio.Validator.Seguridad
{
    public class LocalidadValidator : AbstractValidator<LocalidadPersistenciaDTO>
    {
        public LocalidadValidator()
        {
            RuleFor(x => x.ProvinciaId);

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(250).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");
        }
    }
}
