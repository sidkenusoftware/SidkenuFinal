using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Seguridad.Localidad
{
    public class LocalidadFilterDTO : FilterBaseDTO
    {
        public Guid? ProvinciaId { get; set; }
        public string CadenaBuscar { get; set; }
    }
}
