using AutoMapper;
using Serilog;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.UnidadDeTrabajo;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Comprobante;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;

namespace Sidkenu.Servicio.Implementacion.Core.Comprobante
{
    public class Comprobante
    {
        public IUnidadDeTrabajo UnitOfWork { get; set; }
        public ILogger Logger { get; set; }
        public IConfiguracionServicio ConfiguracionServicio { get; set; }
        public IMapper Mapper { get; set; }
        public IArticuloServicio ArticuloServicio { get; set; }
        public IArticuloTemporalServicio ArticuloTemporalServicio { get; set; }
        public IOrdenFabricacionServicio OrdenFabricacionServicio { get; set; }
        public IConfiguracionCoreServicio ConfiguracionCoreServicio { get; set; }


        public virtual ResultDTO Add(ComprobanteDTO comprobante, string userLogin)
        {
            try
            {
                var nuevoComprobante = ObtenerTipoComprobante(comprobante);
                    
                nuevoComprobante.ClienteId = comprobante.Cliente.Id;
                nuevoComprobante.PersonaId = comprobante.Persona.Id;
                nuevoComprobante.EmpresaId = comprobante.EmpresaId;
                nuevoComprobante.SubTotal = comprobante.SubTotal;
                nuevoComprobante.Descuento = comprobante.Descuento;
                nuevoComprobante.Total = comprobante.Total;
                nuevoComprobante.Fecha = comprobante.Fecha;
                nuevoComprobante.User = userLogin;
                nuevoComprobante.EstaEliminado = false;

                nuevoComprobante.Detalles = new List<ComprobanteDetalle>();
                nuevoComprobante.Totales = new  List<ComprobanteTotales>();
                nuevoComprobante.MedioPagos = new List<MedioPago>();
                nuevoComprobante.Movimientos = new List<MovimientoCaja>();

                foreach (var detalle in comprobante.Detalles.Where(x=>x.TipoItem != Aplicacion.Constantes.TipoItemFactura.Fabricacion).ToList())
                {
                    var nuevoDetalle = new ComprobanteDetalle();

                    nuevoDetalle.ArticuloId = detalle.ArticuloId;
                    nuevoDetalle.Codigo = detalle.Codigo;
                    nuevoDetalle.Descripcion = detalle.Descripcion;
                    nuevoDetalle.Neto = detalle.Neto;
                    nuevoDetalle.Iva = detalle.Iva;
                    nuevoDetalle.Alicuota = detalle.Impuesto;
                    nuevoDetalle.Cantidad = detalle.Cantidad;
                    nuevoDetalle.SubTotal = detalle.SubTotal;
                    nuevoDetalle.Foto = detalle.TipoItem == Aplicacion.Constantes.TipoItemFactura.Fabricacion ? detalle.Foto : null;
                    nuevoDetalle.FechaEntrega = detalle.TipoItem == Aplicacion.Constantes.TipoItemFactura.Fabricacion ? detalle.FechaEntrega : null;
                    nuevoDetalle.EstaEliminado = false;
                    nuevoDetalle.TipoItem = detalle.TipoItem;
                    
                    nuevoComprobante.Detalles.Add(nuevoDetalle);

                    // Stock

                    
                }

                var totalesPorAlicuota = comprobante.Detalles
                    .GroupBy(x => x.Impuesto)
                    .Select(x => new ComprobanteTotales
                    {
                        Alicuota = x.Key,
                        Neto = x.Sum(n => n.Neto),
                        Iva = x.Sum(i => i.Iva)
                    });

                foreach (var item in totalesPorAlicuota.ToList())
                {
                    var nuevoTotal = new ComprobanteTotales();
                    nuevoTotal.Alicuota = item.Alicuota;
                    nuevoTotal.Neto = item.Neto;
                    nuevoTotal.Iva = item.Iva;
                    nuevoTotal.User = userLogin;
                    nuevoTotal.EstaEliminado = false;
                    
                    nuevoComprobante.Totales.Add(nuevoTotal);
                }

                return new ResultDTO
                {
                    State = true,
                    Data = nuevoComprobante,
                };
            }
            catch (Exception ex)
            {
                return new ResultDTO { State = false, Data = null, Message = "Ocurrio un error al crear el nuevo Comprobante Base" };
            }           
        }

        public virtual ResultDTO GetComprobantesPendientesCobroVentaMostrador(Guid id, Guid empresaId, string cadenaBuscar)
        {
            try
            {
                return new ResultDTO
                {
                    State = true,
                };
            }
            catch (Exception ex)
            {
                return new ResultDTO { State = false, Data = null, Message = "Ocurrio un error al crear el nuevo Comprobante Base" };
            }
        }

        private Dominio.Entidades.Core.Comprobante ObtenerTipoComprobante(ComprobanteDTO comprobante)
        {
            if (comprobante is ComprobanteVentaDTO)
            {
                return new Dominio.Entidades.Core.ComprobanteVenta();
            }

            if (comprobante is ComprobanteGastoDTO)
            {
                return new Dominio.Entidades.Core.ComprobanteGasto();
            }

            if (comprobante is ComprobanteTransferenciaDTO)
            {
                return new Dominio.Entidades.Core.ComprobanteTransferencia();
            }

            return new Dominio.Entidades.Core.Comprobante();
        }
    }
}
