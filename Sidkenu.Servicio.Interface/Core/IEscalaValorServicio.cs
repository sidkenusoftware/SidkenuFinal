using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Variante;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IVarianteValorServicio
    {
        ResultDTO Add(ValorVariantePersistenciaDTO entidad, string user);
        ResultDTO Delete(ValorVarianteDeleteDTO deleteDTO, string user);
        ResultDTO GetAll(Guid escalaId);
    }
}
