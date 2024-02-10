using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Servicio.DTOs.Core.Articulo;

namespace Sidkenu.Dominio.Repositorio.Core.Articulo
{
    public interface IArticuloRepositorio<T> : IRepositoryGenericPersistencia<T> where T : EntidadBase
    {
        IEnumerable<ArticuloDTO> GetAll();
    }
}
