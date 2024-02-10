using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.Repositorio;
using Sidkenu.Dominio.Repositorio.Core;

namespace Sidkenu.Dominio.UnidadDeTrabajo
{
    public interface IUnidadDeTrabajo
    {
        void Dispose();
        void Commit();

        void Migrate();

        void BeginTransaction();

        void CommitTransaction();

        void Rollback();

        // ------------------------------------------------------------------------------ //
        // ---------------                  Seguridad                       ------------- //
        // ------------------------------------------------------------------------------ //

        #region Repositorio de Seguridad

        public IRepositoryGeneric<Empresa> EmpresaRepository { get; }
        public IRepositoryGeneric<Provincia> ProvinciaRepository { get; }
        public IRepositoryGeneric<Localidad> LocalidadRepository { get; }
        public IRepositoryGeneric<TipoResponsabilidad> TipoResponsabilidadRepository { get; }
        public IRepositoryGeneric<IngresoBruto> IngresoBrutoRepository { get; }
        public IRepositoryGeneric<Persona> PersonaRepository { get; }
        public IRepositoryGeneric<Usuario> UsuarioRepository { get; }
        public IRepositoryGeneric<EmpresaPersona> EmpresaPersonaRepository { get; }
        public IRepositoryGeneric<ConfiguracionSeguridad> ConfiguracionSeguridadRepository { get; }
        public IRepositoryGeneric<Grupo> GrupoRepository { get; }
        public IRepositoryGeneric<GrupoFormulario> GrupoFormularioRepository { get; }
        public IRepositoryGeneric<GrupoPersona> GrupoPersonaRepository { get; }
        public IRepositoryGeneric<Formulario> FormularioRepository { get; }
        public IRepositoryGeneric<Modulo> ModuloRepository { get; }

        #endregion

        // ------------------------------------------------------------------------------ //
        // ---------------                     Core                         ------------- //
        // ------------------------------------------------------------------------------ //

        #region Repositorio de Core

        public IArticuloRepository ArticuloRepository { get; }
        public IArticuloTemporalRepository ArticuloTemporalRepository { get; }
        public IRepositoryGeneric<ArticuloBaja> ArticuloBajaRepository { get; }
        public IRepositoryGeneric<ArticuloOpcional> ArticuloOpcionalRepository { get; }
        public IRepositoryGeneric<ArticuloKit> ArticuloKitRepository { get; }
        public IRepositoryGeneric<ArticuloProveedor> ArticuloProveedorRepository { get; }
        public IRepositoryGeneric<ArticuloDeposito> ArticuloDepositoRepository { get; }
        public IArticuloDepositoBulkRepository ArticuloDepositoBulkRepository { get; }
        public IRepositoryGeneric<ArticuloFormula> ArticuloFormulaRepository { get; }
        public IRepositoryGeneric<ArticuloHistorialCosto> ArticuloHistorialCostoRepository { get; }
        public IRepositoryGeneric<ArticuloPrecio> ArticuloPrecioRepository { get; }
        public IRepositoryGeneric<Caja> CajaRepository { get; }
        public IRepositoryGeneric<CajaDetalle> CajaDetalleRepository { get; }
        public IRepositoryGeneric<Cliente> ClienteRepository { get; }
        public IRepositoryGeneric<CondicionIva> CondicionIvaRepository { get; }
        public IRepositoryGeneric<Contador> ContadorRepository { get; }
        public IRepositoryGeneric<CuentaCorrienteCliente> CuentaCorrienteClienteRepository { get; }
        public IRepositoryGeneric<CuentaCorrienteProveedor> CuentaCorrienteProveedorRepository { get; }
        public IRepositoryGeneric<Variante> VarianteRepository { get; }
        public IRepositoryGeneric<VarianteValor> VarianteValorRepository { get; }
        public IRepositoryGeneric<Deposito> DepositoRepository { get; }
        public IRepositoryGeneric<Familia> FamiliaRepository { get; }
        public IRepositoryGeneric<FamiliaListaPrecio> FamiliaListaPrecioRepository { get; }
        public IRepositoryGeneric<FamiliaCaja> FamiliaCajaRepository { get; }
        public IRepositoryGeneric<TipoGasto> TipoGastoRepository { get; }
        public IRepositoryGeneric<Gasto> GastoRepository { get; }
        public IRepositoryGeneric<ListaPrecio> ListaPrecioRepository { get; }
        public IRepositoryGeneric<Marca> MarcaRepository { get; }
        public IRepositoryGeneric<MarcaListaPrecio> MarcaListaPrecioRepository { get; }
        public IRepositoryGeneric<MotivoBaja> MotivoBajaRepository { get; }
        public IRepositoryGeneric<MovimientoCaja> MovimientoCajaRepository { get; }
        public IRepositoryGeneric<Tarjeta> TarjetaRepository { get; }
        public IRepositoryGeneric<PlanTarjeta> PlanTarjetaRepository { get; }
        public IRepositoryGeneric<Proveedor> ProveedorRepository { get; }
        public IRepositoryGeneric<Salon> SalonRepository { get; }
        public IRepositoryGeneric<TipoDocumento> TipoDocumentoRepository { get; }
        public IRepositoryGeneric<UnidadMedida> UnidadMedidaRepository { get; }
        public IRepositoryGeneric<Mesa> MesaRepository { get; }
        public IRepositoryGeneric<ConfiguracionCore> ConfiguracionCoreRepository { get; }
        public IRepositoryGeneric<OrdenFabricacion> OrdenFabricacionRepository { get; }
        public IRepositoryGeneric<CostoFabricacion> CostoFabricacionRepository { get; }
        public IRepositoryGeneric<ConfiguracionBalanza> ConfiguracionBalanzaRepository { get; }
        public IRepositoryGeneric<Banco> BancoRepository { get; }
        public IComprobanteVentaRepository ComprobanteVentaRepository { get; }
        public IComprobanteGastoRepository ComprobanteGastoRepository { get; }
        public IComprobanteTransferenciaRepository ComprobanteTransferenciaRepository { get; }
        public IRepositoryGeneric<MedioPago> MedioPagoRepository { get; }
        public IRepositoryGeneric<PuestoTrabajo> PuestoTrabajoRepository { get; }
        public IRepositoryGeneric<CajaPuestoTrabajo> CajaPuestoTrabajoRepository { get; }

        public IRepositoryGeneric<Pedido> PedidoRepository { get; }

        #endregion
    }
}
