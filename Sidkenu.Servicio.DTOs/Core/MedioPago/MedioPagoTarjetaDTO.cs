namespace Sidkenu.Servicio.DTOs.Core.MedioPago
{
    public class MedioPagoTarjetaDTO : MedioPagoDTO
    {
        public Guid PlanTarjetaId { get; set; }
        public string NumeroCupon { get; set; }
    }
}
