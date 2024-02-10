using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Cliente;
using Sidkenu.Servicio.DTOs.Seguridad.Persona;

namespace Sidkenu.Servicio.DTOs.Core.Gasto
{
    public class GastosPersistenciaDTO : EntidadBaseDTO
    {
        public Guid EmpresaId { get; set; }
        public Guid CajaDetalleId { get; set; }
        public Guid CajaId { get; set; }
        public Guid TipoGastoId { get; set; }
        public Guid PersonaId { get; set; }

        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }
    }
}
