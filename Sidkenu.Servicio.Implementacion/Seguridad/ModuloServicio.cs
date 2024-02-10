using AutoMapper;
using FluentValidation;
using Serilog;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.Modulo;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Seguridad;
using Sidkenu.Servicio.Validator;

namespace Sidkenu.Servicio.Implementacion.Seguridad
{
    public class ModuloServicio : ServicioBase, IModuloServicio
    {
        private readonly IValidator<ModuloPersistenciaDTO> _validatorDto;

        public ModuloServicio(IUnidadDeTrabajo unitOfWork,
            IMapper mapper,
            ILogger logger,
            IConfiguracionServicio configuracionServicio,
            IValidator<ModuloPersistenciaDTO> validatorDto)
            : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            this._validatorDto = validatorDto;
        }

        public ResultDTO AddOrUpdate(ModuloPersistenciaDTO entidad, string user)
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

                var entity = _mapper.Map<Modulo>(entidad);

                entity.User = user;

                if (entidad.Id != Guid.Empty)
                {
                    _unitOfWork.ModuloRepository.Update(entity);
                }
                else
                {
                    _unitOfWork.ModuloRepository.Add(entity);
                }

                _unitOfWork.Commit();

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<ModuloDTO>(entity),
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
                var entities = _unitOfWork.ModuloRepository.
                    GetByFilter(x => x.EmpresaId == empresaId);

                if (entities == null)
                {
                    return new ResultDTO
                    {
                        State = true,
                        Data = null
                    };
                }

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<ModuloDTO>(entities.FirstOrDefault())
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
    }
}
