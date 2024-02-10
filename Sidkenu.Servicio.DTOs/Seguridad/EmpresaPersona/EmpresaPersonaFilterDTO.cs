using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Seguridad.EmpresaPersona
{
    public class EmpresaPersonaFilterDTO : FilterBaseDTO
    {
        public Guid EmpresaId { get; set; }
        public string? CadenaBuscar { get; set; } = null;
    }
}