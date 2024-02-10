using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Seguridad.PuestoTrabajo
{
    public class PuestoTrabajoDTO : EntidadBaseDTO
    {
        public Guid EmpresaId { get; set; }

        public string Descripcion { get; set; }

        public string SerialNumber { get; set; }

        public bool ExisteBase { get; set; }
    }
}
