using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.Banco
{
    public class BancoFilterDTO : FilterBaseDTO
    {
        public string? CadenaBuscar { get; set; } = null;
    }
}