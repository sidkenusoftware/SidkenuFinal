namespace Sidkenu.Servicio.DTOs.Core.MedioPago
{
    public class MedioPagoChequeDTO : MedioPagoDTO
    {
        public string NumeroCheque { get; set; }

        public Guid BancoId { get; set; }

        public DateTime FechaVencimiento { get; set; }
    }
}
