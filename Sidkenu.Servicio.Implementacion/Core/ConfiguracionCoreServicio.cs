using AutoMapper;
using FluentValidation;
using Serilog;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.ConfiguracionCore;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class ConfiguracionCoreServicio : ServicioBase, IConfiguracionCoreServicio
    {
        private readonly IValidator<ConfiguracionCorePersistenciaDTO> _validatorDto;

        public ConfiguracionCoreServicio(IUnidadDeTrabajo unitOfWork,
                                         IMapper mapper,
                                         ILogger logger,
                                         IConfiguracionServicio configuracionServicio,
                                         IValidator<ConfiguracionCorePersistenciaDTO> validatorDto)
                                         : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            _validatorDto = validatorDto;
        }

        public ResultDTO AddOrUpdate(ConfiguracionCorePersistenciaDTO entidad, string userLogin)
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

                var entityActual = _unitOfWork.ConfiguracionCoreRepository
                    .GetById(entidad.Id);

                var entity = _mapper.Map<ConfiguracionCore>(entidad);

                entity.User = userLogin;

                if (entityActual == null)
                {
                    // Insert                    
                    _unitOfWork.ConfiguracionCoreRepository.Add(entity);
                }
                else
                {
                    // Update
                    _unitOfWork.ConfiguracionCoreRepository.Update(entity);
                }

                _unitOfWork.Commit();

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<ConfiguracionCoreDTO>(entity),
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

        public ResultDTO Get(Guid empresaId)
        {
            try
            {
                var entities = _unitOfWork.ConfiguracionCoreRepository
                    .GetByFilter(x=>x.EmpresaId == empresaId);

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
                    Data = _mapper.Map<ConfiguracionCoreDTO>(entities.FirstOrDefault())
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
