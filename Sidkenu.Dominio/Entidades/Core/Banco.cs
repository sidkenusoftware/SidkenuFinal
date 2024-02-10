using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Dominio.Entidades.Seguridad;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class Banco : EntidadBase
    {
        // Propiedades
        public Guid? EmpresaId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }


        // Propiedades de Navegacion
        public Empresa Empresa { get; set; }
        public List<MedioPagoTransferencia> Transferencias { get; set; }
        public List<MedioPagoCheque> Cheques { get; set; }
    }
}
