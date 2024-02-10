using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Seguridad.Localidad
{
    public class LocalidadDTO : EntidadBaseDTO
    {
        public Guid ProvinciaId { get; set; }
        public string Provincia { get; set; }
        public string Descripcion { get; set; }
    }
}
