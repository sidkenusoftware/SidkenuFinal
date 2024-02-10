using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.Marca
{
    public class MarcaDTO : EntidadBaseDTO
    {
        public Guid? EmpresaId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        // ================================================================= //

        public bool ActivarAumentoPrecioPublico { get; set; }
        public decimal? AumentoPrecioPublico { get; set; }
        public TipoValor? TipoValorPublico { get; set; }

        // ================================================================= //

        public bool ActivarAumentoPrecioPublicoListaPrecio { get; set; }
        public decimal? AumentoPrecioPublicoListaPrecio { get; set; }
        public TipoValor? TipoValorPublicoListaPrecio { get; set; }
    }
}
