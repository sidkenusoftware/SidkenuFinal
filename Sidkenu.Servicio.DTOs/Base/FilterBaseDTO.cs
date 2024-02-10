namespace Sidkenu.Servicio.DTOs.Base
{
    public class FilterBaseDTO
    {
        public Guid EmpresaId { get; set; }

        public bool VerEliminados { get; set; } = false;
    }
}
