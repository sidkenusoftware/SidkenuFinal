using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.ArticuloKit;
using Sidkenu.Servicio.DTOs.Core.ConfiguracionCore;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class ArticuloKitServicio : ServicioBase, IArticuloKitServicio
    {
        private readonly IConfiguracionCoreServicio _configuracionCoreServicio;

        public ArticuloKitServicio(IUnidadDeTrabajo unitOfWork,
                                   IMapper mapper,
                                   ILogger logger,
                                   IConfiguracionCoreServicio configuracionCoreServicio)
                                   : base(unitOfWork, mapper, logger)
        {
            _configuracionCoreServicio = configuracionCoreServicio;
        }        

        public ResultDTO Add(ArticuloKitPersistenciaDTO articuloKit, string user)
        {
            try
            {
                var _configCoreResult = _configuracionCoreServicio
                    .Get(articuloKit.EmpresaId);

                if (_configCoreResult == null || !_configCoreResult.State)
                {
                    return new ResultDTO
                    {
                        State = false,
                        Message = "Ocurrió un error al obtener la configuracion del Inventario"
                    };
                }

                var _configCore = (ConfiguracionCoreDTO)_configCoreResult.Data;

                var _articulo = _unitOfWork.ArticuloRepository
                    .GetById(articuloKit.ArticuloBaseId, x=> x.Include(i=>i.ArticuloDepositos)
                                                              .Include(i => i.ArticuloPrecios));

                if(_articulo == null ) 
                {
                    return new ResultDTO
                    {
                        State = false,
                        Message = "Ocurrió un error al obtener el Articulo BASE"
                    };
                }

                var _artKits = _unitOfWork.ArticuloKitRepository.GetByFilter(x => x.ArticuloPadreId == _articulo.Id);

                foreach (var artKit in articuloKit.Articulos.ToList())
                {
                    if (artKit.ExisteBase)
                    {
                        var art = _artKits.FirstOrDefault(x => x.ArticuloHijoId == artKit.ArticuloHijoId);

                        if (art == null)
                            return new ResultDTO
                            {
                                State = false,
                                Message = "Ocurrió un error al obtener el Articulo Kit"
                            };

                        if (artKit.EstaEliminado)
                        {
                            _unitOfWork.ArticuloKitRepository.DeleteFisico(art.Id, user);
                        }
                        else
                        {
                            art.Cantidad = artKit.Cantidad;

                            _unitOfWork.ArticuloKitRepository.Update(art);
                        }
                    }
                    else
                    {
                        var artKitNuevo = new ArticuloKit
                        {
                            ArticuloHijoId = artKit.Id,
                            ArticuloPadreId = _articulo.Id,
                            Cantidad = artKit.Cantidad,
                            EstaEliminado = false,
                            User = user,                            
                        };

                        _unitOfWork.ArticuloKitRepository.Add(artKitNuevo);
                    }
                }
                
                // Actualizo el Precio Publico a TODAS las Listas
                foreach (var artPrecio in _articulo.ArticuloPrecios.ToList())
                {
                    artPrecio.Monto = articuloKit.PrecioPublico;
                }

                // Actualizo el Stock al deposito por defecto para la venta
                foreach (var artDepo in _articulo.ArticuloDepositos.Where(x=>x.DepositoId == _configCore.DepositoPorDefectoParaVentaId).ToList())
                {
                    artDepo.Cantidad = articuloKit.Stock;
                }

                // Actualizo el Precio costo y la Fecha de Vigencia
                _articulo.PrecioCosto = articuloKit.PrecioCosto;
                _articulo.FechaVigenciaKit = articuloKit.FechaVigencia;

                _unitOfWork.ArticuloRepository.Update(_articulo);

                _unitOfWork.Commit();                

                return new ResultDTO
                {
                    State = true,
                    Message = "El kit se grabo correctamente"
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
    }
}
