using Sidkenu.Dominio.Entidades.Base;
using Sidkenu.Infraestructura;
using Sidkenu.Servicio.DTOs.Core.Articulo;

namespace Sidkenu.Dominio.Repositorio.Core.Articulo
{
    public class ArticuloRepositorio<T> : RepositoryGenericPersistencia<T>, IArticuloRepositorio<T> where T : EntidadBase
    {
        private readonly DataContext _dataContext;

        public ArticuloRepositorio(DataContext dataContext) 
            : base(dataContext)
        {
            this._dataContext = dataContext;
        }

        public IEnumerable<ArticuloDTO> GetAll()
        {
            var query2 = from art in _dataContext.Articulos
                        join artConc in _dataContext.ArticuloConcentradores on art.Id equals artConc.ArticuloId
                        join artProv in _dataContext.ArticuloConcentradorProveedores on artConc.Id equals artProv.ArticuloConcentradorId
                        join conIva in _dataContext.CondicionIvas on art.CondicionIvaId equals conIva.Id
                        join fam in _dataContext.Familias on art.FamiliaId equals fam.Id
                        join marc in _dataContext.Marcas on art.MarcaId equals marc.Id
                        join umv in _dataContext.UnidadMedidas on art.UnidadMedidaVentaId equals umv.Id
                        join umc in _dataContext.UnidadMedidas on art.UnidadMedidaCompraId equals umc.Id
                        select new ArticuloDTO
                        {
                            Descripcion = art.Descripcion,
                            Abreviatura = art.Abreviatura,
                            ActivarBonificacion = art.ActivarBonificacion,
                            ActivarLimiteVenta = art.ActivarLimiteVenta,
                            ArticuloId = art.Id,
                            Id = artConc.Id,
                        };

            var query = from art in _dataContext.Articulos
                        join artConc in _dataContext.ArticuloConcentradores on art.Id equals artConc.ArticuloId
                        join artProv in _dataContext.ArticuloConcentradorProveedores on artConc.Id equals artProv.ArticuloConcentradorId
                        join conIva in _dataContext.CondicionIvas on art.CondicionIvaId equals conIva.Id
                        join fam in _dataContext.Familias on art.FamiliaId equals fam.Id
                        join marc in _dataContext.Marcas on art.MarcaId equals marc.Id
                        join umv in _dataContext.UnidadMedidas on art.UnidadMedidaVentaId equals umv.Id
                        join umc in _dataContext.UnidadMedidas on art.UnidadMedidaCompraId equals umc.Id
                        join escVal1 in _dataContext.EscalaValores on artConc.EscalaValorUnoId equals escVal1.Id into escalaValor1 from e1 in escalaValor1.DefaultIfEmpty()
                        join escVal2 in _dataContext.EscalaValores on artConc.EscalaValorDosId equals escVal2.Id into escalaValor2 from e2 in escalaValor2.DefaultIfEmpty()
                        select new ArticuloDTO
                        {
                            Descripcion = art.Descripcion,
                            Abreviatura = art.Abreviatura,
                            ActivarBonificacion = art.ActivarBonificacion,
                            ActivarLimiteVenta = art.ActivarLimiteVenta,
                            ArticuloId = art.Id,
                            Id = artConc.Id,
                            Bonificacion = art.Bonificacion,
                            BonificacionFechaDesde = art.BonificacionFechaDesde,
                            BonificacionFechaHasta = art.BonificacionFechaHasta,
                            Codigo = art.Codigo,
                            CodigoBarra = art.CodigoBarra,
                            Comision = art.Comision,
                            CondicionIva = conIva.Descripcion,
                            CondicionIvaId = conIva.Id,
                            DescripcionAdicional = art.DescripcionAdicional,
                            Detalle = art.Detalle,
                            EmpresaId = art.EmpresaId,
                            EstaBloqueado = art.EstaBloqueado,
                            EstaEliminado = art.EstaEliminado && artConc.EstaEliminado,
                            FamiliaId = art.FamiliaId,
                            Familia = fam.Descripcion,
                            MarcaId = art.MarcaId,
                            Marca = marc.Descripcion,
                            Foto = art.Foto,
                            LimiteVenta = art.LimiteVenta,
                            Garantia = art.Garantia,
                            PerfilArticulo = art.PerfilArticulo,
                            PermiteStockNegativo = art.PermiteStockNegativo,
                            PorcentajePerdida = art.PorcentajePerdida,
                            PuntoPedido = art.PuntoPedido,
                            StockMaximo = art.StockMaximo,
                            StockMinimo = art.StockMinimo,
                            TieneGarantia = art.TieneGarantia,
                            TienePerdida = art.TienePerdida,
                            TipoArticulo = artConc.TipoArticulo,
                            TipoBonificacion = art.TipoBonificacion,
                            UnidadMedidaCompraId = art.UnidadMedidaCompraId,
                            UnidadMedidaVentaId = art.UnidadMedidaVentaId,
                            UnidadMedidaCompra = umc.Descripcion,
                            UnidadMedidaVenta = umv.Descripcion,
                            EscalaValor1Id = artConc.EscalaValorDosId,
                            EscalaValor2Id = artConc.EscalaValorDosId,
                            EstaSeleccionado = false,
                            EscalaValorUno = e1.Descripcion ?? string.Empty,
                            EscalaValorDos = e2.Descripcion ?? string.Empty,
                        };

            return query.ToList();
        }        
    }
}
