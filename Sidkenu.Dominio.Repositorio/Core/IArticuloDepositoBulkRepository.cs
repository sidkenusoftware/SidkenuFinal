using Sidkenu.Dominio.Entidades.Core;


namespace Sidkenu.Dominio.Repositorio.Core
{
    public interface IArticuloDepositoBulkRepository
    {
        void Add(List<ArticuloDeposito> articulosDepositos);
    }
}
