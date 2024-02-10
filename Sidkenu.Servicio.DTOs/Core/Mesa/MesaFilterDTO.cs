using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.Mesa
{
    public class MesaFilterDTO : FilterBaseDTO
    {
        public Guid? SalonId { get; set; }
        public string? CadenaBuscar { get; set; } = null;
    }
}