using FluentValidation;
using Sidkenu.Servicio.DTOs.Seguridad.Provincia;

namespace Sidkenu.Servicio.Validator.Seguridad
{
    public class ProvinciaValidator : AbstractValidator<ProvinciaPersistenciaDTO>
    {
        public ProvinciaValidator()
        {
            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(250).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");
        }
    }
}
