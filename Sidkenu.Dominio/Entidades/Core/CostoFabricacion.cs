using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Dominio.Entidades.Seguridad;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class CostoFabricacion : EntidadBase
    {
        public Guid EmpresaId { get; set; }                
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }


        // Propiedades de Navegacion
        public virtual Empresa Empresa { get; set; }
    }
}
