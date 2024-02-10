using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Repositorio
{
    public interface IRepositoryGeneric<T> : IRepositoryGenericLectura<T>, IRepositoryGenericPersistencia<T> where T : EntidadBase
    {
    }
}
