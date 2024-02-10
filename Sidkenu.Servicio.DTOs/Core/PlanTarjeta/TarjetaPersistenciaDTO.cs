using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.PlanTarjeta
{
    public class PlanTarjetaPersistenciaDTO : EntidadBaseDTO
    {
        public Guid TarjetaId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Alicuota { get; set; }
    }
}
