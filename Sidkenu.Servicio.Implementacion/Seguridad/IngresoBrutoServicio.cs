using AutoMapper;
using FluentValidation;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.IngresoBruto;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;
using System.Linq.Expressions;

namespace Sidkenu.Servicio.Implementacion.Seguridad
{
    public class IngresoBrutoServicio : ServicioBase, IIngresoBrutoServicio
    {
        private readonly IValidator<IngresoBrutoPersistenciaDTO> _validatorDto;

        public IngresoBrutoServicio(IUnidadDeTrabajo unitOfWork,
                                    IMapper mapper,
                                    ILogger logger,
                                    IConfiguracionServicio configuracionServicio,
                                    IValidator<IngresoBrutoPersistenciaDTO> validatorDto)
                                    : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            _validatorDto = validatorDto;
        }

        public ResultDTO Add(IngresoBrutoPersistenciaDTO entidad, string user)
        {
            try
            {
                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validacion de Datos de Ingreso Gruto: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var existeEntidad = VerificarSiExiste(entidad.Descripcion);

                if (existeEntidad)
                    return new ResultDTO
                    {
                        Message = "Los datos ingresados ya existen",
                        State = false,
                    };

                var entity = _mapper.Map<IngresoBruto>(entidad);

                entity.User = user;
                entity.EstaEliminado = false;

                _unitOfWork.IngresoBrutoRepository.Add(entity);

                _unitOfWork.Commit();

                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Add Ingreso Bruto - User: {user}", entity);
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

        public ResultDTO Update(IngresoBrutoPersistenciaDTO entidad, string user)
        {
            try
            {
                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validacion de Datos de Ingreso Bruto: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var entityActual = _unitOfWork.IngresoBrutoRepository.GetById(entidad.Id);

                if (entityActual == null)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information("Ocurrio un error al obtener los datos el Ingreso Bruto", entidad);
                    }

                    return new ResultDTO
                    {
                        Message = "Ocurrio un error al obtener los datos del IngresoBruto",
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

                var entity = _mapper.Map<IngresoBruto>(entidad);

                entity.User = user;

                _unitOfWork.IngresoBrutoRepository.Update(entity);

                _unitOfWork.Commit();

                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Update Ingreso Bruto - User: {user}", entity);
                }

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IngresoBrutoDTO>(entity),
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

        public ResultDTO Delete(IngresoBrutoDeleteDTO deleteDTO, string user)
        {
            try
            {
                var entidad = _unitOfWork.IngresoBrutoRepository.GetById(deleteDTO.Id);

                if (entidad == null)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrio un error al obtener los datos de Ingreso Bruto. Id: {deleteDTO.Id}", entidad);
                    }

                    return new ResultDTO
                    {
                        Message = "Ocurrio un error al obtener los datos",
                        State = false
                    };
                }

                _unitOfWork.IngresoBrutoRepository.Delete(deleteDTO.Id, user);

                _unitOfWork.Commit();

                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Delete Ingreso Bruto - User: {user}", entidad);
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
                var entities = _unitOfWork.IngresoBrutoRepository.GetAll();

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<IngresoBrutoDTO>>(entities)
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message}.");
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO GetByFilter(IngresoBrutoFilterDTO filter)
        {
            try
            {
                filter.CadenaBuscar = !string.IsNullOrEmpty(filter.CadenaBuscar) ? filter.CadenaBuscar : string.Empty;

                Expression<Func<IngresoBruto, bool>> filtro = filtro => true;

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

                var entities = _unitOfWork.IngresoBrutoRepository.GetByFilter(filtro);

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<IngresoBrutoDTO>>(entities)
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message}.");
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
                var entity = _unitOfWork.IngresoBrutoRepository.GetById(id);

                if (entity == null)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrio un error al obtener los datos del Ingreso Bruto. Id: {id}");
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
                    Data = _mapper.Map<IngresoBrutoDTO>(entity)
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
            var entidads = _unitOfWork.IngresoBrutoRepository.GetAll();

            if (entidads != null && entidads.Any())
            {
                if (id == null)
                {
                    return entidads.Any(x => x.Descripcion.ToLower() == descripcion.ToLower());
                }
                else
                {
                    return entidads.Any(x => x.Id != id.Value
                        && x.Descripcion.ToLower() == descripcion.ToLower());
                }
            }
            else
            {
                return false;
            }
        }
    }
}
