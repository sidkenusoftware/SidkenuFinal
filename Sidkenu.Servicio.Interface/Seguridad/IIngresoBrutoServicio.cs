using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.IngresoBruto;

namespace Sidkenu.Servicio.Interface.Seguridad
{
    public interface IIngresoBrutoServicio
    {
        ResultDTO Add(IngresoBrutoPersistenciaDTO entidad, string user);
        ResultDTO Update(IngresoBrutoPersistenciaDTO entidad, string user);
        ResultDTO Delete(IngresoBrutoDeleteDTO deleteDTO, string user);

        // ================================================================= //

        ResultDTO GetById(Guid id);
        ResultDTO GetByFilter(IngresoBrutoFilterDTO filter);
        ResultDTO GetAll();
    }
}
