using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.ArticuloVariante;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class ArticuloVarianteServicio : ServicioBase, IArticuloVarianteServicio
    {
        private readonly IArticuloPrecioServicio _articuloPrecioServicio;

        public ArticuloVarianteServicio(IUnidadDeTrabajo unitOfWork,
                                        IMapper mapper,
                                        ILogger logger,
                                        IConfiguracionServicio configuracionServicio,
                                        IArticuloPrecioServicio articuloPrecioServicio)
                                        : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            _articuloPrecioServicio = articuloPrecioServicio;
        }

        public ResultDTO Add(List<ArticuloVarianteValorPersistenciaDTO> listaVariantesValores, Guid empresaId, string user)
        {
            try
            {
                var listaVariantesRepetidas = new List<string>();

                var _fechaActualizacion = DateTime.Now;
                
                if (!listaVariantesValores.Any())
                    return new ResultDTO { State = false, Message = "No hay valores cargados" };

                var articuloId = listaVariantesValores.First().ArticuloId;

                var _articuloSeleccionado = _unitOfWork.ArticuloRepository.GetById(articuloId,
                    i => i.Include(z => z.ArticuloPrecios)
                          .Include(z => z.ArticuloDepositos));

                var listaVariantes = _unitOfWork.VarianteValorRepository
                    .GetAll();

                var _marca = _unitOfWork.MarcaRepository.GetById(_articuloSeleccionado.MarcaId);

                var _familia = _unitOfWork.FamiliaRepository.GetById(_articuloSeleccionado.FamiliaId);

                foreach (var variante in listaVariantesValores)
                {
                    var varianteValorUno = listaVariantes.First(x => x.Id == variante.VarianteValor1Id);
                    var varianteValorDos = listaVariantes.First(x => x.Id == variante.VarianteValor2Id);

                    if (VerificarSiExiste(articuloId, varianteValorUno, varianteValorDos)) 
                    {
                        listaVariantesRepetidas.Add($"{_articuloSeleccionado.Descripcion} {varianteValorUno.Descripcion} {varianteValorDos.Descripcion}");
                        continue;
                    }

                    var nuevoArticulo = new Articulo
                    {
                        Abreviatura = _articuloSeleccionado.Abreviatura,
                        ActivarBonificacion = _articuloSeleccionado.ActivarBonificacion,
                        Bonificacion = _articuloSeleccionado.Bonificacion,
                        BonificacionFechaDesde = _articuloSeleccionado.BonificacionFechaDesde,
                        BonificacionFechaHasta = _articuloSeleccionado.BonificacionFechaHasta,
                        Codigo = $"{_articuloSeleccionado.Codigo} {varianteValorUno.Codigo} {varianteValorDos.Codigo}",
                        CodigoBarra = _articuloSeleccionado.CodigoBarra,
                        Comision = _articuloSeleccionado.Comision,
                        CondicionIvaId = _articuloSeleccionado.CondicionIvaId,
                        Descripcion = $"{_articuloSeleccionado.Descripcion} {varianteValorUno.Descripcion} {varianteValorDos.Descripcion}",
                        DescripcionAdicional = _articuloSeleccionado.DescripcionAdicional,
                        Detalle = _articuloSeleccionado.Detalle,
                        EmpresaId = _articuloSeleccionado.EmpresaId,
                        EstaBloqueado = _articuloSeleccionado.EstaBloqueado,
                        EstaEliminado = false,
                        FamiliaId = _articuloSeleccionado.FamiliaId,
                        Foto = _articuloSeleccionado.Foto,
                        Garantia = _articuloSeleccionado.Garantia,
                        LimiteVenta = _articuloSeleccionado.LimiteVenta,
                        MarcaId = _articuloSeleccionado.MarcaId,
                        PerfilArticulo = PerfilArticulo.CompraVenta,
                        PermiteStockNegativo = _articuloSeleccionado.PermiteStockNegativo,
                        PorcentajePerdida = _articuloSeleccionado.PorcentajePerdida,
                        PuntoPedido = _articuloSeleccionado.PuntoPedido,
                        Rentabilidad = _articuloSeleccionado.Rentabilidad,
                        StockMaximo = _articuloSeleccionado.StockMaximo,
                        StockMinimo = _articuloSeleccionado.StockMinimo,
                        TieneGarantia = _articuloSeleccionado.TieneGarantia,
                        ActivarLimiteVenta = _articuloSeleccionado.ActivarLimiteVenta,
                        TienePerdida = _articuloSeleccionado.TienePerdida,
                        TieneRentabilidad = _articuloSeleccionado.TieneRentabilidad,
                        TipoBonificacion = _articuloSeleccionado.TipoBonificacion,
                        UnidadMedidaCompraId = _articuloSeleccionado.UnidadMedidaCompraId,
                        UnidadMedidaVentaId = _articuloSeleccionado.UnidadMedidaVentaId,                        
                        ArticuloPadreId = articuloId,
                        VarianteValorUnoId = variante.VarianteValor1Id,
                        VarianteValorDosId = variante.VarianteValor2Id,
                        User = user,
                        TipoArticulo = Aplicacion.Constantes.TipoArticulo.Variante,                        
                        
                        ArticuloDepositos = new List<ArticuloDeposito>(),
                        ArticuloPrecios = new List<ArticuloPrecio>()
                    };

                    // =================================================================================== //
                    // ==============                     ASIGNO a DEPOSITOS               =============== //
                    // =================================================================================== //

                    foreach (var articuloDeposito in _articuloSeleccionado.ArticuloDepositos)
                    {
                        nuevoArticulo.ArticuloDepositos.Add(new ArticuloDeposito
                        {
                            Cantidad = variante.UtilizaPrecioStockIndividual ? variante.Stock : articuloDeposito.Cantidad,
                            DepositoId = articuloDeposito.DepositoId,
                            EstaEliminado = false,
                            User = user
                        });
                    }

                    // =================================================================================== //
                    // ==============                   CALCULO  PRECIO                    =============== //
                    // =================================================================================== //

                    var _precioCostoArticulo = variante.UtilizaPrecioStockIndividual ? variante.Precio : _articuloSeleccionado.PrecioCosto;

                    nuevoArticulo.PrecioCosto = _precioCostoArticulo;

                    _articuloPrecioServicio.AddOrUpdate(new DTOs.Core.ArticuloPrecio.ArticuloPrecioPersistenciaDTO
                    {
                        ArticuloId = variante.ArticuloId,
                        EmpresaId = empresaId,
                        FamiliaId = _articuloSeleccionado.FamiliaId,
                        MarcaId = _articuloSeleccionado.MarcaId,
                        PrecioCostoArticulo = _precioCostoArticulo,
                        RentabilidadArticulo = _articuloSeleccionado.Rentabilidad,
                        TieneRentabilidad = _articuloSeleccionado.TieneRentabilidad,
                        EsFabricado = false
                    },
                    nuevoArticulo,
                    user);

                    _unitOfWork.ArticuloRepository.Add(nuevoArticulo);                    
                }

                _unitOfWork.Commit();

                var mensaje = string.Empty;

                mensaje = !listaVariantesRepetidas.Any() ? "Los datos se grabaron correctamente"
                                                         : "Las siguientes variantes no se cargaron porque ya existen:" + Environment.NewLine
                                                           + string.Join(Environment.NewLine, listaVariantesRepetidas) + "." + Environment.NewLine;

                mensaje += listaVariantesRepetidas.Any() && listaVariantesRepetidas.Count < listaVariantesValores.Count
                           ? "El resto de las variantes se grabaron correctamente"
                           : string.Empty;

                return new ResultDTO
                {
                    State = true,
                    Message = mensaje,
                };
            }
            catch (ValidationException ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error de validaciones {ErrorValidator.ObtenerErrores(ex.Errors)} - User: {user}.");
                }

                return new ResultDTO
                {
                    Message = ErrorValidator.ObtenerErrores(ex.Errors),
                    State = false
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {user}.");
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        private bool VerificarSiExiste(Guid articuloId, VarianteValor varianteValorUno, VarianteValor varianteValorDos)
        {
            return _unitOfWork.ArticuloRepository.GetByFilter(x => x.ArticuloPadreId == articuloId
                                                                   && x.VarianteValorUnoId == varianteValorUno.Id
                                                                   && x.VarianteValorDosId == varianteValorDos.Id).Any();
        }
    }
}
