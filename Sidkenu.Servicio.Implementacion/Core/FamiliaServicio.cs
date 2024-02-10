using AutoMapper;
using FluentValidation;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Familia;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;
using System.Linq.Expressions;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class FamiliaServicio : ServicioBase, IFamiliaServicio
    {
        private readonly IValidator<FamiliaPersistenciaDTO> _validatorDto;

        public FamiliaServicio(IUnidadDeTrabajo unitOfWork,
                                 IMapper mapper,
                                 ILogger logger,
                                 IConfiguracionServicio configuracionServicio,
                                 IValidator<FamiliaPersistenciaDTO> validatorDto)
                                 : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            _validatorDto = validatorDto;
        }

        public ResultDTO Add(FamiliaPersistenciaDTO entidad, string user)
        {
            try
            {
                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validacion de Datos de la Familia: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var existeEntidad = VerificarSiExiste(entidad.Descripcion, entidad.EmpresaId);

                if (existeEntidad)
                    return new ResultDTO
                    {
                        Message = "Los datos ingresados ya existen",
                        State = false,
                    };

                var entity = _mapper.Map<Dominio.Entidades.Core.Familia>(entidad);

                entity.User = user;
                entity.EstaEliminado = false;

                _unitOfWork.FamiliaRepository.Add(entity);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Add Familia - User: {user}", entity);
                }

                return new ResultDTO
                {
                    State = true,
                    Message = "Los datos se grabaron correctamente",
                    Data = entity
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

        public ResultDTO Update(FamiliaPersistenciaDTO entidad, bool actualizarPrecio, string user)
        {
            try
            {
                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validacion de Datos de la Familia: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var entityActual = _unitOfWork.FamiliaRepository.GetById(entidad.Id);

                if (entityActual == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information("Ocurrio un error al obtener los datos de la Familia", entidad);
                    }

                    return new ResultDTO
                    {
                        Message = "Ocurrio un error al obtener los datos de la Familia",
                        State = false
                    };

                }

                if (entityActual.EstaEliminado)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information("No se puede Actualizar los datos porque la entidad seleccionada se encuentra eliminada", entityActual);
                    }

                    return new ResultDTO
                    {
                        Message = "No se puede Actualizar los datos porque la entidad seleccionada se encuentra eliminada",
                        State = false,
                    };
                }

                var entity = _mapper.Map<Dominio.Entidades.Core.Familia>(entidad);

                entity.User = user;

                _unitOfWork.FamiliaRepository.Update(entity);

                // Actualizacion de Precios
                if (actualizarPrecio)
                {
                    
                }

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Update Familia - User: {user}", entity);
                }

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<FamiliaDTO>(entity),
                    Message = "Los datos se actualizaron correctamente"
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

        public ResultDTO Delete(FamiliaDeleteDTO deleteDTO, string user)
        {
            try
            {
                var entidad = _unitOfWork.FamiliaRepository.GetById(deleteDTO.Id);

                if (entidad == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrio un error al obtener los datos de la Familia. Id: {deleteDTO.Id}", entidad);
                    }

                    return new ResultDTO
                    {
                        Message = "Ocurrio un error al obtener los datos",
                        State = false
                    };
                }

                _unitOfWork.FamiliaRepository.Delete(deleteDTO.Id, user);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Delete Familia - User: {user}", entidad);
                }

                return new ResultDTO
                {
                    State = true,
                    Message = !entidad.EstaEliminado ? "Los datos se eliminaron correctamente" : "Los datos se recuperaron correctamente"
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
                var entities = _unitOfWork.FamiliaRepository.GetAll();

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<FamiliaDTO>>(entities)
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
                Expression<Func<Dominio.Entidades.Core.Familia, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.EmpresaId == empresaId || x.EmpresaId == null);

                var entities = _unitOfWork.FamiliaRepository.GetByFilter(filtro);

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<FamiliaDTO>>(entities)
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

        public ResultDTO GetByFilter(FamiliaFilterDTO filter)
        {
            try
            {
                filter.CadenaBuscar = !string.IsNullOrEmpty(filter.CadenaBuscar) ? filter.CadenaBuscar : string.Empty;

                Expression<Func<Dominio.Entidades.Core.Familia, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.EmpresaId == filter.EmpresaId || x.EmpresaId == null);

                if (filter.CadenaBuscar.IndexOf(SeparacionFiltroBusqueda.CaracterSeparador) != -1)
                {
                    var primeraPasada = true;

                    var listaCadenas = filter.CadenaBuscar.Split(SeparacionFiltroBusqueda.CaracterSeparador, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var cadena in listaCadenas)
                    {
                        if (primeraPasada)
                        {
                            filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                && x.Descripcion.ToLower().Contains(cadena.ToLower()));

                            primeraPasada = false;
                        }
                        else
                        {
                            filtro = filtro.Or(x => x.EstaEliminado == filter.VerEliminados
                                && x.Descripcion.ToLower().Contains(cadena.ToLower()));
                        }
                    }
                }
                else
                {
                    filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                && x.Descripcion.ToLower().Contains(filter.CadenaBuscar.ToLower()));
                }

                var entities = _unitOfWork.FamiliaRepository.GetByFilter(filtro);

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<FamiliaDTO>>(entities)
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
                var entity = _unitOfWork.FamiliaRepository.GetById(id);

                if (entity == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrio un error al obtener los datos de la Familia. Id: {id}");
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
                    Data = _mapper.Map<FamiliaDTO>(entity)
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
        private bool VerificarSiExiste(string descripcion, Guid? empresaId, Guid? id = null)
        {
            var entidads = _unitOfWork.FamiliaRepository.GetAll();

            if (entidads != null && entidads.Any())
            {
                if (id == null)
                {
                    if (empresaId.HasValue)
                    {
                        return entidads.Any(x => x.EmpresaId == empresaId.Value && x.Descripcion.ToLower() == descripcion.ToLower());
                    }
                    else
                    {
                        return entidads.Any(x => x.Descripcion.ToLower() == descripcion.ToLower());
                    }
                }
                else
                {
                    if (empresaId.HasValue)
                    {
                        return entidads.Any(x => x.Id != id.Value
                            && x.EmpresaId == empresaId
                            && x.Descripcion.ToLower() == descripcion.ToLower());
                    }
                    else
                    {
                        return entidads.Any(x => x.Id != id.Value
                            && x.Descripcion.ToLower() == descripcion.ToLower());
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
