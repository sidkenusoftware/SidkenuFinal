using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Servicio.DTOs.Base;

namespace Sidkenu.Servicio.DTOs.Core.Deposito
{
    public class DepositoPersistenciaDTO : EntidadBaseDTO
    {
        public Guid EmpresaId { get; set; }
        public Guid? PersonaId { get; set; }

        public string Abreviatura { get; set; }
        public string Descripcion { get; set; }
        public string Direccion { get; set; }
        public TipoDeposito TipoDeposito { get; set; }
    }
}
