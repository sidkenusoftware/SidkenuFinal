using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Seguridad.PuestoTrabajo
{
    public class PuestoTrabajoPersistenciaDTO : EntidadBaseDTO
    {
        public Guid EmpresaId { get; set; }
        public string Descripcion { get; set; }
        public string SerialNumber { get; set; }
    }
}
