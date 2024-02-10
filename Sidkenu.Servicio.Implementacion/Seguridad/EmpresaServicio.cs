using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.Empresa;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;
using System.Linq.Expressions;
using static Sidkenu.Aplicacion.Comun.AndOr;

namespace Sidkenu.Servicio.Implementacion.Seguridad
{
    public class EmpresaServicio : ServicioBase, IEmpresaServicio
    {
        private readonly IValidator<EmpresaPersistenciaDTO> _validatorDto;

        public EmpresaServicio(IUnidadDeTrabajo unitOfWork,
                               IMapper mapper,
                               ILogger logger,
                               IConfiguracionServicio configuracionServicio,
                               IValidator<EmpresaPersistenciaDTO> validatorDto)
                               : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            _validatorDto = validatorDto;
        }

        public ResultDTO Add(EmpresaPersistenciaDTO entidad, string user)
        {


            try
            {
                // _unitOfWork.BeginTransaction();


                var _existenEmpresasCargadas = _unitOfWork.EmpresaRepository.GetAll().Any();

                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validacion de Datos de Empresa: {mensaje}");
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

                var entity = _mapper.Map<Empresa>(entidad);

                entity.User = user;
                entity.EstaEliminado = false;
                entity.Modulos = new List<Modulo>();

                entity.Modulos.Add(new Modulo
                {
                    Compra = false,
                    Fabricacion = false,
                    Inventario = false,
                    PuntoVenta = false,
                    Seguridad = true,
                    Venta = false,
                    User = user,
                    EstaEliminado = false,
                });

                _unitOfWork.EmpresaRepository.Add(entity);


                if (_existenEmpresasCargadas)
                {
                    var grupoAdmin = new Grupo
                    {
                        Descripcion = "Administrador",
                        PorDefecto = true,
                        User = user,
                        EstaEliminado = false,
                        EmpresaId = entity.Id,
                        GrupoFormularios = new List<GrupoFormulario>()
                    };

                    _unitOfWork.GrupoRepository.Add(grupoAdmin);

                    foreach (var formulario in _unitOfWork.FormularioRepository.GetAll())
                    {
                        var grupoFormulario = new GrupoFormulario
                        {
                            GrupoId = grupoAdmin.Id,
                            FormularioId = formulario.Id,
                            EstaEliminado = false,
                            User = user,
                        };

                        _unitOfWork.GrupoFormularioRepository.Add(grupoFormulario);
                    }

                    var userLogin = _unitOfWork.UsuarioRepository.GetByFilter(x => x.Nombre == user);

                    if (userLogin != null)
                    {
                        var grupoPersona = new GrupoPersona
                        {
                            GrupoId = grupoAdmin.Id,
                            EstaEliminado = false,
                            PersonaId = userLogin.First().PersonaId,
                            User = user
                        };

                        _unitOfWork.GrupoPersonaRepository.Add(grupoPersona);
                    }

                    _unitOfWork.Commit();
                }

                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Add Empresa - User: {user}", entity);
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

        public ResultDTO Update(EmpresaPersistenciaDTO entidad, string user)
        {
            try
            {
                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validacion de Datos de Empresa: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var entityActual = _unitOfWork.EmpresaRepository.GetById(entidad.Id);

                if (entityActual == null)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information("Ocurrio un error al obtener los datos de la Empresa", entidad);
                    }

                    return new ResultDTO
                    {
                        Message = "Ocurrio un error al obtener los datos del Empresa",
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

                var entity = _mapper.Map<Empresa>(entidad);

                entity.User = user;

                _unitOfWork.EmpresaRepository.Update(entity);

                _unitOfWork.Commit();

                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Update Empresa - User: {user}", entity);
                }

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<EmpresaDTO>(entity),
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

        public ResultDTO Delete(EmpresaDeleteDTO deleteDTO, string user)
        {
            try
            {
                var entidad = _unitOfWork.EmpresaRepository.GetById(deleteDTO.Id);

                if (entidad == null)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrio un error al obtener los datos de la Empresa. Id: {deleteDTO.Id}", deleteDTO);
                    }

                    return new ResultDTO
                    {
                        Message = "Ocurrio un error al obtener los datos",
                        State = false
                    };
                }

                _unitOfWork.EmpresaRepository.Delete(deleteDTO.Id, user);

                _unitOfWork.Commit();

                if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Delete Empresa - User: {user}", entidad);
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
                var entities = _unitOfWork.EmpresaRepository.GetAll(x => x.Include(ms => ms.TipoResponsabilidad)
                                     .Include(ib => ib.IngresoBruto)
                                     .Include(ms => ms.Localidad).ThenInclude(ms => ms.Provincia));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<EmpresaDTO>>(entities)
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Ocurrió un error al obtener los datos de la Empresa. {ex.Message}");
                }

                return new ResultDTO
                {
                    Message = ErrorException.Mensaje(ex),
                    State = false
                };
            }
        }

        public ResultDTO GetByFilter(EmpresaFilterDTO filter)
        {
            try
            {
                filter.CadenaBuscar = !string.IsNullOrEmpty(filter.CadenaBuscar) ? filter.CadenaBuscar : string.Empty;

                Expression<Func<Empresa, bool>> filtro = filtro => true;

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
                                || x.Abreviatura.ToLower().Contains(cadena.ToLower())
                                || x.Localidad.Descripcion.ToLower().Contains(cadena.ToLower())
                                || x.Localidad.Provincia.Descripcion.ToLower().Contains(cadena.ToLower())
                                || x.TipoResponsabilidad.Descripcion.ToLower().Contains(cadena.ToLower())
                                || x.IngresoBruto.Descripcion.ToLower().Contains(cadena.ToLower())
                                || x.Cuit == cadena));

                            primeraPasada = false;
                        }
                        else
                        {
                            filtro = filtro.Or(x => x.EstaEliminado == filter.VerEliminados
                                && (x.Descripcion.ToLower().Contains(cadena.ToLower())
                                || x.Abreviatura.ToLower().Contains(cadena.ToLower())
                                || x.Localidad.Descripcion.ToLower().Contains(cadena.ToLower())
                                || x.Localidad.Provincia.Descripcion.ToLower().Contains(cadena.ToLower())
                                || x.TipoResponsabilidad.Descripcion.ToLower().Contains(cadena.ToLower())
                                || x.IngresoBruto.Descripcion.ToLower().Contains(cadena.ToLower())
                                || x.Cuit == cadena));
                        }
                    }
                }
                else
                {
                    filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                && (x.Descripcion.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                || x.Abreviatura.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                || x.Localidad.Descripcion.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                || x.Localidad.Provincia.Descripcion.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                || x.TipoResponsabilidad.Descripcion.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                || x.IngresoBruto.Descripcion.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                || x.Cuit == filter.CadenaBuscar));
                }

                var entities = _unitOfWork.EmpresaRepository.GetByFilter(filtro, null,
                               x => x.Include(ms => ms.TipoResponsabilidad)
                                     .Include(ib => ib.IngresoBruto)
                                     .Include(ms => ms.Localidad).ThenInclude(ms => ms.Provincia));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<EmpresaDTO>>(entities)
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Ocurrió un error al obtener los datos de la Empresa. {ex.Message}");
                }

                return new ResultDTO
                {
                    Message = $"Ocurrió un error grave al obener los datos - {ex.Message}",
                    State = false
                };
            }
        }

        public ResultDTO GetById(Guid id)
        {
            try
            {
                var entity = _unitOfWork.EmpresaRepository
                    .GetById(id, x => x.Include(x => x.TipoResponsabilidad)
                                       .Include(x => x.IngresoBruto)
                                       .Include(x => x.Localidad).ThenInclude(x => x.Provincia));

                if (entity == null)
                {
                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrio un error al obtener los datos de Empresa. Id: {id}");
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
                    Data = _mapper.Map<EmpresaDTO>(entity)
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
            var entidads = _unitOfWork.EmpresaRepository.GetAll();

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
