using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Proveedor;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;
using System.Linq.Expressions;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class ProveedorServicio : ServicioBase, IProveedorServicio
    {
        private readonly IValidator<ProveedorPersistenciaDTO> _validatorDto;

        public ProveedorServicio(IUnidadDeTrabajo unitOfWork,
                               IMapper mapper,
                               ILogger logger,
                               IConfiguracionServicio configuracionServicio,
                               IValidator<ProveedorPersistenciaDTO> validatorDto)
                               : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            _validatorDto = validatorDto;
        }

        public ResultDTO Add(ProveedorPersistenciaDTO entidad, string user)
        {
            try
            {
                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validación de Datos del Proveedor: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var existeEntidad = VerificarSiExiste(entidad.CUIT, entidad.EmpresaId);

                if (existeEntidad)
                    return new ResultDTO
                    {
                        Message = "Los datos ingresados ya existen",
                        State = false,
                    };

                var entity = _mapper.Map<Dominio.Entidades.Core.Proveedor>(entidad);

                entity.User = user;
                entity.EstaEliminado = false;

                _unitOfWork.ProveedorRepository.Add(entity);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Add Proveedor - User: {user}", entity);
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

        public ResultDTO Update(ProveedorPersistenciaDTO entidad, string user)
        {
            try
            {
                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validación de Datos del Proveedor: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var entityActual = _unitOfWork.ProveedorRepository.GetById(entidad.Id);

                if (entityActual == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information("Ocurrió un error al obtener los datos del Proveedor", entidad);
                    }

                    return new ResultDTO
                    {
                        Message = "Ocurrió un error al obtener los datos del Proveedor",
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

                var entity = _mapper.Map<Dominio.Entidades.Core.Proveedor>(entidad);

                entity.User = user;

                _unitOfWork.ProveedorRepository.Update(entity);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Update Proveedor - User: {user}", entity);
                }

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<ProveedorDTO>(entity),
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

        public ResultDTO Delete(ProveedorDeleteDTO deleteDTO, string user)
        {
            try
            {
                var entidad = _unitOfWork.ProveedorRepository.GetById(deleteDTO.Id);

                if (entidad == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrió un error al obtener los datos del Proveedor. Id: {deleteDTO.Id}", deleteDTO);
                    }

                    return new ResultDTO
                    {
                        Message = "Ocurrió un error al obtener los datos",
                        State = false
                    };
                }

                _unitOfWork.ProveedorRepository.Delete(deleteDTO.Id, user);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Delete Proveedor - User: {user}", entidad);
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
                var entities = _unitOfWork.ProveedorRepository
                    .GetAll(x => x.Include(z => z.TipoResponsabilidad));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<ProveedorDTO>>(entities)
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Ocurrió un error al obtener los datos del Proveedor. {ex.Message}");
                }

                return new ResultDTO
                {
                    Message = ErrorException.Mensaje(ex),
                    State = false
                };
            }
        }

        public ResultDTO GetAll(Guid empresaId)
        {
            try
            {
                var entities = _unitOfWork.ProveedorRepository
                    .GetByFilter(x => x.EmpresaId == empresaId, null, x => x.Include(z => z.TipoResponsabilidad));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<ProveedorDTO>>(entities)
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Ocurrió un error al obtener los datos del Proveedor. {ex.Message}");
                }

                return new ResultDTO
                {
                    Message = ErrorException.Mensaje(ex),
                    State = false
                };
            }
        }

        public ResultDTO GetByFilter(ProveedorFilterDTO filter)
        {
            try
            {
                filter.CadenaBuscar = !string.IsNullOrEmpty(filter.CadenaBuscar) ? filter.CadenaBuscar : string.Empty;

                Expression<Func<Dominio.Entidades.Core.Proveedor, bool>> filtro = filtro => true;

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
                                && (x.RazonSocial.ToLower().Contains(cadena.ToLower())
                                || x.CUIT == cadena));

                            primeraPasada = false;
                        }
                        else
                        {
                            filtro = filtro.Or(x => x.EstaEliminado == filter.VerEliminados
                                && (x.RazonSocial.ToLower().Contains(cadena.ToLower())
                                || x.CUIT == cadena));
                        }
                    }
                }
                else
                {
                    filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                && (x.RazonSocial.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                || x.CUIT == filter.CadenaBuscar));
                }

                var entities = _unitOfWork.ProveedorRepository.GetByFilter(filtro, null, x => x.Include(z => z.TipoResponsabilidad));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<ProveedorDTO>>(entities)
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Ocurrió un error al obtener los datos del Proveedor. {ex.Message}");
                }

                return new ResultDTO
                {
                    Message = $"Ocurrió un error grave al obtener los datos - {ex.Message}",
                    State = false
                };
            }
        }

        public ResultDTO GetById(Guid id)
        {
            try
            {
                var entity = _unitOfWork.ProveedorRepository
                    .GetById(id, x => x.Include(z => z.TipoResponsabilidad));

                if (entity == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrió un error al obtener los datos del Proveedor. Id: {id}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = "No se encontró el dato solicitado"
                    };
                }

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<ProveedorDTO>(entity)
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message}. Id: {id}");
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
            var entidads = _unitOfWork.ProveedorRepository.GetAll();

            if (entidads != null && entidads.Any())
            {
                if (id == null)
                {
                    if (empresaId.HasValue)
                    {
                        return entidads.Any(x => x.EmpresaId == empresaId.Value && x.CUIT.ToLower() == descripcion.ToLower());
                    }
                    else
                    {
                        return entidads.Any(x => x.CUIT.ToLower() == descripcion.ToLower());
                    }
                }
                else
                {
                    if (empresaId.HasValue)
                    {
                        return entidads.Any(x => x.Id != id.Value
                            && x.EmpresaId == empresaId
                            && x.CUIT.ToLower() == descripcion.ToLower());
                    }
                    else
                    {
                        return entidads.Any(x => x.Id != id.Value
                            && x.CUIT.ToLower() == descripcion.ToLower());
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
