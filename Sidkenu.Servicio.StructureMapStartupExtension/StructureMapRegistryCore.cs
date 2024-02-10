using FluentValidation;
using Sidkenu.Servicio.DTOs.Core.Articulo;
using Sidkenu.Servicio.DTOs.Core.Caja;
using Sidkenu.Servicio.DTOs.Core.Cliente;
using Sidkenu.Servicio.DTOs.Core.CondicionIva;
using Sidkenu.Servicio.DTOs.Core.Variante;
using Sidkenu.Servicio.DTOs.Core.Familia;
using Sidkenu.Servicio.DTOs.Core.Gasto;
using Sidkenu.Servicio.DTOs.Core.ListaPrecio;
using Sidkenu.Servicio.DTOs.Core.Marca;
using Sidkenu.Servicio.DTOs.Core.Mesa;
using Sidkenu.Servicio.DTOs.Core.MotivoBaja;
using Sidkenu.Servicio.DTOs.Core.Proveedor;
using Sidkenu.Servicio.DTOs.Core.TipoGasto;
using Sidkenu.Servicio.DTOs.Core.UnidadMedida;
using Sidkenu.Servicio.Implementacion.Core;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Validator.Core;
using StructureMap;
using Sidkenu.Servicio.DTOs.Core.Deposito;
using Sidkenu.Servicio.DTOs.Core.ConfiguracionCore;
using Sidkenu.Servicio.DTOs.Core.Tarjeta;
using Sidkenu.Servicio.DTOs.Core.PlanTarjeta;
using Sidkenu.Servicio.DTOs.Core.Banco;
using Sidkenu.Servicio.Implementacion.Core.Comprobante;

namespace Sidkenu.Servicio.StructureMapStartupExtension
{
    public class StructureMapRegistryCore : Registry
    {
        public StructureMapRegistryCore()
        {
            // Servicios Core
            For<ICajaServicio>().Use<CajaServicio>();
            For<ITipoGastoServicio>().Use<TipoGastoServicio>();
            For<IGastosServicio>().Use<GastoServicio>();
            For<IClienteServicio>().Use<ClienteServicio>();
            For<IProveedorServicio>().Use<ProveedorServicio>();
            For<IListaPrecioServicio>().Use<ListaPrecioServicio>();
            For<ITipoDocumentoServicio>().Use<TipoDocumentoServicio>();
            For<IFamiliaServicio>().Use<FamiliaServicio>();
            For<IMarcaServicio>().Use<MarcaServicio>();
            For<IUnidadMedidaServicio>().Use<UnidadMedidaServicio>();
            For<IMesaServicio>().Use<MesaServicio>();
            For<IMotivoBajaServicio>().Use<MotivoBajaServicio>();
            For<IArticuloServicio>().Use<ArticuloServicio>();
            For<IArticuloProveedorServicio>().Use<ArticuloProveedorServicio>();
            For<ICondicionIvaServicio>().Use<CondicionIvaServicio>();
            For<IVarianteServicio>().Use<VarianteServicio>();
            For<IVarianteValorServicio>().Use<VarianteValorServicio>();
            For<IArticuloVarianteServicio>().Use<ArticuloVarianteServicio>();
            For<IArticuloKitServicio>().Use<ArticuloKitServicio>();
            For<IArticuloFormulaServicio>().Use<ArticuloFormulaServicio>();
            For<IDepositoServicio>().Use<DepositoServicio>();
            For<IConfiguracionCoreServicio>().Use<ConfiguracionCoreServicio>();
            For<IConfiguracionBalanzaServicio>().Use<ConfiguracionBalanzaServicio>();
            For<IOrdenFabricacionServicio>().Use<OrdenFabricacionServicio>();
            For<IContadorServicio>().Use<ContadorServicio>();
            For<ICostoFabricacionServicio>().Use<CostoFabricacionServicio>();
            For<IArticuloPrecioServicio>().Use<ArticuloPrecioServicio>();
            For<ITarjetaServicio>().Use<TarjetaServicio>(); 
            For<IPlanTarjetaServicio>().Use<PlanTarjetaServicio>();
            For<ICuentaCorrienteClienteServicio>().Use<CuentaCorrienteClienteServicio>();
            For<IBancoServicio>().Use<BancoServicio>();
            For<IComprobanteServicio>().Use<ComprobanteServicio>();
            For<IArticuloTemporalServicio>().Use<ArticuloTemporalServicio>();
            For<ICajaPuestoTrabajoServicio>().Use<CajaPuestoTrabajoServicio>();
            For<IMovimientoCajaServicio>().Use<MovimientoCajaServicio>();
            For<IPedidoServicio>().Use<PedidoServicio>();

            // Validaciones Core
            For<IValidator<ArticuloPersistenciaDTO>>().Use<ArticuloValidator>();
            For<IValidator<CajaPersistenciaDTO>>().Use<CajaValidator>();
            For<IValidator<TipoGastoPersistenciaDTO>>().Use<TipoGastoValidator>();
            For<IValidator<GastosPersistenciaDTO>>().Use<GastoValidator>();
            For<IValidator<ClientePersistenciaDTO>>().Use<ClienteValidator>();
            For<IValidator<ProveedorPersistenciaDTO>>().Use<ProveedorValidator>();
            For<IValidator<ListaPrecioPersistenciaDTO>>().Use<ListaPrecioValidator>();
            For<IValidator<FamiliaPersistenciaDTO>>().Use<FamiliaValidator>();
            For<IValidator<UnidadMedidaPersistenciaDTO>>().Use<UnidadMedidaValidator>();
            For<IValidator<MarcaPersistenciaDTO>>().Use<MarcaValidator>();
            For<IValidator<MesaPersistenciaDTO>>().Use<MesaValidator>();
            For<IValidator<MotivoBajaPersistenciaDTO>>().Use<MotivoBajaValidator>();
            For<IValidator<CondicionIvaPersistenciaDTO>>().Use<CondicionIvaValidator>();
            For<IValidator<VariantePersistenciaDTO>>().Use<VarianteValidator>();
            For<IValidator<DepositoPersistenciaDTO>>().Use<DepositoValidator>();
            For<IValidator<ConfiguracionCorePersistenciaDTO>>().Use<ConfiguracionCoreValidator>();
            For<IValidator<TarjetaPersistenciaDTO>>().Use<TarjetaValidator>();
            For<IValidator<PlanTarjetaPersistenciaDTO>>().Use<PlanTarjetaValidator>();
            For<IValidator<BancoPersistenciaDTO>>().Use<BancoValidator>();
        }
    }
}



