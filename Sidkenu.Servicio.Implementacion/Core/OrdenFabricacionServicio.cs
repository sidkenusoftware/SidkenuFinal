using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Articulo;
using Sidkenu.Servicio.DTOs.Core.OrdenFabricacion;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;
using System.Linq.Expressions;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class OrdenFabricacionServicio : ServicioBase, IOrdenFabricacionServicio
    {
        private readonly IConfiguracionCoreServicio _configuracionCoreServicio;
        private readonly IArticuloServicio _articuloServicio;
        private readonly IContadorServicio _contadorServicio;
        private readonly IArticuloPrecioServicio _articuloPrecioServicio;

        public OrdenFabricacionServicio(IUnidadDeTrabajo unitOfWork,
                                IMapper mapper,
                                ILogger logger,
                                IConfiguracionServicio configuracionServicio,
                                IConfiguracionCoreServicio configuracionCoreServicio,
                                IArticuloServicio articuloServicio,
                                IContadorServicio contadorServicio,
                                IArticuloPrecioServicio articuloPrecioServicio)
                                : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            _configuracionCoreServicio = configuracionCoreServicio;
            _articuloServicio = articuloServicio;
            _articuloPrecioServicio = articuloPrecioServicio;

            this._contadorServicio = contadorServicio;            
        }

        public ResultDTO Add(OrdenFabricacionPersistenciaDTO entidad, string user)
        {
            try
            {


                var contadorResult = _contadorServicio
                    .ObtenerNumero(TipoContador.OrdenFabricacion, entidad.EmpresaId, user);

                var existeEntidad = VerificarSiExiste(entidad.ArticuloId, entidad.EmpresaId);

                if (existeEntidad)
                    return new ResultDTO
                    {
                        Message = "Los datos ingresados ya existen",
                        State = false,
                    };

                var entity = _mapper.Map<OrdenFabricacion>(entidad);

                entity.Numero = entidad.OrigenFabricacion == OrigenFabricacion.Fabrica ? (int)contadorResult : entidad.NumeroOrden;
                entity.Fecha = DateTime.Now;
                entity.EstadoOrdenFabricacion = EstadoOrdenFabricacion.Pendiente;
                entity.User = user;
                entity.EstaEliminado = false;
                entity.OrdenFabricacionDetalles = new List<OrdenFabricacionDetalle>();
                entity.OrigenFabricacion = entidad.OrigenFabricacion;
                entity.ActulizarPrecioPublico = entidad.ActulizarPrecioPublico; 

                foreach (var detalle in entidad.Detalles)
                {
                    entity.OrdenFabricacionDetalles.Add(new OrdenFabricacionDetalle
                    {
                        ArticuloId = detalle.ArticuloId,
                        Cantidad = detalle.Cantidad,
                        Codigo = detalle.Codigo,
                        Descripcion = detalle.Descripcion,
                        EstaEliminado = false,
                        User = user
                    });
                }

                _unitOfWork.OrdenFabricacionRepository.Add(entity);

                entidad.Id = entity.Id;

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Add Orden de Fabricación - User: {user}", entity);
                }

                return new ResultDTO
                {
                    State = true,
                    Message = "Los datos se grabaron correctamente",
                    Data = entidad
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

        private bool VerificarSiExiste(Guid articuloId, Guid empresaId)
        {
            var result = _unitOfWork.OrdenFabricacionRepository
                .GetByFilter(x => x.ArticuloBaseId == articuloId
                               && x.EmpresaId == empresaId
                               && x.EstadoOrdenFabricacion == Aplicacion.Constantes.EstadoOrdenFabricacion.Pendiente);

            return result !=null && result.Any();
        }

        public ResultDTO GetByFilter(OrdenFabricacionFilterDTO filter)
        {
            try
            {
                filter.CadenaBuscar = !string.IsNullOrEmpty(filter.CadenaBuscar) ? filter.CadenaBuscar : string.Empty;

                Expression<Func<OrdenFabricacion, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.EmpresaId == filter.EmpresaId);

                if (int.TryParse(filter.CadenaBuscar, out int _codigo))
                {
                    filtro = filtro.And(x => x.Numero == _codigo);
                }

                var entities = _unitOfWork.OrdenFabricacionRepository.GetByFilterTake(filtro,
                                                                                      o => o.OrderByDescending(x => x.Fecha),
                                                                                      i =>
                                                                                          //i.Include(x => x.DepositoDestino)
                                                                                          // .Include(x => x.DepositoOrigen)
                                                                                          i.Include(x => x.ArticuloBase)
                                                                                           .Include(x => x.OrdenFabricacionDetalles)
                                                                                                          .ThenInclude(x => x.Articulo)
                                                                                                          .ThenInclude(x => x.ArticuloDepositos)
                                                                                           .Include(x => x.OrdenFabricacionDetalles)
                                                                                                          .ThenInclude(x => x.Articulo)
                                                                                                          .ThenInclude(x => x.ArticuloPrecios),
                                                                                       false);

                var result = _mapper.Map<IEnumerable<OrdenFabricacionDTO>>(entities);

                // Controlar que haya todos los Insumos

                foreach (var orden in result
                    // .Where(x => x.EstadoOrdenFabricacion == Aplicacion.Constantes.EstadoOrdenFabricacion.Pendiente)
                    .ToList())
                {
                    var resultArticulos = _articuloServicio
                        .GetByIds(orden.Detalles.Select(x => x.ArticuloId).ToList(), filter.EmpresaId, orden.DepositoOrigenId);

                    if (!resultArticulos.State)
                    {
                        return new ResultDTO
                        {
                            State = false,
                            Message = "Ocurrio un problema al verificar los Stock de cada Articulo involucrado en el proceso de fabricacion"
                        };
                    }

                    foreach (var detalle in orden.Detalles.ToList())
                    {
                        var _resultArt = (List<ArticuloDTO>)resultArticulos.Data;

                        var _art = _resultArt.FirstOrDefault(x=>x.Id == detalle.ArticuloId);

                        if (_art != null)
                        {
                            detalle.StockActual = _art.Stock;
                            detalle.CantidadFabricar = orden.CantidadFabricar;
                        }
                    }

                    orden.SePuedeFabricar = !orden.Detalles.Any(x => x.Faltante > 0);
                }

                return new ResultDTO
                {
                    State = true,
                    Data = result.ToList(),
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message}");
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO CambiarEstadoEnProceso(Guid OrdenFabricacionId, string user)
        {
            try
            {
                var orden = _unitOfWork.OrdenFabricacionRepository
                    .GetById(OrdenFabricacionId, i => i.Include(z=>z.OrdenFabricacionDetalles), false);

                if(orden != null) 
                {
                    orden.EstadoOrdenFabricacion = EstadoOrdenFabricacion.EnProceso;
                    orden.User = user;
                }

                _unitOfWork.OrdenFabricacionRepository.Update(orden);

                foreach (var detalle in orden.OrdenFabricacionDetalles)
                {
                    var articuloDeposito = _unitOfWork.ArticuloDepositoRepository
                        .GetByFilter(x => x.DepositoId == orden.DepositoOrigenId
                                       && x.ArticuloId == detalle.ArticuloId, null, null, false).First();

                    var resultArticuloDeposito = _unitOfWork.ArticuloDepositoRepository.GetById(articuloDeposito.Id);

                    resultArticuloDeposito.Cantidad -= orden.CantidadFabricar * detalle.Cantidad;

                    _unitOfWork.ArticuloDepositoRepository.Update(resultArticuloDeposito);
                }

                _unitOfWork.Commit();

                return new ResultDTO { State = true };
            }
            catch (Exception ex)
            {
                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO CancelarOrdenFabricacion(Guid OrdenFabricacionId, string user)
        {
            try
            {
                var orden = _unitOfWork.OrdenFabricacionRepository
                    .GetById(OrdenFabricacionId, i => i.Include(z => z.OrdenFabricacionDetalles), false);

                if (orden != null)
                {
                    orden.EstadoOrdenFabricacion = EstadoOrdenFabricacion.Pendiente;
                    orden.User = user;
                }

                _unitOfWork.OrdenFabricacionRepository.Update(orden);

                foreach (var detalle in orden.OrdenFabricacionDetalles)
                {
                    var articuloDeposito = _unitOfWork.ArticuloDepositoRepository
                        .GetByFilter(x => x.DepositoId == orden.DepositoOrigenId
                                       && x.ArticuloId == detalle.ArticuloId, null, null, false).First();

                    var resultArticuloDeposito = _unitOfWork.ArticuloDepositoRepository.GetById(articuloDeposito.Id);

                    resultArticuloDeposito.Cantidad += orden.CantidadFabricar * detalle.Cantidad;

                    _unitOfWork.ArticuloDepositoRepository.Update(resultArticuloDeposito);
                }

                _unitOfWork.Commit();

                return new ResultDTO { State = true , Message = "La Orden de Fabricación se canceló" };
            }
            catch (Exception ex)
            {
                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO FinalizarOrdenFabricacion(Guid OrdenFabricacionId, string user)
        {
            try
            {
                var orden = _unitOfWork.OrdenFabricacionRepository
                    .GetById(OrdenFabricacionId,
                               i => i.Include(z => z.OrdenFabricacionDetalles)
                                     .Include(z => z.ArticuloBase)
                                     .Include(z => z.ArticuloBase).ThenInclude(z => z.OrdenFabricaciones), false);

                if (orden != null)
                {
                    orden.EstadoOrdenFabricacion = EstadoOrdenFabricacion.Finalizada;
                    orden.User = user;
                }

                _unitOfWork.OrdenFabricacionRepository.Update(orden);

                if (orden.OrigenFabricacion == OrigenFabricacion.Fabrica)
                {
                    // Actualizo el Stock

                    var articuloDeposito = _unitOfWork.ArticuloDepositoRepository
                        .GetByFilter(x => x.DepositoId == orden.DepositoOrigenId
                                       && x.ArticuloId == orden.ArticuloBaseId, null, null, false).First();

                    var resultArticuloDeposito = _unitOfWork.ArticuloDepositoRepository.GetById(articuloDeposito.Id);

                    resultArticuloDeposito.Cantidad += orden.CantidadFabricar;

                    _unitOfWork.ArticuloDepositoRepository.Update(resultArticuloDeposito);

                    foreach (var item in orden.OrdenFabricacionDetalles.ToList())
                    {
                        var articuloDepositoDetalle = _unitOfWork.ArticuloDepositoRepository
                        .GetByFilter(x => x.DepositoId == orden.DepositoOrigenId
                                       && x.ArticuloId == item.ArticuloId, null, null, false).First();

                        var resultArticuloDepositoDetalle = _unitOfWork.ArticuloDepositoRepository.GetById(articuloDepositoDetalle.Id);

                        resultArticuloDepositoDetalle.Cantidad += orden.CantidadFabricar;

                        _unitOfWork.ArticuloDepositoRepository.Update(resultArticuloDepositoDetalle);
                    }

                    //// Actualizo los Precios

                    //_articuloPrecioServicio.AddOrUpdate(new DTOs.Core.ArticuloPrecio.ArticuloPrecioPersistenciaDTO
                    //{
                    //    ArticuloId = orden.ArticuloBaseId,
                    //    EmpresaId = orden.EmpresaId,
                    //    FamiliaId = orden.ArticuloBase.FamiliaId,
                    //    MarcaId = orden.ArticuloBase.MarcaId,
                    //    PrecioCostoArticulo = orden.ArticuloBase.PrecioCosto,
                    //    RentabilidadArticulo = orden.ArticuloBase.Rentabilidad,
                    //    TieneRentabilidad = orden.ArticuloBase.TieneRentabilidad,
                    //    EsFabricado = true
                    //}, user);
                }
                else
                {
                    foreach (var item in orden.OrdenFabricacionDetalles.ToList())
                    {
                        var articuloDepositoDetalle = _unitOfWork.ArticuloDepositoRepository
                        .GetByFilter(x => x.DepositoId == orden.DepositoOrigenId
                                       && x.ArticuloId == item.ArticuloId, null, null, false).First();

                        var resultArticuloDepositoDetalle = _unitOfWork.ArticuloDepositoRepository.GetById(articuloDepositoDetalle.Id);

                        resultArticuloDepositoDetalle.Cantidad += orden.CantidadFabricar;

                        _unitOfWork.ArticuloDepositoRepository.Update(resultArticuloDepositoDetalle);
                    }
                }

                _unitOfWork.Commit();

                return new ResultDTO { State = true, Message = "La Orden de Fabricación se canceló" };
            }
            catch (Exception ex)
            {
                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }
    }
}
