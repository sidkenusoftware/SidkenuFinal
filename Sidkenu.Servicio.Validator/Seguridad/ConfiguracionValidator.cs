using FluentValidation;
using Sidkenu.Servicio.DTOs.Seguridad.Configuracion;

namespace Sidkenu.Servicio.Validator.Seguridad
{
    public class ConfiguracionValidator : AbstractValidator<ConfiguracionPersistenciaDTO>
    {
        public ConfiguracionValidator()
        {
            RuleFor(x => x.LoginNormal).NotNull();

            RuleFor(x => x.UsuarioCredencial)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(50).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.PasswordCredencial)
                .NotEmpty().WithMessage("La {PropertyName} no puede estar vacía")
                .MaximumLength(250).WithMessage("La {PropertyName} no pude ser mayor a {MaxLength} caracteres.");

            RuleFor(x => x.Puerto).NotNull();

            RuleFor(x => x.Host).NotNull();

            RuleFor(x => x.LogError).NotNull();

            RuleFor(x => x.LogInformacion).NotNull();

            RuleFor(x => x.EnviarMailCreateUsuario).NotNull();
        }
    }
}