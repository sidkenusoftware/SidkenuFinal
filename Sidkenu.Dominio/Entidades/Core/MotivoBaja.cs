using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Dominio.Entidades.Seguridad;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class MotivoBaja : EntidadBase
    {
        // Propiedad
        public Guid? EmpresaId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }


        // Propiedad de Navegacion
        public virtual List<ArticuloBaja> ArticuloBajas { get; set; }
        public virtual Empresa Empresa { get; set; }
    }
}