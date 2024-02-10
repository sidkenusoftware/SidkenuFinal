using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Deposito;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IDepositoServicio
    {
        ResultDTO Add(DepositoPersistenciaDTO entidad, string user);
        ResultDTO Update(DepositoPersistenciaDTO entidad, string user);
        ResultDTO Delete(DepositoDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(DepositoFilterDTO filter);
        ResultDTO GetAll(Guid empresaId);
        ResultDTO GetAll();

        // ================================================================= //

        ResultDTO MarcarComoPredeterminado(Guid depositoId, Guid empresaId, TipoDeposito tipoDeposito);
    }
}
