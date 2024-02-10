using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.CuentaCorrienteCliente
{
    public class CtaCteClienteDTO : EntidadBaseDTO
    {
        public Guid ClienteId { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime? FechaVencimiento { get; set; }
        public string NroComprobanteFactura { get; set; }
        public TipoMovimiento TipoMovimiento { get; set; }
        public decimal Monto { get; set; }
        public string? Observacion { get; set; }
    }
}
