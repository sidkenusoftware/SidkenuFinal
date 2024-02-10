using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Dominio.Entidades.Seguridad;

namespace Sidkenu.Dominio.Entidades.Core
{
    public class Deposito : EntidadBase
    {
        // Propiedades
        public Guid EmpresaId { get; set; }
        public Guid? PersonaId { get; set; }

        public string Abreviatura { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public TipoDeposito TipoDeposito { get; set; }
        public bool Predeterminado { get; set; }

        // Propiedades de Navegacion
        public virtual Empresa Empresa { get; set; }

        public virtual Persona Persona { get; set; }

        public virtual List<ArticuloDeposito> ArticuloDepositos { get; set; }

        public virtual List<ConfiguracionCore> ConfiguracionCoreDepositoPorDefectoParaVentas { get; set; }
        public virtual List<ConfiguracionCore> ConfiguracionCoreDepositoPorDefectoParaCompras { get; set; }

        public virtual List<OrdenFabricacion> OrdenFabricacionOrigenes { get; set; }
        public virtual List<OrdenFabricacion> OrdenFabricacionDestinos { get; set; }
    }
}
