using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.ListaPrecio
{
    public class ListaPrecioPersistenciaDTO : EntidadBaseDTO
    {
        public Guid EmpresaId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        public decimal Rentabilidad { get; set; }
    }
}
