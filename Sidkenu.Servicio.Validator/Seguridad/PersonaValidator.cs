using FluentValidation;
using Sidkenu.Servicio.DTOs.Seguridad.Persona;

namespace Sidkenu.Servicio.Validator.Seguridad
{
    public class PersonaValidator : AbstractValidator<PersonaPersistenciaDTO>
    {
        public PersonaValidator()
        {
            RuleFor(x => x.Apellido)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(250).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(250).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.Direccion)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(250).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.Telefono)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(25).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.Mail)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(250).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.")
                .EmailAddress().WithMessage("El {PropertyName} no es una direccion de correo valida.");

            RuleFor(x => x.Cuil)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(13).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.FechaNacimiento);

            RuleFor(x => x.Foto).NotNull().WithMessage("La {PropertyName} no puede estar vacía");
        }
    }
}
