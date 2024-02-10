using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class CuentaCorrienteCliente : EntidadBase
    {
        // Propiedades
        public Guid ClienteId { get; set; }

        public DateTime Fecha { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string NroComprobanteFactura { get; set; }
        public TipoMovimiento TipoMovimiento { get; set; }
        public decimal Monto { get; set; }
        public string? Observacion { get; set; }


        // Propiedades de Navegacion
        public virtual Cliente Cliente { get; set; }
    }
}
