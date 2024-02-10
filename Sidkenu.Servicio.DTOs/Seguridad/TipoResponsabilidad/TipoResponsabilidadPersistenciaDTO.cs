using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Seguridad.TipoResponsabilidad
{
    public class TipoResponsabilidadPersistenciaDTO : EntidadBaseDTO
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }
    }
}
