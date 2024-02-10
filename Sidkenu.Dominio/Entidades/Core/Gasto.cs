using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Dominio.Entidades.Seguridad;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class Gasto : EntidadBase
    {
        public Guid EmpresaId { get; set; }
        public Guid TipoGastoId { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Descripcion { get; set; }


        // Propiedades de Navegacion
        public virtual Empresa Empresa { get; set; }
        public virtual TipoGasto TipoGasto { get; set; }
    }
}
