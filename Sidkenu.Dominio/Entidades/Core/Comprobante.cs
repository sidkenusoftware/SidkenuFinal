using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Dominio.Entidades.Seguridad;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class Comprobante : EntidadBase
    {
        public Guid EmpresaId { get; set; }
        public Guid PersonaId { get; set; }
        public Guid ClienteId { get; set; }

        public DateTime Fecha { get; set; }
        public string? Numero { get; set; }        
        public decimal SubTotal { get; set; }
        public decimal Descuento { get; set; }
        public decimal Total { get; set; }

        public TipoComprobante TipoComprobante { get; set; }
        public EstadoComprobante EstadoComprobante { get; set; }

        // Propiedades de Navegacion
        public virtual Empresa Empresa { get; set; }
        public virtual Persona Persona { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual List<ComprobanteTotales> Totales { get; set; }
        public virtual List<ComprobanteDetalle> Detalles { get; set; }
        public virtual List<MedioPago> MedioPagos { get; set; }
        public virtual List<MovimientoCaja> Movimientos { get; set; }
    }
}
