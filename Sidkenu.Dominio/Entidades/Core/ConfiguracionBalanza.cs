using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Dominio.Entidades.Seguridad;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class ConfiguracionBalanza : EntidadBase
    {
        // Propiedades
        public Guid EmpresaId { get; set; }
        
        public int CodigoIdentificarImporte { get; set; }
        public int DecimalesImporte { get; set; }

        public int CodigoIdentificarPeso { get; set; }
        public int DecimalPeso { get; set; }

        public bool ConvierteUnidadPeso { get; set; }
        public decimal Equivalencia { get; set; }

        public int LongitudTotal { get; set; }
        public int InicioIdentificarTipo { get; set; }
        public int CantidadIdentificarTipo { get; set; }
        public int InicioIdentificarCodigoArcitulo { get; set; }
        public int CantidadIdentificarCodigoArcitulo { get; set; }
        public int InicioIdentificarImportePrecio { get; set; }
        public int CantidadIdentificarImportePrecio { get; set; }

        // Propiedades de Navegacion
        public Empresa Empresa { get; set; }
    }
}
