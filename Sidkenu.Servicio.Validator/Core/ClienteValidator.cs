using FluentValidation;
using Sidkenu.Servicio.DTOs.Core.Cliente;

namespace Sidkenu.Servicio.Validator.Core
{
    public class ClienteValidator : AbstractValidator<ClientePersistenciaDTO>
    {
        public ClienteValidator()
        {
            RuleFor(x => x.RazonSocial)
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

            RuleFor(x => x.Documento)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(13).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.FechaNacimiento);

            RuleFor(x => x.TipoResponsabilidadId);

            RuleFor(x => x.ActivarBonificacion);

            RuleFor(x => x.ActivarCuentaCorriente);
        }
    }
}
