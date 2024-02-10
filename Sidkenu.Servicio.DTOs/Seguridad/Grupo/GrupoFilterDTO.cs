using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Seguridad.Grupo
{
    public class GrupoFilterDTO : FilterBaseDTO
    {
        public Guid EmpresaId { get; set; }
        public string CadenaBuscar { get; set; }
    }
}
