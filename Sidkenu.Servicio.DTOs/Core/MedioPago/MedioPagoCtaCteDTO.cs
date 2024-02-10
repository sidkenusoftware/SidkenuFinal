using Sidkenu.Servicio.DTOs.Core.Cliente;

namespace Sidkenu.Servicio.DTOs.Core.MedioPago
{
    public class MedioPagoCtaCteDTO : MedioPagoDTO
    {
        public ClienteDTO Cliente { get; set; }
    }
}
