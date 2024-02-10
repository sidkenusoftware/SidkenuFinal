using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.CajaPuestoTrabajo
{
    public class CajaPuestoTrabajoFilterDTO : FilterBaseDTO
    {
        public Guid CajaId { get; set; }
        public string? CadenaBuscar { get; set; } = null;
    }
}