using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.CondicionIva;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface ICondicionIvaServicio
    {
        ResultDTO Add(CondicionIvaPersistenciaDTO entidad, string user);
        ResultDTO Update(CondicionIvaPersistenciaDTO entidad, string user);
        ResultDTO Delete(CondicionIvaDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(CondicionIvaFilterDTO filter);
        ResultDTO GetAll(Guid empresaId);
        ResultDTO GetAll();
    }
}
