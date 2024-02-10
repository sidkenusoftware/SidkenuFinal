using AutoMapper;
using Serilog;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Seguridad.Configuracion;
using Sidkenu.Servicio.Interface.Base;
using Sidkenu.Servicio.Interface.Seguridad;

namespace Sidkenu.Servicio.Implementacion.Base
{
    public class ServicioBase : IServicioBase
    {
        protected readonly IUnidadDeTrabajo _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly ILogger _logger;
        protected readonly IConfiguracionServicio _configuracionServicio;

        protected ConfiguracionDTO _configuracionDTO;

        public ServicioBase(IUnidadDeTrabajo unitOfWork,
            IMapper mapper,
            ILogger logger,
            IConfiguracionServicio configuracionServicio)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _configuracionServicio = configuracionServicio;

            var result = _configuracionServicio.Get();

            if (result != null && result.State)
            {
                _configuracionDTO = (ConfiguracionDTO)result.Data;
            }
            else
            {
                _configuracionDTO = (ConfiguracionDTO)null;

                _logger.Error("no se pudo cargar la configuración del Sistema");
            }
        }

        public ServicioBase(IUnidadDeTrabajo unitOfWork,
            IMapper mapper,
            ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }         
    }
}
