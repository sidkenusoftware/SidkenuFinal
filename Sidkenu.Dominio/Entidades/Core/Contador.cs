using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Dominio.Entidades.Seguridad;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class Contador : EntidadBase
    {
        public Guid EmpresaId { get; set; }
        public TipoContador TipoContador { get; set; }
        public long Numero { get; set; }

        // Propiedades de Navegacion
        public virtual Empresa Empresa { get; set; }
    }
}
