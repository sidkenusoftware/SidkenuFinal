using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.ArticuloFormula;

namespace Sidkenu.Servicio.Interface.Core
{
    public interface IArticuloFormulaServicio
    {
        ResultDTO Add(ArticuloFormulaPersistenciaDTO articuloKit, string user);
    }
}
