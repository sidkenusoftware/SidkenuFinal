using AutoMapper;
using Serilog;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.TipoDocumento;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;

namespace Sidkenu.Servicio.Implementacion.Core
{
    public class TipoDocumentoServicio : ServicioBase, ITipoDocumentoServicio
    {
        public TipoDocumentoServicio(IUnidadDeTrabajo unitOfWork,
                                 IMapper mapper,
                                 ILogger logger,
                                 IConfiguracionServicio configuracionServicio)
                                 : base(unitOfWork, mapper, logger, configuracionServicio)
        {

        }

        public ResultDTO GetAll()
        {
            try
            {
                var entities = _unitOfWork.TipoDocumentoRepository.GetAll();

                return new ResultDTO
                {
                    State = true,
                    Data = _mapper.Map<IEnumerable<TipoDocumentoDTO>>(entities)
                };
            }
            catch (Exception ex)
            {
                if (_configuracionDTO != null && _configuracionDTO.LogError)
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
