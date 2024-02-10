using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Variante;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IVarianteServicio
    {
        ResultDTO Add(VariantePersistenciaDTO entidad, string user);
        ResultDTO Update(VariantePersistenciaDTO entidad, string user);
        ResultDTO Delete(VarianteDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(VarianteFilterDTO filter);
        ResultDTO GetAll(Guid empresaId);
        ResultDTO GetAll();
    }
}
