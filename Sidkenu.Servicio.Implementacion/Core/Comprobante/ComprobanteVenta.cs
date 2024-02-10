using Azure;
using Microsoft.EntityFrameworkCore;
using Sidkenu.Aplicacion.Constantes;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Servicio.DTOs.Base;
using Sidkenu.Servicio.DTOs.Core.Articulo;
using Sidkenu.Servicio.DTOs.Core.Comprobante;
using Sidkenu.Servicio.DTOs.Core.ConfiguracionCore;
using Sidkenu.Servicio.DTOs.Core.MedioPago;
using Sidkenu.Servicio.DTOs.Core.OrdenFabricacion;

namespace Sidkenu.Servicio.Implementacion.Core.Comprobante
{
    public class ComprobanteVenta : Comprobante
    {
        public override ResultDTO Add(ComprobanteDTO comprobante, string userLogin)
        {
            try
            {
                var comprobanteResult = base.Add(comprobante, userLogin);

                if (comprobanteResult != null && comprobanteResult.State)
                {
                    var nuevoComprobante = (Dominio.Entidades.Core.ComprobanteVenta)comprobanteResult.Data;

                    foreach (var detalle in comprobante.Detalles.Where(x => x.TipoItem == Aplicacion.Constantes.TipoItemFactura.Fabricacion))
                    {
                        var nuevoDetalle = new ComprobanteDetalle();

                        var articuloTemporalId = AsignarDatosArticuloTemporal(detalle, comprobante.EmpresaId, userLogin);

                        nuevoDetalle.ArticuloId = articuloTemporalId;
                        nuevoDetalle.Codigo = detalle.Codigo;
                        nuevoDetalle.Descripcion = detalle.Descripcion;
                        nuevoDetalle.Neto = detalle.Neto;
                        nuevoDetalle.Iva = detalle.Iva;
                        nuevoDetalle.Alicuota = detalle.Impuesto;
                        nuevoDetalle.Cantidad = detalle.Cantidad;
                        nuevoDetalle.SubTotal = detalle.SubTotal;
                        nuevoDetalle.Foto = detalle.Foto;
                        nuevoDetalle.FechaEntrega = detalle.FechaEntrega;
                        nuevoDetalle.EstaEliminado = false;
                        nuevoDetalle.TipoItem = detalle.TipoItem;

                        nuevoComprobante.Detalles.Add(nuevoDetalle);

                        var ordenFabricacion = AsignarDatosOrdenFabricacion(detalle, comprobante.EmpresaId, articuloTemporalId, userLogin);
                    }

                    /// Medios de Pagos

                    foreach (var mp in comprobante.MedioPagos.ToList())
                    {
                        if (mp is MedioPagoEfectivoDTO)
                        {
                            var mpEfectivo = AsignarMedioPagoEfectivo(comprobante, mp);
                            mpEfectivo.User = userLogin;

                            nuevoComprobante.MedioPagos.Add(mpEfectivo);

                            // Movimiento de Caja 

                            var nuevoMovCaja = AsignarMovimientoCaja(comprobante.Fecha,
                                mpEfectivo.Capital,
                                mpEfectivo.Interes,
                                $"Op => Efectivo",
                                comprobante.CajaDetalleId,
                                TipoOperacionMovimiento.Efectivo,
                                userLogin);

                            nuevoComprobante.Movimientos.Add(nuevoMovCaja);
                        }
                        else if (mp is MedioPagoChequeDTO)
                        {
                            var mpCheque = AsignarMedioPagoCheque(comprobante, mp);
                            mpCheque.User = userLogin;

                            nuevoComprobante.MedioPagos.Add(mpCheque);

                            // Movimiento de Caja

                            var _banco = UnitOfWork.BancoRepository.GetById(mpCheque.BancoId);

                            var nuevoMovCaja = AsignarMovimientoCaja(comprobante.Fecha,
                                mpCheque.Capital,
                                mpCheque.Interes,
                                $"Op => Cheque - Nro: {mpCheque.NumeroCheque} - Banco: {_banco.Descripcion} - Venc: {mpCheque.FechaVencimiento.ToShortDateString()}",
                                comprobante.CajaDetalleId,
                                TipoOperacionMovimiento.Cheque,
                                userLogin);

                            nuevoComprobante.Movimientos.Add(nuevoMovCaja);
                        }
                        else if (mp is MedioPagoTransferenciaDTO)
                        {
                            var mpTransferencia = AsignarMedioPagoTransferencia(comprobante, mp);
                            mpTransferencia.User = userLogin;

                            nuevoComprobante.MedioPagos.Add(mpTransferencia);

                            // Movimiento de Caja

                            var _banco = UnitOfWork.BancoRepository.GetById(mpTransferencia.BancoId);

                            var nuevoMovCaja = AsignarMovimientoCaja(comprobante.Fecha,
                                mpTransferencia.Capital,
                                mpTransferencia.Interes,
                                $"Op => Transferencia - Nro: {mpTransferencia.NumeroTransferencia} - Banco: {_banco.Descripcion} - Titular: {mpTransferencia.NombreTitular}",
                                comprobante.CajaDetalleId,
                                TipoOperacionMovimiento.Transferencia,
                                userLogin);

                            nuevoComprobante.Movimientos.Add(nuevoMovCaja);
                        }
                        else if (mp is MedioPagoTarjetaDTO)
                        {
                            var mpTarjeta = AsignarMedioPagoTarjeta(comprobante, mp);
                            mpTarjeta.User = userLogin;

                            nuevoComprobante.MedioPagos.Add(mpTarjeta);

                            // Movimiento de Caja

                            var planTarjeta = UnitOfWork.PlanTarjetaRepository.GetById(mpTarjeta.PlanTarjetaId, i => i.Include(x => x.Tarjeta));

                            var nuevoMovCaja = AsignarMovimientoCaja(comprobante.Fecha,
                                mpTarjeta.Capital,
                                mpTarjeta.Interes,
                                $"Op => Tarjeta - {planTarjeta.Tarjeta.Descripcion} - Plan: {planTarjeta.Descripcion} - Cupon: {mpTarjeta.NumeroCupon}",
                                comprobante.CajaDetalleId,
                                TipoOperacionMovimiento.Tarjeta,
                                userLogin);

                            nuevoComprobante.Movimientos.Add(nuevoMovCaja);
                        }
                        else if (mp is MedioPagoCtaCteDTO)
                        {
                            var mpCtaCte = AsignarMedioPagoCtaCte(comprobante, mp);
                            mpCtaCte.User = userLogin;

                            nuevoComprobante.MedioPagos.Add(mpCtaCte);

                            var _nuevaCtaCteCliente = AsignarCuentaCorriente(comprobante, userLogin, mpCtaCte);

                            UnitOfWork.CuentaCorrienteClienteRepository.Add(_nuevaCtaCteCliente);

                            var _cliente = UnitOfWork.ClienteRepository.GetById(mpCtaCte.ClienteId, i => i.Include(z => z.TipoDocumento));

                            var nuevoMovCaja = AsignarMovimientoCaja(comprobante.Fecha,
                                mpCtaCte.Capital,
                                mpCtaCte.Interes,
                                $"Op => Cta. Cte. {_cliente.RazonSocial} - {_cliente.TipoDocumento.Descripcion}: {_cliente.Documento}",
                                comprobante.CajaDetalleId,
                                TipoOperacionMovimiento.CuentaCorriente,
                                userLogin);

                            nuevoComprobante.Movimientos.Add(nuevoMovCaja);
                        }
                    }

                    UnitOfWork.ComprobanteVentaRepository.Add(nuevoComprobante);

                    // Descontar Stock

                    var _configCoreResult = ConfiguracionCoreServicio.Get(comprobante.EmpresaId);

                    if (_configCoreResult == null || !_configCoreResult.State)
                    {
                        return new ResultDTO
                        {
                            State = false,
                            Message = "Ocurrio un error al obtener la configuracion del deposito seleccionado para la venta"
                        };
                    }

                    var _configCore = (ConfiguracionCoreDTO)_configCoreResult.Data;


                    var _articulosDescontar = nuevoComprobante.Detalles
                        .Where(x => x.ArticuloId.HasValue && x.TipoItem != TipoItemFactura.Fabricacion)
                        .GroupBy(x => x.ArticuloId)
                        .Select(x => new
                        {
                            Id = x.Key,
                            Cantidad = x.Sum(s => s.Cantidad)
                        }).ToList();


                    foreach (var _articulo in _articulosDescontar.ToList())
                    {
                        var _articulosDepositos = UnitOfWork.ArticuloDepositoRepository
                            .GetByFilter(x => x.DepositoId == _configCore.DepositoPorDefectoParaVentaId
                                              && x.ArticuloId == _articulo.Id, null,
                                              i => i.Include(z => z.Articulo)
                                                  .Include(z => z.Articulo).ThenInclude(z => z.ArticuloHijoKits)
                                                  .Include(z => z.Articulo).ThenInclude(z => z.ArticuloFormulas));

                        if (_articulosDepositos == null)
                        {
                            return new ResultDTO
                            {
                                State = false,
                                Message = "Ocurrio un error al obtener el articulo para descontar Stock"
                            };
                        }

                        var _artDep = _articulosDepositos.First();

                        _artDep.Cantidad -= _articulo.Cantidad;

                        UnitOfWork.ArticuloDepositoRepository.Update(_artDep);

                        if (_artDep.Articulo.TipoArticulo == TipoArticulo.Kit)
                        {
                            foreach (var artkit in _artDep.Articulo.ArticuloHijoKits)
                            {
                                var _articulosDepositosKits = UnitOfWork.ArticuloDepositoRepository
                            .GetByFilter(x => x.DepositoId == _configCore.DepositoPorDefectoParaVentaId
                                              && x.ArticuloId == artkit.ArticuloHijoId);

                                if (_articulosDepositosKits == null)
                                {
                                    return new ResultDTO
                                    {
                                        State = false,
                                        Message = "Ocurrio un error al obtener el articulo para descontar Stock"
                                    };
                                }

                                var _artDepkit = _articulosDepositosKits.First();

                                _artDepkit.Cantidad -= (_articulo.Cantidad * artkit.Cantidad);

                                UnitOfWork.ArticuloDepositoRepository.Update(_artDepkit);
                            }
                        }

                        if (_artDep.Articulo.TipoArticulo == TipoArticulo.Formula)
                        {
                            foreach (var artForm in _artDep.Articulo.ArticuloFormulas)
                            {
                                var _articulosDepositosForms = UnitOfWork.ArticuloDepositoRepository
                            .GetByFilter(x => x.DepositoId == _configCore.DepositoPorDefectoParaVentaId
                                              && x.ArticuloId == artForm.ArticuloSecundarioId);

                                if (_articulosDepositosForms == null)
                                {
                                    return new ResultDTO
                                    {
                                        State = false,
                                        Message = "Ocurrio un error al obtener el articulo para descontar Stock"
                                    };
                                }

                                var _artDepForm = _articulosDepositosForms.First();

                                _artDepForm.Cantidad -= (_articulo.Cantidad * artForm.Cantidad);

                                UnitOfWork.ArticuloDepositoRepository.Update(_artDepForm);
                            }
                        }
                    }


                    UnitOfWork.Commit();

                    return new ResultDTO
                    {
                        State = true,
                        Message = "Los datos se grabaron correctamente"
                    };
                }
                else
                {
                    return comprobanteResult;
                }
            }
            catch (Exception ex)
            {
                return new ResultDTO
                {
                    Message = ex.Message,
                    State = false
                };
            }
        }

        private static MovimientoCaja AsignarMovimientoCaja(DateTime fecha, decimal capital, decimal interes, string descripcion, Guid cajaDetalleId, TipoOperacionMovimiento operacion, string userLogin)
        {
            var nuevoMovCaja = new MovimientoCaja();

            nuevoMovCaja.TipoMovimiento = TipoMovimiento.Ingreso;
            nuevoMovCaja.Capital = capital;
            nuevoMovCaja.Interes = interes;
            nuevoMovCaja.CajaDetalleId = cajaDetalleId;
            nuevoMovCaja.Descripcion = descripcion; 
            nuevoMovCaja.EstaEliminado = false;
            nuevoMovCaja.Fecha = fecha;
            nuevoMovCaja.User = userLogin;
            nuevoMovCaja.TipoOperacion = operacion;

            return nuevoMovCaja;
        }

        private static CuentaCorrienteCliente AsignarCuentaCorriente(ComprobanteDTO comprobante, string userLogin, MedioPagoCtaCte mpCtaCte)
        {
            var _nuevaCtaCteCliente = new CuentaCorrienteCliente();

            _nuevaCtaCteCliente.ClienteId = mpCtaCte.ClienteId;
            _nuevaCtaCteCliente.TipoMovimiento = Aplicacion.Constantes.TipoMovimiento.Egreso;
            _nuevaCtaCteCliente.Fecha = comprobante.Fecha;
            _nuevaCtaCteCliente.NroComprobanteFactura = string.Empty;
            _nuevaCtaCteCliente.Monto = mpCtaCte.Capital + mpCtaCte.Interes;
            _nuevaCtaCteCliente.EstaEliminado = false;
            _nuevaCtaCteCliente.User = userLogin;

            return _nuevaCtaCteCliente;
        }

        private static MedioPagoCtaCte AsignarMedioPagoCtaCte(ComprobanteDTO comprobante, MedioPagoDTO? mp)
        {
            var mpCtaCte = new MedioPagoCtaCte();

            var mpCtaCteDto = (MedioPagoCtaCteDTO)mp;

            mpCtaCte.EmpresaId = comprobante.EmpresaId;
            mpCtaCte.Interes = mpCtaCteDto.Interes;
            mpCtaCte.Capital = mpCtaCteDto.Capital;
            mpCtaCte.Tipo = mpCtaCteDto.Tipo;
            mpCtaCte.ClienteId = mpCtaCteDto.Cliente.Id;

            return mpCtaCte;
        }

        private static MedioPagoTarjeta AsignarMedioPagoTarjeta(ComprobanteDTO comprobante, MedioPagoDTO? mp)
        {
            var mpTarjeta = new MedioPagoTarjeta();

            var mpTarjetaDto = (MedioPagoTarjetaDTO)mp;

            mpTarjeta.EmpresaId = comprobante.EmpresaId;
            mpTarjeta.Interes = mpTarjetaDto.Interes;
            mpTarjeta.Capital = mpTarjetaDto.Capital;
            mpTarjeta.Tipo = mpTarjetaDto.Tipo;
            mpTarjeta.NumeroCupon = mpTarjetaDto.NumeroCupon;
            mpTarjeta.PlanTarjetaId = mpTarjetaDto.PlanTarjetaId;

            return mpTarjeta;
        }

        private static MedioPagoTransferencia AsignarMedioPagoTransferencia(ComprobanteDTO comprobante, MedioPagoDTO? mp)
        {
            var mpTransferencia = new MedioPagoTransferencia();

            var mpTransferenciaDto = (MedioPagoTransferenciaDTO)mp;

            mpTransferencia.EmpresaId = comprobante.EmpresaId;
            mpTransferencia.Interes = mpTransferenciaDto.Interes;
            mpTransferencia.Capital = mpTransferenciaDto.Capital;
            mpTransferencia.Tipo = mpTransferenciaDto.Tipo;
            mpTransferencia.NumeroTransferencia = mpTransferenciaDto.NumeroTransferencia;
            mpTransferencia.BancoId = mpTransferenciaDto.BancoId;
            mpTransferencia.NombreTitular = mpTransferenciaDto.NombreTitular;

            return mpTransferencia;
        }

        private static MedioPagoCheque AsignarMedioPagoCheque(ComprobanteDTO comprobante, MedioPagoDTO? mp)
        {
            var mpCheque = new MedioPagoCheque();

            var mpChequeDto = (MedioPagoChequeDTO)mp;

            mpCheque.EmpresaId = comprobante.EmpresaId;
            mpCheque.Interes = mpChequeDto.Interes;
            mpCheque.Capital = mpChequeDto.Capital;
            mpCheque.Tipo = mpChequeDto.Tipo;
            mpCheque.NumeroCheque = mpChequeDto.NumeroCheque;
            mpCheque.BancoId = mpChequeDto.BancoId;
            mpCheque.FechaVencimiento = mpChequeDto.FechaVencimiento;

            return mpCheque;
        }

        private static MedioPagoEfectivo AsignarMedioPagoEfectivo(ComprobanteDTO comprobante, MedioPagoDTO? mp)
        {
            var mpEfectivo = new MedioPagoEfectivo();

            var mpEfectivoDto = (MedioPagoEfectivoDTO)mp;

            mpEfectivo.EmpresaId = comprobante.EmpresaId;
            mpEfectivo.Interes = mpEfectivoDto.Interes;
            mpEfectivo.Capital = mpEfectivoDto.Capital;
            mpEfectivo.Tipo = mpEfectivoDto.Tipo;

            return mpEfectivo;
        }

        private ResultDTO AsignarDatosOrdenFabricacion(ComprobanteDetalleDTO detalle, Guid empresaId, Guid articuloTemporalId, string userLogin)
        {
            var configCoreResult = ConfiguracionCoreServicio.Get(empresaId);

            var _configuracion = (ConfiguracionCoreDTO)configCoreResult.Data;

            var nuevaOrdenFabricacion = new OrdenFabricacionPersistenciaDTO();

            nuevaOrdenFabricacion.OrigenFabricacion = Aplicacion.Constantes.OrigenFabricacion.Venta;
            nuevaOrdenFabricacion.ArticuloId = articuloTemporalId;
            nuevaOrdenFabricacion.EmpresaId = empresaId;
            nuevaOrdenFabricacion.NumeroOrden = int.Parse(detalle.CodigoFabricacion);
            nuevaOrdenFabricacion.FechaFinalizacion = null;
            nuevaOrdenFabricacion.Cantidad = detalle.Cantidad;
            nuevaOrdenFabricacion.DepositoOrigenId = _configuracion.DepositoPorDefectoParaVentaId;
            nuevaOrdenFabricacion.DepositoDestinoId = _configuracion.DepositoPorDefectoParaVentaId;
            nuevaOrdenFabricacion.EstaEliminado = false;               
            nuevaOrdenFabricacion.ActulizarPrecioPublico = false;
            nuevaOrdenFabricacion.Foto = detalle.Foto;

            nuevaOrdenFabricacion.Detalles = new List<OrdenFabricacionDetalleDTO>();

            foreach (var detalleFabricacion in detalle.FabricacionDetalles.ToList())
            {
                var nuevoDetalleFabricacion = new OrdenFabricacionDetalleDTO();

                nuevoDetalleFabricacion.ArticuloId = detalleFabricacion.ArticuloId.Value;
                nuevoDetalleFabricacion.Cantidad = detalleFabricacion.Cantidad;
                nuevoDetalleFabricacion.Codigo = detalleFabricacion.Codigo;
                nuevoDetalleFabricacion.Descripcion = detalleFabricacion.Descripcion;
                nuevoDetalleFabricacion.EstaEliminado = false;
                
                nuevaOrdenFabricacion.Detalles.Add(nuevoDetalleFabricacion);
            }

            var result = OrdenFabricacionServicio.Add(nuevaOrdenFabricacion, userLogin);

            return new ResultDTO { State = true, Data = result };
        }

        private Guid AsignarDatosArticuloTemporal(ComprobanteDetalleDTO detalle, Guid empresaId, string userLogin)
        {
            var nuevoArticuloTemporal = new ArticuloTemporalPersistenciaDTO();

            nuevoArticuloTemporal.EmpresaId = empresaId;
            nuevoArticuloTemporal.Codigo = detalle.Codigo;
            nuevoArticuloTemporal.Descripcion = detalle.Descripcion;
            nuevoArticuloTemporal.Foto = detalle.Foto;
            nuevoArticuloTemporal.PrecioPublico = detalle.PrecioPublico;
            nuevoArticuloTemporal.EstaEliminado = true;
            nuevoArticuloTemporal.Foto = detalle.Foto;

            var result = ArticuloTemporalServicio.Add(nuevoArticuloTemporal, userLogin);

            if (result == null || !result.State)
            {
                throw new Exception("Ocurrió un error al crear el archivo temporal");
            }

            return ((ArticuloTemporalPersistenciaDTO)result.Data).Id;
        }
    }
}

