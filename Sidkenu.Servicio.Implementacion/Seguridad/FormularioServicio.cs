using AutoMapper;
using Serilog;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Seguridad.Formulario;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Seguridad;

namespace Sidkenu.Servicio.Implementacion.Seguridad
{
    public class FormularioServicio : ServicioBase, IFormularioServicio
    {
        public FormularioServicio(IUnidadDeTrabajo unitOfWork,
                                    IMapper mapper,
                                    ILogger logger,
                                    IConfiguracionServicio configuracionServicio)
                                    : base(unitOfWork, mapper, logger, configuracionServicio)
        {
        }

        public ResultDTO Add(List<FormularioDTO> formularios, string userLogin)
        {
            try
            {
                foreach (var formulario in formularios.Where(x => !x.ExisteBase).ToList())
                {
                    var entity = _mapper.Map<Formulario>(formulario);

                    entity.User = userLogin;
                    entity.EstaEliminado = false;

                    _unitOfWork.FormularioRepository.Add(entity);

                    if (base._configuracionDTO != null && base._configuracionDTO.LogInformacion)
                    {
                        _logger.Information($"Add Formulario/Pantalla - Form: {entity.DescripcionCompleta} - User: {userLogin}", entity);
                    }
                }

                _unitOfWork.Commit();

                return new ResultDTO
                {
                    State = true,
                    Message = "Los datos se grabaron correctamente"
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error {ex.Message} - User: {userLogin}.");
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
                var result = _unitOfWork.FormularioRepository.GetAll();

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<FormularioDTO>>(result)
                };
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
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
    }
}
