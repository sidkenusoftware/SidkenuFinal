using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidkenu.Aplicacion.Comun;
using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Cliente;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;
using System.Linq.Expressions;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class ClienteServicio : ServicioBase, IClienteServicio
    {
        private readonly IValidator<ClientePersistenciaDTO> _validatorDto;

        public ClienteServicio(IUnidadDeTrabajo unitOfWork,
                               IMapper mapper,
                               ILogger logger,
                               IConfiguracionServicio configuracionServicio,
                               IValidator<ClientePersistenciaDTO> validatorDto)
                               : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            _validatorDto = validatorDto;
        }

        public ResultDTO Add(ClientePersistenciaDTO entidad, string user)
        {
            try
            {
                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validación de Datos del Cliente: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var existeEntidad = VerificarSiExiste(entidad.Documento);

                if (existeEntidad)
                    return new ResultDTO
                    {
                        Message = "Los datos ingresados ya existen",
                        State = false,
                    };

                var entity = _mapper.Map<Dominio.Entidades.Core.Cliente>(entidad);

                entity.User = user;
                entity.EstaEliminado = false;

                _unitOfWork.ClienteRepository.Add(entity);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Add Cliente - User: {user}", entity);
                }

                return new ResultDTO
                {
                    State = true,
                    Message = "Los datos se grabaron correctamente",
                    Data = _mapper.Map<ClienteDTO>(entity)
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

        public ResultDTO Update(ClientePersistenciaDTO entidad, string user)
        {
            try
            {
                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Error de validación de Datos de Cliente: {mensaje}");
                    }

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var entityActual = _unitOfWork.ClienteRepository.GetById(entidad.Id);

                if (entityActual == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information("Ocurrió un error al obtener los datos de la Cliente", entidad);
                    }

                    return new ResultDTO
                    {
                        Message = "Ocurrió un error al obtener los datos del Cliente",
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

                var entity = _mapper.Map<Dominio.Entidades.Core.Cliente>(entidad);

                entity.User = user;

                _unitOfWork.ClienteRepository.Update(entity);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Update Cliente - User: {user}", entity);
                }

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<ClienteDTO>(entity),
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

        public ResultDTO Delete(ClienteDeleteDTO deleteDTO, string user)
        {
            try
            {
                var entidad = _unitOfWork.ClienteRepository.GetById(deleteDTO.Id);

                if (entidad == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrió un error al obtener los datos de la Cliente. Id: {deleteDTO.Id}", deleteDTO);
                    }

                    return new ResultDTO
                    {
                        Message = "Ocurrió un error al obtener los datos",
                        State = false
                    };
                }

                _unitOfWork.ClienteRepository.Delete(deleteDTO.Id, user);

                _unitOfWork.Commit();

                if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                {
                    _logger.Information($"Delete Cliente - User: {user}", entidad);
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
                var entities = _unitOfWork.ClienteRepository
                    .GetAll(x => x.Include(z => z.TipoDocumento)
                                .Include(z => z.TipoResponsabilidad)
                                .Include(z => z.ListaPrecio));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<ClienteDTO>>(entities)
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Ocurrió un error al obtener los datos de la Cliente. {ex.Message}");
                }

                return new ResultDTO
                {
                    Message = ErrorException.Mensaje(ex),
                    State = false
                };
            }
        }

        public ResultDTO GetByFilter(ClienteFilterDTO filter)
        {
            try
            {
                filter.CadenaBuscar = !string.IsNullOrEmpty(filter.CadenaBuscar) ? filter.CadenaBuscar : string.Empty;

                Expression<Func<Dominio.Entidades.Core.Cliente, bool>> filtro = filtro => true;

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
                                || x.Documento == cadena));

                            primeraPasada = false;
                        }
                        else
                        {
                            filtro = filtro.Or(x => x.EstaEliminado == filter.VerEliminados
                                && (x.RazonSocial.ToLower().Contains(cadena.ToLower())
                                || x.Documento == cadena));
                        }
                    }
                }
                else
                {
                    filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                && (x.RazonSocial.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                || x.Documento == filter.CadenaBuscar));
                }

                filtro = filtro.And(x => x.Documento != ClientePorDefecto.NumeroDocumentoConsumidorFinal);

                var entities = _unitOfWork.ClienteRepository.GetByFilter(filtro, null, x => x.Include(z => z.TipoDocumento)
                                .Include(z => z.TipoResponsabilidad)
                                .Include(z => z.ListaPrecio));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<ClienteDTO>>(entities)
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Ocurrió un error al obtener los datos de la Cliente. {ex.Message}");
                }

                return new ResultDTO
                {
                    Message = $"Ocurrió un error grave al obtener los datos - {ex.Message}",
                    State = false
                };
            }
        }

        public ResultDTO GetByFilterLookUp(ClienteFilterDTO filter)
        {
            try
            {
                filter.CadenaBuscar = !string.IsNullOrEmpty(filter.CadenaBuscar) ? filter.CadenaBuscar : string.Empty;

                Expression<Func<Dominio.Entidades.Core.Cliente, bool>> filtro = filtro => true;

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
                                || x.Documento == cadena));

                            primeraPasada = false;
                        }
                        else
                        {
                            filtro = filtro.Or(x => x.EstaEliminado == filter.VerEliminados
                                && (x.RazonSocial.ToLower().Contains(cadena.ToLower())
                                || x.Documento == cadena));
                        }
                    }
                }
                else
                {
                    filtro = filtro.And(x => x.EstaEliminado == filter.VerEliminados
                                && (x.RazonSocial.ToLower().Contains(filter.CadenaBuscar.ToLower())
                                || x.Documento == filter.CadenaBuscar));
                }

                filtro = filtro.And(x => x.Documento != ClientePorDefecto.NumeroDocumentoConsumidorFinal);

                var entities = _unitOfWork.ClienteRepository.GetByFilter(filtro, null, x => x.Include(z => z.TipoDocumento)
                                .Include(z => z.TipoResponsabilidad));

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<ClienteDTO>>(entities)
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Ocurrió un error al obtener los datos de la Cliente. {ex.Message}");
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
                var entity = _unitOfWork.ClienteRepository
                    .GetById(id, x => x.Include(z => z.TipoDocumento)
                                       .Include(z => z.TipoResponsabilidad)
                                       .Include(z => z.ListaPrecio));

                if (entity == null)
                {
                    if (_configuracionDTO != null && _configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Ocurrió un error al obtener los datos del cliente. Id: {id}");
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
                    Data = _mapper.Map<ClienteDTO>(entity)
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

        public ResultDTO GetConsumidorFinal()
        {
            try
            {
                var entity = _unitOfWork.ClienteRepository
                    .GetByFilter(x => x.Documento == ClientePorDefecto.NumeroDocumentoConsumidorFinal);

                if (entity == null)
                {
                    return new ResultDTO
                    {
                        State = false,
                        Message = "Ocurrio un error al obtener el cliente consumidor final"
                    };
                }

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<ClienteDTO>(entity.FirstOrDefault())
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message}. Documento: {ClientePorDefecto.NumeroDocumentoConsumidorFinal}");
                }

                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        public ResultDTO GetByNumeroDocumento(string numeroDocumento)
        {
            try
            {
                Expression<Func<Dominio.Entidades.Core.Cliente, bool>> filtro = filtro => true;

                filtro = filtro.And(x => x.Documento == numeroDocumento);

                var entities = _unitOfWork.ClienteRepository.GetByFilter(filtro, null, x => x.Include(z => z.TipoDocumento)
                                .Include(z => z.TipoResponsabilidad));

                var result = _mapper.Map<IEnumerable<ClienteDTO>>(entities);

                return new ResultDTO
                {
                    State = true,
                    Data = result.FirstOrDefault()
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Ocurrió un error al obtener los datos de la Cliente. {ex.Message}");
                }

                return new ResultDTO
                {
                    Message = $"Ocurrió un error grave al obtener los datos - {ex.Message}",
                    State = false
                };
            }
        }

        // ------------------------------------------------------------------------------------------------------ //
        // ------------------------------             Metodos Privados              ----------------------------- //
        // ------------------------------------------------------------------------------------------------------ //
        private bool VerificarSiExiste(string descripcion, Guid? id = null)
        {
            var entidads = _unitOfWork.ClienteRepository.GetAll();

            if (entidads != null && entidads.Any())
            {
                if (id == null)
                {
                    return entidads.Any(x => x.Documento.ToLower() == descripcion.ToLower());
                }
                else
                {
                    return entidads.Any(x => x.Id != id.Value
                        && x.Documento.ToLower() == descripcion.ToLower());
                }
            }
            else
            {
                return false;
            }
        }
    }
}
