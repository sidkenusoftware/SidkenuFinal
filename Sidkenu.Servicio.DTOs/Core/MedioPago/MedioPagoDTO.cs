using Sidkenu.Aplicacion.Comun;
using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.MedioPago
{
    public class MedioPagoDTO : EntidadBaseDTO
    {
        public TipoMedioDePago Tipo { get; set; }
        public string TipoMedioDePagoStr => EnumDescription.Get(Tipo);
        public decimal Capital { get; set; }
        public decimal Interes { get; set; }
    }
}
