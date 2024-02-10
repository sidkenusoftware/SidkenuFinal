using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.OrdenFabricacion
{
    public class OrdenFabricacionPersistenciaDTO : EntidadBaseDTO
    {
        public Guid ArticuloId { get; set; }
        public Guid EmpresaId { get; set; }
        public Guid DepositoOrigenId { get; set; }
        public Guid DepositoDestinoId { get; set; }
        public decimal Cantidad { get; set; }
        public OrigenFabricacion OrigenFabricacion { get; set; }
        public DateTime? FechaFinalizacion { get; set; }
        public int NumeroOrden { get; set; }
        public bool ActulizarPrecioPublico { get; set; }
        public List<OrdenFabricacionDetalleDTO> Detalles { get; set; }
        public byte[] Foto { get; set; }
    }
}
