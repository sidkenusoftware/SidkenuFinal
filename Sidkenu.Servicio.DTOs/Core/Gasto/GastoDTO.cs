using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.Gasto
{
    public class GastoDTO : EntidadBaseDTO
    {
        // Propiedades de Navegacion
        public Guid CajaDetalleId { get; set; }
        public Guid GastoId { get; set; }

        public string Gasto { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }
    }
}
