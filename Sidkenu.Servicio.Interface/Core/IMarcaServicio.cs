using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Marca;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IMarcaServicio
    {
        ResultDTO Add(MarcaPersistenciaDTO entidad, string user);
        ResultDTO Update(MarcaPersistenciaDTO entidad, bool actualizarPrecio,  string user);
        ResultDTO Delete(MarcaDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(MarcaFilterDTO filter);
        ResultDTO GetAll(Guid empresaId);
        ResultDTO GetAll();
    }
}
