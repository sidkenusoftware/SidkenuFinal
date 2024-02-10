using FluentValidation;
using Sidkenu.Servicio.DTOs.Core.Deposito;

namespace Sidkenu.Servicio.Validator.Core
{
    public class DepositoValidator : AbstractValidator<DepositoPersistenciaDTO>
    {
        public DepositoValidator()
        {
            RuleFor(x => x.EmpresaId);

            RuleFor(x => x.Abreviatura)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(10).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(250).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.Direccion)
                .MaximumLength(250).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.TipoDeposito);
        }
    }
}