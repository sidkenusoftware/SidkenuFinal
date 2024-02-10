using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class CuentaCorrienteProveedor : EntidadBase
    {
        // Propiedades
        public Guid ComprobanteCompraId { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string Descripcion { get; set; }
        public TipoMovimiento TipoMovimiento { get; set; }
        public decimal Monto { get; set; }

        // Propiedades de Navegacion
        public virtual ComprobanteCompra ComprobanteCompra { get; set; }
    }
}
