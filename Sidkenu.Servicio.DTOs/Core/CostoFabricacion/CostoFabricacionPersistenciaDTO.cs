using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.CostoFabricacion
{
    public class CostoFabricacionPersistenciaDTO : EntidadBaseDTO
    {
        public Guid? EmpresaId { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
    }
}
