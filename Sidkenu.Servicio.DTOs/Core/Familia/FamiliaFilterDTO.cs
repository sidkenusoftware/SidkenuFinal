using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.Familia
{
    public class FamiliaFilterDTO : FilterBaseDTO
    {
        public string? CadenaBuscar { get; set; } = null;
    }
}