using Sidkenu.Aplicacion.Comun;
using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.Movimiento
{
    public class MovimientoCajaDTO : EntidadBaseDTO
    {
        // Propiedades de Navegacion
        public Guid CajaDetalleId { get; set; }

        public Guid ComprobanteId { get; set; }

        public decimal Capital { get; set; }

        public decimal Interes { get; set; }

        public DateTime Fecha { get; set; }

        public string FechaStr => Fecha.ToShortDateString();

        public string HoraStr => Fecha.ToShortTimeString();

        public TipoOperacionMovimiento TipoOperacion { get; set; }
        public string TipoOperacionStr => EnumDescription.Get(TipoOperacion);

        public string Descripcion { get; set; }

        public TipoMovimiento TipoMovimiento { get; set; }
    }
}
