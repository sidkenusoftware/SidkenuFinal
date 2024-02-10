using FluentValidation;
using Sidkenu.Servicio.DTOs.Seguridad.AsistenteCargaInicial;

namespace Sidkenu.Servicio.Validator.Seguridad.Asistente
{
    public class AsistenteEmpresaValidator : AbstractValidator<EmpresaAsistenteDTO>
    {
        public AsistenteEmpresaValidator()
        {
            RuleFor(x => x.Abreviatura)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .Length(0, 5).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.Descripcion)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(250).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.Codigo);

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

            RuleFor(x => x.Referente)
                .MaximumLength(250).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.Cuit)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(13).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.FechaInicioActividad);

            RuleFor(x => x.NroIngresoBruto)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(13).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.Logo).NotNull().WithMessage("La {PropertyName} no puede estar vacía");
        }
    }
}
