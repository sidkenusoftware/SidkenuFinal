using FluentValidation;
using Sidkenu.Servicio.DTOs.Core.ListaPrecio;

namespace Sidkenu.Servicio.Validator.Core
{
    public class ListaPrecioValidator : AbstractValidator<ListaPrecioPersistenciaDTO>
    {
        public ListaPrecioValidator()
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