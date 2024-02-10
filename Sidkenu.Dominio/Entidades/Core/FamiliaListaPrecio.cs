using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Dominio.Entidades.Seguridad;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class FamiliaListaPrecio : EntidadBase
    {
        // Propiedades
        public Guid? EmpresaId { get; set; }
        public Guid FamiliaId { get; set; }
        public Guid ListaPrecioId { get; set; }
        public TipoValor TipoValor { get; set; }
        public decimal Valor { get; set; }


        // Propiedades de Navegacion
        public virtual Familia Familia { get; set; }
        public virtual ListaPrecio ListaPrecio { get; set; }
        public virtual Empresa Empresa { get; set; }
    }
}
