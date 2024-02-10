using FluentValidation;
using Sidkenu.Servicio.DTOs.Seguridad.Modulo;

namespace Sidkenu.Servicio.Validator.Seguridad
{
    public class ModuloValidator : AbstractValidator<ModuloPersistenciaDTO>
    {
        public ModuloValidator()
        {
            RuleFor(x => x.EmpresaId).NotNull().WithMessage("La {PropertyName} no puede estar vacía");

            RuleFor(x => x.Seguridad).NotNull().WithMessage("La {PropertyName} no puede estar vacía");

            RuleFor(x => x.Compra).NotNull().WithMessage("La {PropertyName} no puede estar vacía");

            RuleFor(x => x.Compra).NotNull().WithMessage("La {PropertyName} no puede estar vacía");

            RuleFor(x => x.Fabricacion).NotNull().WithMessage("La {PropertyName} no puede estar vacía");

            RuleFor(x => x.Inventario).NotNull().WithMessage("La {PropertyName} no puede estar vacía");

            RuleFor(x => x.PuntoVenta).NotNull().WithMessage("La {PropertyName} no puede estar vacía");

            RuleFor(x => x.Venta).NotNull().WithMessage("La {PropertyName} no puede estar vacía");
        }
    }
}