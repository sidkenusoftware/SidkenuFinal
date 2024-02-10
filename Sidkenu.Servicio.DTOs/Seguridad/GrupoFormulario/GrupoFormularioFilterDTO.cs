using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Seguridad.GrupoFormulario
{
    public class GrupoFormularioFilterDTO : FilterBaseDTO
    {
        public Guid GrupoId { get; set; }
        public string? CadenaBuscar { get; set; } = null;
    }
}