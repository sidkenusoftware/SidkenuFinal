using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.TipoGasto
{
    public class TipoGastoPersistenciaDTO : EntidadBaseDTO
    {
        public Guid? EmpresaId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
    }
}
