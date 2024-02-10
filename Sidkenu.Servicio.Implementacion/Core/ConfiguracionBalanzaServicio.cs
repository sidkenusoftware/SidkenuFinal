using AutoMapper;
using Serilog;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.ConfiguracionBalanza;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class ConfiguracionBalanzaServicio : ServicioBase, IConfiguracionBalanzaServicio
    {
        public ConfiguracionBalanzaServicio(IUnidadDeTrabajo unitOfWork,
                                            IMapper mapper,
                                            ILogger logger,
                                            IConfiguracionServicio configuracionServicio)
                                            : base(unitOfWork, mapper, logger, configuracionServicio)
        {
        }

        public ResultDTO AddOrUpdate(ConfiguracionBalanzaPersistenciaDTO entidad, string userLogin)
        {
            try
            {
                var entityActual = _unitOfWork.ConfiguracionBalanzaRepository
                    .GetById(entidad.Id);

                var entity = _mapper.Map<ConfiguracionBalanza>(entidad);

                entity.User = userLogin;

                if (entityActual == null)
                {
                    // Insert                    
                    _unitOfWork.ConfiguracionBalanzaRepository.Add(entity);
                }
                else
                {
                    // Update
                    _unitOfWork.ConfiguracionBalanzaRepository.Update(entity);
                }

                _unitOfWork.Commit();

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<ConfiguracionBalanzaDTO>(entity),
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
                var entities = _unitOfWork.ConfiguracionBalanzaRepository
                    .GetByFilter(x => x.EmpresaId == empresaId);

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
                    Data = _mapper.Map<ConfiguracionBalanzaDTO>(entities.FirstOrDefault())
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
