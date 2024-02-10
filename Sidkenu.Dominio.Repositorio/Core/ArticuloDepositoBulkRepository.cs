using Microsoft.EntityFrameworkCore;
using Sidkenu.Dominio.Entidades.Core;
using EFCore.BulkExtensions;

namespace Sidkenu.Dominio.Repositorio.Core
{
    public class ArticuloDepositoBulkRepository : IArticuloDepositoBulkRepository
    {
        private DbContext _context;
        public ArticuloDepositoBulkRepository(DbContext context)
        {
            _context = context;
        }

        public void Add(List<ArticuloDeposito> articulosDepositos)
        {
            _context.BulkInsert(articulosDepositos);
        }
    }
}
