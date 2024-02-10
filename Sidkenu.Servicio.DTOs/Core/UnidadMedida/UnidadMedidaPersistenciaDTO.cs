using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.UnidadMedida
{
    public class UnidadMedidaPersistenciaDTO : EntidadBaseDTO
    {
        public Guid? EmpresaId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public decimal Equivalencia { get; set; }
    }
}
