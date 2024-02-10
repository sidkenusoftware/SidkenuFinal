using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.ArticuloVariante;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IArticuloVarianteServicio
    {
        ResultDTO Add(List<ArticuloVarianteValorPersistenciaDTO> listaVariantes, Guid empresaId, string user);
    }
}
