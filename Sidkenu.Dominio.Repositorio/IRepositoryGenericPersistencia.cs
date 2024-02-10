using Sidkenu.Dominio.Entidades.Base;

namespace Sidkenu.Dominio.Repositorio
{
    public interface IRepositoryGenericPersistencia<T> where T : EntidadBase
    {
        // Persistencia
        void Add(T entity);

        void Update(T entity);

        void Delete(Guid id, string userLogin);

        void DeleteFisico(Guid id, string userLogin);
    }
}
