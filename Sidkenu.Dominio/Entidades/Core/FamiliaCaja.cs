using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class FamiliaCaja : EntidadBase
    {
        // Propiedades
        public Guid FamiliaId { get; set; }
        public Guid CajaId { get; set; }

        // Propiedades de Navegacion
        public virtual Familia Familia { get; set; }
        public virtual Caja Caja { get; set; }
    }
}
