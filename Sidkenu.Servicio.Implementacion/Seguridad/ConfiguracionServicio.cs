using AutoMapper;
using FluentValidation;
using Serilog;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.Configuracion;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;

namespace Sidkenu.Servicio.Implementacion.Seguridad
{
    public class ConfiguracionServicio : ServicioBase, IConfiguracionServicio
    {
        private readonly IValidator<ConfiguracionPersistenciaDTO> _validatorDto;

        public ConfiguracionServicio(IUnidadDeTrabajo unitOfWork,
                                     IMapper mapper,
                                     ILogger logger,
                                     IValidator<ConfiguracionPersistenciaDTO> validatorDto = null)
                                     : base(unitOfWork, mapper, logger)
        {
            _validatorDto = validatorDto;
        }

        public ResultDTO AddOrUpdate(ConfiguracionPersistenciaDTO entidad, string user)
        {
            try
            {
                var validationResult = _validatorDto.Validate(entidad);

                if (!validationResult.IsValid)
                {
                    var mensaje = ErrorValidator.ObtenerErrores(validationResult);

                    return new ResultDTO
                    {
                        State = false,
                        Message = mensaje,
                    };
                }

                var entityActual = _unitOfWork.ConfiguracionSeguridadRepository.GetById(entidad.Id);

                var entity = _mapper.Map<ConfiguracionSeguridad>(entidad);

                entity.User = user;

                if (entityActual == null)
                {
                    // Insert                    
                    _unitOfWork.ConfiguracionSeguridadRepository.Add(entity);
                }
                else
                {
                    // Update
                    _unitOfWork.ConfiguracionSeguridadRepository.Update(entity);
                }

                _unitOfWork.Commit();

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<ConfiguracionDTO>(entity),
                    Message = "Los datos se actualizaron correctamente"
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

        public ResultDTO Get()
        {
            try
            {
                var entities = _unitOfWork.ConfiguracionSeguridadRepository.GetAll();

                if (entities == null && entities.Any())
                {
                    return new ResultDTO
                    {
                        State = false,
                        Data = null
                    };
                }

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<ConfiguracionDTO>(entities.FirstOrDefault())
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && base._configuracionDTO != null && base._configuracionDTO.LogError)
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
    }
}
