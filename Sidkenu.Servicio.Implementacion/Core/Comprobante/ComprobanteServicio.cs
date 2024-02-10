using AutoMapper;
using Serilog;
using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Comprobante;
using Sidkenu.Servicio.Implementacion.Base;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;

namespace Sidkenu.Servicio.Implementacion.Core.Comprobante
{
    public class ComprobanteServicio : ServicioBase, IComprobanteServicio
    {
        private Dictionary<TipoComprobante, string> _diccionarioComprobante;

        private readonly IArticuloServicio _articuloServicio;
        private readonly IArticuloTemporalServicio _articuloTemporalServicio;
        private readonly IOrdenFabricacionServicio _ordenFabricacionServicio;
        private readonly IConfiguracionCoreServicio _configuracionCoreServicio;

        public ComprobanteServicio(IUnidadDeTrabajo unitOfWork,
                                   IMapper mapper,
                                   ILogger logger,
                                   IConfiguracionServicio configuracionServicio,
                                   IArticuloServicio articuloServicio,
                                   IArticuloTemporalServicio articuloTemporalServicio,
                                   IOrdenFabricacionServicio ordenFabricacionServicio,
                                   IConfiguracionCoreServicio configuracionCoreServicio)
                                   : base(unitOfWork, mapper, logger, configuracionServicio)
        {
            _articuloServicio = articuloServicio;
            _articuloTemporalServicio = articuloTemporalServicio;
            _ordenFabricacionServicio = ordenFabricacionServicio;
            _configuracionCoreServicio = configuracionCoreServicio;

            _diccionarioComprobante = new Dictionary<TipoComprobante, string>();

            CargarDiccionario(_diccionarioComprobante);            
        }

        private void CargarDiccionario(Dictionary<TipoComprobante, string> diccionario)
        {
            diccionario.Add(TipoComprobante.Venta, "Sidkenu.Servicio.Implementacion.Core.Comprobante.ComprobanteVenta");
            diccionario.Add(TipoComprobante.Salon, "Sidkenu.Servicio.Implementacion.Core.Comprobante.ComprobanteSalon");
            diccionario.Add(TipoComprobante.Compra, "Sidkenu.Servicio.Implementacion.Core.Comprobante.ComprobanteCompra");
            diccionario.Add(TipoComprobante.NotaCredito, "Sidkenu.Servicio.Implementacion.Core.Comprobante.ComprobanteNotaCredito");
            diccionario.Add(TipoComprobante.NotaDebito, "Sidkenu.Servicio.Implementacion.Core.Comprobante.ComprobanteNotaDebito");
            diccionario.Add(TipoComprobante.Gastos, "Sidkenu.Servicio.Implementacion.Core.Comprobante.ComprobanteGastos");
            diccionario.Add(TipoComprobante.TransferenciaCaja, "Sidkenu.Servicio.Implementacion.Core.Comprobante.ComprobanteTransferencia");
        }

        public ResultDTO Add(ComprobanteDTO comprobante, string userLogin)
        {
            // Valida que este en el diccionario
            if (!_diccionarioComprobante.TryGetValue(comprobante.TipoComprobante, out string? tipoComprobante))
            {
                throw new Exception("No se encontro el Comprobante seleccionado");
            }

            // Valida el tipo de comprobante (clase)
            var _comprobante = Type.GetType(tipoComprobante) ?? throw new Exception("Ocurrio un error al obtener el comprobante");

            var comprobanteResult = Activator.CreateInstance(_comprobante) as Comprobante;

            if (comprobanteResult != null)
            {
                comprobanteResult.UnitOfWork = base._unitOfWork;
                comprobanteResult.Logger = base._logger;
                comprobanteResult.ConfiguracionServicio = base._configuracionServicio;
                comprobanteResult.Mapper = base._mapper;
                comprobanteResult.ArticuloServicio = _articuloServicio;
                comprobanteResult.ArticuloTemporalServicio = _articuloTemporalServicio;
                comprobanteResult.OrdenFabricacionServicio = _ordenFabricacionServicio;
                comprobanteResult.ConfiguracionCoreServicio = _configuracionCoreServicio;

                return comprobanteResult.Add(comprobante, userLogin);
            }
            else
            {
                return new ResultDTO() 
                {
                    State = false,
                    Message = "Ocurrió un error al grabar la Factura",
                };
            }
        }

        public ResultDTO GetComprobantesPendientesCobroVentaMostrador(Guid id, Guid empresaId, string cadenaBuscar)
        {
            // Valida que este en el diccionario
            if (!_diccionarioComprobante.TryGetValue(TipoComprobante.Venta, out string? tipoComprobante))
            {
                throw new Exception("No se encontro el Comprobante seleccionado");
            }

            // Valida el tipo de comprobante (clase)
            var _comprobante = Type.GetType(tipoComprobante) ?? throw new Exception("Ocurrio un error al obtener el comprobante");

            var comprobanteResult = Activator.CreateInstance(_comprobante) as Comprobante;

            if (comprobanteResult != null)
            {
                comprobanteResult.UnitOfWork = base._unitOfWork;
                comprobanteResult.Logger = base._logger;
                comprobanteResult.ConfiguracionServicio = base._configuracionServicio;
                comprobanteResult.Mapper = base._mapper;
                comprobanteResult.ArticuloServicio = _articuloServicio;
                comprobanteResult.ArticuloTemporalServicio = _articuloTemporalServicio;
                comprobanteResult.OrdenFabricacionServicio = _ordenFabricacionServicio;
                comprobanteResult.ConfiguracionCoreServicio = _configuracionCoreServicio;

                return comprobanteResult.GetComprobantesPendientesCobroVentaMostrador(id, empresaId, cadenaBuscar);
            }
            else
            {
                return new ResultDTO()
                {
                    State = false,
                    Message = "Ocurrió un error al grabar la Factura",
                };
            }
        }
    }
}
