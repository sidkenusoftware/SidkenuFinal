using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Dominio.Entidades.Seguridad;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class TipoGasto : EntidadBase
    {
        // Propiedades
        public Guid? EmpresaId { get; set; }

        public string Codigo { get; set; }

        public string Descripcion { get; set; }


        // Propiedades de Navegacion
        public virtual Empresa Empresa { get; set; }
        public virtual List<Gasto> Gastos { get; set; }
        public virtual List<ComprobanteGasto> ComprobanteGastos { get; set; }
    }
}
