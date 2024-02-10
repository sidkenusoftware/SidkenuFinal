using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Seguridad.Grupo
{
    public class GrupoPersistenciaDTO : EntidadBaseDTO
    {
        public Guid EmpresaId { get; set; }
        public string Descripcion { get; set; }
    }
}
