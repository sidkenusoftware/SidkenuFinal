using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Seguridad.Modulo
{
    public class ModuloPersistenciaDTO : EntidadBaseDTO
    {
        public Guid EmpresaId { get; set; }

        public bool Seguridad { get; set; }

        public bool Venta { get; set; }

        public bool Compra { get; set; }

        public bool Inventario { get; set; }

        public bool Fabricacion { get; set; }

        public bool PuntoVenta { get; set; }

        public bool Caja { get; set; }

        public bool DashBoard { get; set; }
    }
}
