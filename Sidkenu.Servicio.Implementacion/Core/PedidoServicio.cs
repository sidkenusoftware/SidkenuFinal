using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Pedido;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;
using System.Linq.Expressions;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class PedidoServicio : ServicioBase, IPedidoServicio
    {
        public PedidoServicio(IUnidadDeTrabajo unitOfWork, 
                              IMapper mapper, 
                              ILogger logger, 
                              IConfiguracionServicio configuracionServicio) 
                              : base(unitOfWork, mapper, logger, configuracionServicio)
        {

        }

        public ResultDTO Add(PedidoPersistenciaDTO entidad, string user)
        {
            try
            {
                var existeEntidad = VerificarSiExiste(entidad.Numero, entidad.EmpresaId);

                if (existeEntidad)
                    return new ResultDTO
                    {
                        Message = "Los datos ingresados ya existen",
                        State = false,
                    };

                var entity = _mapper.Map<Dominio.Entidades.Core.Pedido>(entidad);

                entity.User = user;
                entity.EstaEliminado = false;
                entity.Detalles = new List<PedidoDetalle>();

                foreach (var detalleDto in entidad.Detalles.ToList())
                {
                    var entityDetalle = _mapper.Map<Dominio.Entidades.Core.PedidoDetalle>(detalleDto);

                    entityDetalle.User = user;
                    entityDetalle.EstaEliminado = false;

                    entity.Detalles.Add(entityDetalle);
                }

                _unitOfWork.PedidoRepository.Add(entity);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Add Pedido - User: {user}", entity);
                }

                return new ResultDTO
                {
                    State = true,
                    Message = "Los datos se grabaron correctamente",
                    Data = entity
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

        

        public ResultDTO GetAll()
        {
            try
            {
                var entities = _unitOfWork.PedidoRepository.GetAll(i => i.Include(z => z.Proveedor)
                                                                         .Include(z => z.Detalles).ThenInclude(a => a.Articulo));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<PedidoDTO>>(entities)
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

        public ResultDTO GetAll(Guid empresaId)
        {
            try
            {
                Expression<Func<Pedido, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.EmpresaId == empresaId || x.EmpresaId == null);

                var entities = _unitOfWork.PedidoRepository.GetByFilter(filtro, null, i => i.Include(z => z.Proveedor)
                                                                                            .Include(z => z.Detalles).ThenInclude(a => a.Articulo));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<PedidoDTO>>(entities)
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

        public ResultDTO GetByFilter(PedidoFilterDTO filter)
        {
            try
            {
                filter.CadenaBuscar = !string.IsNullOrEmpty(filter.CadenaBuscar) ? filter.CadenaBuscar : string.Empty;

                int _numeroPedido = 0;
                    
                int.TryParse(filter.CadenaBuscar, out _numeroPedido);

                Expression<Func<Pedido, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.EmpresaId == filter.EmpresaId || x.EmpresaId == null);

                filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                    && (x.Numero == _numeroPedido 
                                    || x.Proveedor.RazonSocial.ToLower() == filter.CadenaBuscar.ToLower()
                                    || x.Proveedor.CUIT == filter.CadenaBuscar));

                var entities = _unitOfWork.PedidoRepository.GetByFilter(filtro,
                                                                        null,
                                                                        i => i.Include(z => z.Proveedor)
                                                                              .Include(z => z.Detalles).ThenInclude(a => a.Articulo));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<PedidoDTO>>(entities)
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

        public ResultDTO GetById(Guid id)
        {
            try
            {
                var entity = _unitOfWork.PedidoRepository.GetById(id, i => i.Include(z => z.Proveedor)
                                                                            .Include(z => z.Detalles).ThenInclude(a => a.Articulo));

                if (entity == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrio un error al obtener los datos de la Pedido. Id: {id}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = "No se encontro el dato solicitado"
                    };
                }

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<PedidoDTO>(entity)
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

        // ------------------------------------------------------------------------------------------------------ //
        // ------------------------------             Metodos Privados              ----------------------------- //
        // ------------------------------------------------------------------------------------------------------ //
        private bool VerificarSiExiste(int numero, Guid? empresaId, Guid? id = null)
        {
            var entidads = _unitOfWork.PedidoRepository.GetAll();

            if (entidads != null && entidads.Any())
            {
                if (id == null)
                {
                    if (empresaId.HasValue)
                    {
                        return entidads.Any(x => x.EmpresaId == empresaId.Value && x.Numero == numero);
                    }
                    else
                    {
                        return entidads.Any(x => x.Numero == numero);
                    }
                }
                else
                {
                    if (empresaId.HasValue)
                    {
                        return entidads.Any(x => x.Id != id.Value
                            && x.EmpresaId == empresaId
                            && x.Numero == numero);
                    }
                    else
                    {
                        return entidads.Any(x => x.Id != id.Value
                            && x.Numero == numero);
                    }
                }
            }
            else
            {
                return false;
            }
        }
    }
}
