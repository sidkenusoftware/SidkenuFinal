using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Banco;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IBancoServicio
    {
        ResultDTO Add(BancoPersistenciaDTO entidad, string user);
        ResultDTO Update(BancoPersistenciaDTO entidad, string user);
        ResultDTO Delete(BancoDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(BancoFilterDTO filter);
        ResultDTO GetAll(Guid empresaId);
        ResultDTO GetAll();
    }
}
