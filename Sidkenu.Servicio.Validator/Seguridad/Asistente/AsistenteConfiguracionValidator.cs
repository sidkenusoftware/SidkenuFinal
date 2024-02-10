using FluentValidation;
using Sidkenu.Servicio.DTOs.Seguridad.AsistenteCargaInicial;

namespace Sidkenu.Servicio.Validator.Seguridad.Asistente
{
    public class AsistenteConfiguracionValidator : AbstractValidator<ConfiguracionAsistenteDTO>
    {
        public AsistenteConfiguracionValidator()
        {
            RuleFor(x => x.LoginNormal).NotNull();
        }
    }
}