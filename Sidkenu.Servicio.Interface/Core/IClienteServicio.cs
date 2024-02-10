using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Cliente;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IClienteServicio
    {
        ResultDTO Add(ClientePersistenciaDTO entidad, string user);
        ResultDTO Update(ClientePersistenciaDTO entidad, string user);
        ResultDTO Delete(ClienteDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(ClienteFilterDTO filter);
        ResultDTO GetByFilterLookUp(ClienteFilterDTO filter);
        ResultDTO GetAll();
        ResultDTO GetConsumidorFinal();
        ResultDTO GetByNumeroDocumento(string numeroDocumento);
    }
}
