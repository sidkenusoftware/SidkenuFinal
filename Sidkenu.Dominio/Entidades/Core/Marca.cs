using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Dominio.Entidades.Seguridad;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class Marca : EntidadBase
    {
        // Propiedades
        public Guid? EmpresaId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        // =========================================================== //
        public bool ActivarAumentoPrecioPublico { get; set; }
        public decimal? AumentoPrecioPublico { get; set; }
        public TipoValor? TipoValorPublico { get; set; }

        // ================================================================= //
        public bool ActivarAumentoPrecioPublicoListaPrecio { get; set; }
        public decimal? AumentoPrecioPublicoListaPrecio { get; set; }
        public TipoValor? TipoValorPublicoListaPrecio { get; set; }

        // ================================================================= //

        // Propiedades de Navegacion
        public virtual List<Articulo> Articulos { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual List<MarcaListaPrecio> MarcaListaPrecios { get; set; }
    }
}