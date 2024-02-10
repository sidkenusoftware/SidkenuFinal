using AutoMapper;
using FluentValidation;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Articulo;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;
using System.Linq.Expressions;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class ArticuloTemporalServicio : ServicioBase, IArticuloTemporalServicio
    { 
        public ArticuloTemporalServicio(IUnidadDeTrabajo unitOfWork,
                                        IMapper mapper,
                                        ILogger logger,
                                        IConfiguracionServicio configuracionServicio)
                                        : base(unitOfWork, mapper, logger, configuracionServicio)
        {
        }

        public ResultDTO Add(ArticuloTemporalPersistenciaDTO entidad, string user)
        {
            try
            {
                var entity = _mapper.Map<ArticuloTemporal>(entidad);

                entity.User = user;
                entity.EstaEliminado = false;
                entity.EmpresaId = entidad.EmpresaId;

                _unitOfWork.ArticuloTemporalRepository.Add(entity);

                entidad.Id = entity.Id;

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

        public ResultDTO GetAll(Guid empresaId)
        {
            try
            {
                Expression<Func<ArticuloTemporal, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.EmpresaId == empresaId);

                var entities = _unitOfWork.ArticuloTemporalRepository.GetByFilter(filtro,
                                           x => x.OrderBy(d => d.Descripcion));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<ArticuloTemporalDTO>>(entities)
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

        public ResultDTO GetByFilter(ArticuloFilterDTO filter)
        {
            try
            {
                filter.CadenaBuscar = !string.IsNullOrEmpty(filter.CadenaBuscar) ? filter.CadenaBuscar : string.Empty;

                Expression<Func<ArticuloTemporal, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.EmpresaId == filter.EmpresaId || !x.EmpresaId.HasValue);


                if (filter.CadenaBuscar.IndexOf(SeparacionFiltroBusqueda.CaracterSeparador) != -1)
                {
                    var primeraPasada = true;

                    var listaCadenas = filter.CadenaBuscar.Split(SeparacionFiltroBusqueda.CaracterSeparador, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var cadena in listaCadenas)
                    {
                        if (primeraPasada)
                        {
                            filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                                     && (x.Descripcion.ToLower().Contains(cadena.ToLower())
                                                         || x.Codigo == filter.CadenaBuscar));

                            primeraPasada = false;
                        }
                        else
                        {
                            filtro = filtro.Or(x => x.EstaEliminado == filter.VerEliminados
                                                       && (x.Descripcion.ToLower().Contains(cadena.ToLower())
                                                        || x.Codigo == filter.CadenaBuscar));
                        }
                    }
                }
                else
                {
                    filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                                && (x.Descripcion.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                                 || x.Codigo == filter.CadenaBuscar));
                }

                var entities = _unitOfWork.ArticuloTemporalRepository.GetByFilterTake(filtro,
                                                                                      x => x.OrderBy(d => d.Descripcion),
                                                                                      null,
                                                                                      false);

                var result = _mapper.Map<IEnumerable<ArticuloTemporalDTO>>(entities);

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

        public ResultDTO GetByFilterLookUp(ArticuloFilterDTO filter)
        {
            try
            {
                filter.CadenaBuscar = !string.IsNullOrEmpty(filter.CadenaBuscar) ? filter.CadenaBuscar : string.Empty;

                Expression<Func<ArticuloTemporal, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.EmpresaId == filter.EmpresaId || !x.EmpresaId.HasValue);

                if (filter.CadenaBuscar.IndexOf(SeparacionFiltroBusqueda.CaracterSeparador) != -1)
                {
                    var primeraPasada = true;

                    var listaCadenas = filter.CadenaBuscar.Split(SeparacionFiltroBusqueda.CaracterSeparador, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var cadena in listaCadenas)
                    {
                        if (primeraPasada)
                        {
                            filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                                     && (x.Descripcion.ToLower().Contains(cadena.ToLower())
                                                         || x.Codigo == filter.CadenaBuscar));

                            primeraPasada = false;
                        }
                        else
                        {
                            filtro = filtro.Or(x => x.EstaEliminado == filter.VerEliminados
                                                       && (x.Descripcion.ToLower().Contains(cadena.ToLower())
                                                        || x.Codigo == filter.CadenaBuscar));
                        }
                    }
                }
                else
                {
                    filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                                && (x.Descripcion.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                                 || x.Codigo == filter.CadenaBuscar));
                }

                var entities = _unitOfWork.ArticuloTemporalRepository.GetByFilterTake(filtro,
                                                                                      x => x.OrderBy(d => d.Descripcion),
                                                                                      null,
                                                                                      false);

                var result = _mapper.Map<IEnumerable<ArticuloTemporalDTO>>(entities);

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

        public ResultDTO GetById(Guid id, Guid empresaId)
        {
            try
            {
                var entity = _unitOfWork.ArticuloTemporalRepository.GetById(id);

                if (entity == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrio un error al obtener los datos del Articulo. Id: {id}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = "No se encontro el dato solicitado"
                    };
                }

                var result = _mapper.Map<ArticuloTemporalDTO>(entity);

                return new ResultDTO
                {
                    State = true,
                    Data = result
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
