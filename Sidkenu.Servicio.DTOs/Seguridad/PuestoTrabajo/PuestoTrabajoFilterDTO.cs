using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Seguridad.PuestoTrabajo
{
    public class PuestoTrabajoFilterDTO : FilterBaseDTO
    {
        public Guid EmpresaId { get; set; }
        public string CadenaBuscar { get; set; }
    }
}
