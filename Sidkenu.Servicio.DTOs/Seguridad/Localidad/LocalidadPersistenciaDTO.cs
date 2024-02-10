using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Seguridad.Localidad
{
    public class LocalidadPersistenciaDTO : EntidadBaseDTO
    {
        public Guid ProvinciaId { get; set; }
        public string Descripcion { get; set; }
    }
}
