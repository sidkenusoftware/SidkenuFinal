using FluentValidation;
using Sidkenu.Servicio.DTOs.Core.PlanTarjeta;

namespace Sidkenu.Servicio.Validator.Core
{
    public class PlanTarjetaValidator : AbstractValidator<PlanTarjetaPersistenciaDTO>
    {
        public PlanTarjetaValidator()
        {
            RuleFor(x => x.TarjetaId);

            RuleFor(x => x.Codigo)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(10).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(250).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.Alicuota);
        }
    }
}