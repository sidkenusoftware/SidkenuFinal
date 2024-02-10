using FluentValidation;
using Sidkenu.Servicio.DTOs.Seguridad.Grupo;

namespace Sidkenu.Servicio.Validator.Seguridad
{
    public class GrupoValidator : AbstractValidator<GrupoPersistenciaDTO>
    {
        public GrupoValidator()
        {
            RuleFor(x => x.EmpresaId).NotNull();

            RuleFor(x => x.Descripcion)
                .NotNull()
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(250).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");
        }
    }
}

