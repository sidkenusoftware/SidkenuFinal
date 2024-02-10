using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Articulo;
using Sidkenu.Servicio.DTOs.Core.ArticuloProveedor;
using Sidkenu.Servicio.DTOs.Core.ConfiguracionCore;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;
using System.Linq.Expressions;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class ArticuloProveedorServicio : ServicioBase, IArticuloProveedorServicio
    {
        private readonly IConfiguracionCoreServicio _configuracionCoreServicio;

        public ArticuloProveedorServicio(IUnidadDeTrabajo unitOfWork,
                                                    IMapper mapper,
                                                    ILogger logger,
                                                    IConfiguracionServicio configuracionServicio,
                                                    IConfiguracionCoreServicio configuracionCoreServicio)
                                                    : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            _configuracionCoreServicio = configuracionCoreServicio;
        }

        public ResultDTO GetAll(Guid? articuloId)
        {
            if (articuloId.HasValue)
            {
                return new ResultDTO
                {
                    State = true,
                    Data = new List<ArticuloProveedorDTO>()
                };
            }

            try
            {
                var entities = _unitOfWork.ArticuloRepository.GetAll();

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<ArticuloProveedorDTO>>(entities)
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

        public ResultDTO GetArticulosSugeridos(Guid? proveedorId, Guid empresaId)
        {
            try
            {
                var _configCoreResult = _configuracionCoreServicio
                    .Get(empresaId);

                if (_configCoreResult == null || !_configCoreResult.State)
                {
                    return new ResultDTO
                    {
                        State = false,
                        Message = "Ocurrió un error al obtener la configuracion del Inventario"
                    };
                }

                var _configCore = (ConfiguracionCoreDTO)_configCoreResult.Data;

                Expression<Func<Articulo, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.EmpresaId == empresaId || !x.EmpresaId.HasValue);

                if (proveedorId.HasValue)
                {
                    filtro = filtro.And(x => x.ArticuloProveedores.Any(p => p.ProveedorId == proveedorId.Value));
                }

                filtro = filtro.And(x => x.ArticuloDepositos.Any(d => d.DepositoId == _configCore.DepositoPorDefectoParaCompraId));

                var articulos = _unitOfWork.ArticuloRepository.GetByFilter(filtro,
                                                                          null,
                                                                          i => i.Include(p => p.ArticuloProveedores)
                                                                                .Include(p => p.ArticuloDepositos));

                var listaArticulosSugeridos = new List<Articulo>();

                foreach (var articulo in articulos.ToList())
                {
                    if (articulo.ArticuloDepositos.First(x => x.DepositoId == _configCore.DepositoPorDefectoParaCompraId && !x.EstaEliminado).Cantidad
                        <= articulo.PuntoPedido)
                    {
                        listaArticulosSugeridos.Add(articulo);
                    }
                }

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<ArticuloDTO>>(listaArticulosSugeridos)
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
    }
}
