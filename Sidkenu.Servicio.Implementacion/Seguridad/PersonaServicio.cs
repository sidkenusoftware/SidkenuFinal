using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.Persona;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;
using System.Linq.Expressions;

namespace Sidkenu.Servicio.Implementacion.Seguridad
{
    public class PersonaServicio : ServicioBase, IPersonaServicio
    {
        private readonly IValidator<PersonaPersistenciaDTO> _validatorDto;

        public PersonaServicio(IUnidadDeTrabajo unitOfWork,
                               IMapper mapper,
                               ILogger logger,
                               IConfiguracionServicio configuracionServicio,
                               IValidator<PersonaPersistenciaDTO> validatorDto)
                               : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            _validatorDto = validatorDto;
        }

        public ResultDTO Add(PersonaPersistenciaDTO entidad, string user)
        {
            try
            {
                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validación de Datos de la Persona: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var existeEntidad = VerificarSiExiste(entidad.Cuil);

                if (existeEntidad)
                    return new ResultDTO
                    {
                        Message = "Los datos ingresados ya existen",
                        State = false,
                    };

                var entity = _mapper.Map<Persona>(entidad);

                entity.User = user;
                entity.EstaEliminado = false;

                _unitOfWork.PersonaRepository.Add(entity);

                _unitOfWork.Commit();

                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Add Persona - User: {user}", entity);
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
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
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
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
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

        public ResultDTO Update(PersonaPersistenciaDTO entidad, string user)
        {
            try
            {
                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validación de Datos de Persona: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var entityActual = _unitOfWork.PersonaRepository.GetById(entidad.Id);

                if (entityActual == null)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information("Ocurrió un error al obtener los datos de la Persona", entidad);
                    }

                    return new ResultDTO
                    {
                        Message = "Ocurrió un error al obtener los datos del Persona",
                        State = false
                    };

                }

                if (entityActual.EstaEliminado)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information("No se puede Actualizar los datos porque la entidad seleccionada se encuentra eliminada", entityActual);
                    }

                    return new ResultDTO
                    {
                        Message = "No se puede Actualizar los datos porque la entidad seleccionada se encuentra eliminada",
                        State = false,
                    };
                }

                var entity = _mapper.Map<Persona>(entidad);

                entity.User = user;

                _unitOfWork.PersonaRepository.Update(entity);

                _unitOfWork.Commit();

                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Update Persona - User: {user}", entity);
                }

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<PersonaDTO>(entity),
                    Message = "Los datos se actualizaron correctamente"
                };
            }
            catch (ValidationException ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
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
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
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

        public ResultDTO Delete(PersonaDeleteDTO deleteDTO, string user)
        {
            try
            {
                var entidad = _unitOfWork.PersonaRepository.GetById(deleteDTO.Id);

                if (entidad == null)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrió un error al obtener los datos de la Persona. Id: {deleteDTO.Id}", deleteDTO);
                    }

                    return new ResultDTO
                    {
                        Message = "Ocurrió un error al obtener los datos",
                        State = false
                    };
                }

                _unitOfWork.PersonaRepository.Delete(deleteDTO.Id, user);

                _unitOfWork.Commit();

                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Delete Persona - User: {user}", entidad);
                }

                return new ResultDTO
                {
                    State = true,
                    Message = !entidad.EstaEliminado ? "Los datos se eliminaron correctamente" : "Los datos se recuperaron correctamente"
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
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
                var entities = _unitOfWork.PersonaRepository.GetAll(x => x.Include(u => u.Usuarios));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<PersonaDTO>>(entities)
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Ocurrió un error al obtener los datos de la Persona. {ex.Message}");
                }

                return new ResultDTO
                {
                    Message = ErrorException.Mensaje(ex),
                    State = false
                };
            }
        }

        public ResultDTO GetByFilter(PersonaFilterDTO filter)
        {
            try
            {
                filter.CadenaBuscar = !string.IsNullOrEmpty(filter.CadenaBuscar) ? filter.CadenaBuscar : string.Empty;

                Expression<Func<Persona, bool>> filtro = filtro => true;

                if (filter.CadenaBuscar.IndexOf(SeparacionFiltroBusqueda.CaracterSeparador) != -1)
                {
                    var primeraPasada = true;

                    var listaCadenas = filter.CadenaBuscar.Split(SeparacionFiltroBusqueda.CaracterSeparador, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var cadena in listaCadenas)
                    {
                        if (primeraPasada)
                        {
                            filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                && (x.Apellido.ToLower().Contains(cadena.ToLower())
                                || x.Nombre.ToLower().Contains(cadena.ToLower())
                                || x.Cuil == cadena));

                            primeraPasada = false;
                        }
                        else
                        {
                            filtro = filtro.Or(x => x.EstaEliminado == filter.VerEliminados
                                && (x.Apellido.ToLower().Contains(cadena.ToLower())
                                || x.Nombre.ToLower().Contains(cadena.ToLower())
                                || x.Cuil == cadena));
                        }
                    }
                }
                else
                {
                    filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                && (x.Apellido.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                || x.Nombre.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                || x.Cuil == filter.CadenaBuscar));
                }

                var entities = _unitOfWork.PersonaRepository.GetByFilter(filtro, null, x => x.Include(u => u.Usuarios));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<PersonaDTO>>(entities)
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Ocurrió un error al obtener los datos de la Persona. {ex.Message}");
                }

                return new ResultDTO
                {
                    Message = $"Ocurrió un error grave al obtener los datos - {ex.Message}",
                    State = false
                };
            }
        }

        public ResultDTO GetByFilterLookUp(PersonaFilterDTO filter)
        {
            try
            {
                filter.CadenaBuscar = !string.IsNullOrEmpty(filter.CadenaBuscar) ? filter.CadenaBuscar : string.Empty;

                Expression<Func<Persona, bool>> filtro = filtro => true;

                if (filter.CadenaBuscar.IndexOf(SeparacionFiltroBusqueda.CaracterSeparador) != -1)
                {
                    var primeraPasada = true;

                    var listaCadenas = filter.CadenaBuscar.Split(SeparacionFiltroBusqueda.CaracterSeparador, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var cadena in listaCadenas)
                    {
                        if (primeraPasada)
                        {
                            filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                && (x.Apellido.ToLower().Contains(cadena.ToLower())
                                || x.Nombre.ToLower().Contains(cadena.ToLower())
                                || x.Cuil == cadena));

                            primeraPasada = false;
                        }
                        else
                        {
                            filtro = filtro.Or(x => x.EstaEliminado == filter.VerEliminados
                                && (x.Apellido.ToLower().Contains(cadena.ToLower())
                                || x.Nombre.ToLower().Contains(cadena.ToLower())
                                || x.Cuil == cadena));
                        }
                    }
                }
                else
                {
                    filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                && (x.Apellido.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                || x.Nombre.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                || x.Cuil == filter.CadenaBuscar));
                }

                var entities = _unitOfWork.PersonaRepository.GetByFilter(filtro, null, x => x.Include(u => u.Usuarios));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<PersonaDTO>>(entities)
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Ocurrió un error al obtener los datos de la Persona. {ex.Message}");
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
                var entity = _unitOfWork.PersonaRepository.GetById(id, x => x.Include(u => u.Usuarios));

                if (entity == null)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrió un error al obtener los datos de Empresa. Id: {id}");
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
                    Data = _mapper.Map<PersonaDTO>(entity)
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
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
        private bool VerificarSiExiste(string descripcion, Guid? id = null)
        {
            var entidads = _unitOfWork.PersonaRepository.GetAll();

            if (entidads != null && entidads.Any())
            {
                if (id == null)
                {
                    return entidads.Any(x => x.Cuil.ToLower() == descripcion.ToLower());
                }
                else
                {
                    return entidads.Any(x => x.Id != id.Value
                        && x.Cuil.ToLower() == descripcion.ToLower());
                }
            }
            else
            {
                return false;
            }
        }
    }
}
