using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Seguridad.GrupoPersona
{
    public class GrupoPersonaFilterDTO : FilterBaseDTO
    {
        public Guid GrupoId { get; set; }
        public string? CadenaBuscar { get; set; } = null;
    }
}