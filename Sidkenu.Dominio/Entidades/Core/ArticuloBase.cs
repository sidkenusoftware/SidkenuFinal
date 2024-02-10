using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Dominio.Entidades.Seguridad;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class ArticuloBase : EntidadBase
    {
        // Propiedades
        public Guid? EmpresaId { get; set; }

        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public byte[] Foto { get; set; }

        // Propiedades de Navegacion
        public virtual Empresa Empresa { get; set; }
        public virtual List<OrdenFabricacion> OrdenFabricaciones { get; set; }
    }
}
