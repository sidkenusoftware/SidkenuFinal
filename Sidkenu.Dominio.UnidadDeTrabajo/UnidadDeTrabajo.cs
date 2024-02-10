using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Dominio.Entidades.Seguridad;
using Sidkenu.Dominio.Repositorio;
using Sidkenu.Dominio.Repositorio.Core;
using Sidkenu.Infraestructura;

namespace Sidkenu.Dominio.UnidadDeTrabajo
{
    public class UnidadDeTrabajo : IUnidadDeTrabajo
    {
        private readonly DataContext _context;
        private IDbContextTransaction _transaction;

        public UnidadDeTrabajo(DataContext context)
        {
            _context = context;
        }


        public void Dispose()
        {
            _context?.Dispose();
            _transaction?.Dispose();
        }

        public void Commit()
        {
            _context.SaveChanges();
            _transaction?.Dispose();
        }

        public void Migrate()
        {
            _context.Database.Migrate();
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }

        public void CommitTransaction()
        {
            _transaction?.Commit();
        }

        // ------------------------------------------------------------------------------ //
        // ---------------                  Seguridad                       ------------- //
        // ------------------------------------------------------------------------------ //

        #region Repositorio de Seguridad

        private IRepositoryGeneric<Empresa> _empresaRepository;
        private IRepositoryGeneric<Provincia> _provinciaRepository;
        private IRepositoryGeneric<Localidad> _localidadRepository;
        private IRepositoryGeneric<TipoResponsabilidad> _tipoResponsabilidadRepository;
        private IRepositoryGeneric<IngresoBruto> _ingresoBrutoRepository;
        private IRepositoryGeneric<Usuario> _usuarioRepository;
        private IRepositoryGeneric<Persona> _personaRepository;
        private IRepositoryGeneric<EmpresaPersona> _empresaPersonaRepository;
        private IRepositoryGeneric<ConfiguracionSeguridad> _configuracionSeguridadRepository;
        private IRepositoryGeneric<Grupo> _grupoRepository;
        private IRepositoryGeneric<GrupoFormulario> _grupoFormularioRepository;
        private IRepositoryGeneric<GrupoPersona> _grupoPersonaRepository;
        private IRepositoryGeneric<Formulario> _formularioRepository;
        private IRepositoryGeneric<Modulo> _moduloRepository;

        public IRepositoryGeneric<Empresa> EmpresaRepository => _empresaRepository ??= new RepositoryGeneric<Empresa>(_context);
        public IRepositoryGeneric<Provincia> ProvinciaRepository => _provinciaRepository ??= new RepositoryGeneric<Provincia>(_context);
        public IRepositoryGeneric<Localidad> LocalidadRepository => _localidadRepository ??= new RepositoryGeneric<Localidad>(_context);
        public IRepositoryGeneric<TipoResponsabilidad> TipoResponsabilidadRepository => _tipoResponsabilidadRepository ??= new RepositoryGeneric<TipoResponsabilidad>(_context);
        public IRepositoryGeneric<IngresoBruto> IngresoBrutoRepository => _ingresoBrutoRepository ??= new RepositoryGeneric<IngresoBruto>(_context);
        public IRepositoryGeneric<Persona> PersonaRepository => _personaRepository ??= new RepositoryGeneric<Persona>(_context);
        public IRepositoryGeneric<Usuario> UsuarioRepository => _usuarioRepository ??= new RepositoryGeneric<Usuario>(_context);
        public IRepositoryGeneric<EmpresaPersona> EmpresaPersonaRepository => _empresaPersonaRepository ??= new RepositoryGeneric<EmpresaPersona>(_context);
        public IRepositoryGeneric<ConfiguracionSeguridad> ConfiguracionSeguridadRepository => _configuracionSeguridadRepository ??= new RepositoryGeneric<ConfiguracionSeguridad>(_context);
        public IRepositoryGeneric<Grupo> GrupoRepository => _grupoRepository ??= new RepositoryGeneric<Grupo>(_context);
        public IRepositoryGeneric<GrupoFormulario> GrupoFormularioRepository => _grupoFormularioRepository ??= new RepositoryGeneric<GrupoFormulario>(_context);
        public IRepositoryGeneric<GrupoPersona> GrupoPersonaRepository => _grupoPersonaRepository ??= new RepositoryGeneric<GrupoPersona>(_context);
        public IRepositoryGeneric<Formulario> FormularioRepository => _formularioRepository ??= new RepositoryGeneric<Formulario>(_context);
        public IRepositoryGeneric<Modulo> ModuloRepository => _moduloRepository ??= new RepositoryGeneric<Modulo>(_context);

        #endregion

        // ------------------------------------------------------------------------------ //
        // ---------------                     Core                         ------------- //
        // ------------------------------------------------------------------------------ //

        #region Repositorios de Core

        private IArticuloRepository _articuloRepository;
        private IArticuloTemporalRepository _articuloTemporalRepository;
        private IRepositoryGeneric<ArticuloDeposito> _articuloDepositoRepository;
        private IArticuloDepositoBulkRepository _articuloDepositoBulkRepository;
        private IRepositoryGeneric<ArticuloBaja> _articuloBajaRepository;
        private IRepositoryGeneric<ArticuloOpcional> _articuloOpcionalRepository;
        private IRepositoryGeneric<ArticuloProveedor> _articuloProveedorRepository;
        private IRepositoryGeneric<ArticuloFormula> _articuloFormulaRepository;
        private IRepositoryGeneric<ArticuloHistorialCosto> _articuloHistorialCostoRepository;
        private IRepositoryGeneric<ArticuloPrecio> _articuloPrecioRepository;
        private IRepositoryGeneric<ArticuloKit> _articuloKitRepository;
        private IRepositoryGeneric<Caja> _cajaRepository;
        private IRepositoryGeneric<CajaDetalle> _cajaDetalleRepository;
        private IRepositoryGeneric<Cliente> _clienteRepository;
        private IRepositoryGeneric<CondicionIva> _condicionIvaRepository;
        private IRepositoryGeneric<Contador> _contadorRepository;
        private IRepositoryGeneric<CuentaCorrienteCliente> _cuentaCorrienteClienteRepository;
        private IRepositoryGeneric<CuentaCorrienteProveedor> _cuentaCorrienteProveedorRepository;
        private IRepositoryGeneric<Variante> _escalaRepository;
        private IRepositoryGeneric<VarianteValor> _escalaValorRepository;
        private IRepositoryGeneric<Deposito> _depositoRepository;
        private IRepositoryGeneric<ArticuloDeposito> _depositoArticuloRepository;
        private IRepositoryGeneric<Familia> _familiaRepository;
        private IRepositoryGeneric<FamiliaCaja> _familiaCajaRepository;
        private IRepositoryGeneric<TipoGasto> _tipoGastoRepository;
        private IRepositoryGeneric<Gasto> _gastoRepository;
        private IRepositoryGeneric<ListaPrecio> _listaPrecioRepository;
        private IRepositoryGeneric<Marca> _marcaRepository;
        private IRepositoryGeneric<MotivoBaja> _motivoBajaRepository;
        private IRepositoryGeneric<MovimientoCaja> _movimientoCajaRepository;
        private IRepositoryGeneric<Tarjeta> _tarjetaRepository;
        private IRepositoryGeneric<PlanTarjeta> _planTarjetaRepository;
        private IRepositoryGeneric<Proveedor> _proveedorRepository;
        private IRepositoryGeneric<Salon> _salonRepository;
        private IRepositoryGeneric<TipoDocumento> _tipoDocumentoRepository;
        private IRepositoryGeneric<UnidadMedida> _unidadMedidaRepository;
        private IRepositoryGeneric<Mesa> _mesaRepository;
        private IRepositoryGeneric<ConfiguracionCore> _configuracionCoreRepository;
        private IRepositoryGeneric<FamiliaListaPrecio> _familiaListaPrecioRepository;
        private IRepositoryGeneric<MarcaListaPrecio> _marcaListaPrecioRepository;
        private IRepositoryGeneric<OrdenFabricacion> _ordenFabricacionRepository;
        private IRepositoryGeneric<CostoFabricacion> _costoFabricacionRepository;
        private IRepositoryGeneric<ConfiguracionBalanza> _configuracionBalanzaRepository;
        private IRepositoryGeneric<Banco> _bancoRepository;
        private IComprobanteVentaRepository _comprobanteVentaRepository;
        private IComprobanteGastoRepository _comprobanteGastoRepository;
        private IComprobanteTransferenciaRepository _comprobanteTransferenciaRepository;
        private IRepositoryGeneric<MedioPago> _medioPagoRepository;
        private IRepositoryGeneric<PuestoTrabajo> _puestoTrabajoRepository;
        private IRepositoryGeneric<CajaPuestoTrabajo> _cajaPuestoTrabajoRepository;
        private IRepositoryGeneric<Pedido> _pedidoRepository;

        // ====================================================================================================================================================================================== //

        public IArticuloRepository ArticuloRepository => _articuloRepository ??= new ArticuloRepository(_context);
        public IArticuloTemporalRepository ArticuloTemporalRepository => _articuloTemporalRepository ??= new ArticuloTemporalRepository(_context);
        public IRepositoryGeneric<ArticuloBaja> ArticuloBajaRepository => _articuloBajaRepository ??= new RepositoryGeneric<ArticuloBaja>(_context);
        public IRepositoryGeneric<ArticuloOpcional> ArticuloOpcionalRepository => _articuloOpcionalRepository ??= new RepositoryGeneric<ArticuloOpcional>(_context);
        public IRepositoryGeneric<ArticuloProveedor> ArticuloProveedorRepository => _articuloProveedorRepository ??= new RepositoryGeneric<ArticuloProveedor>(_context);
        public IRepositoryGeneric<ArticuloDeposito> ArticuloDepositoRepository => _articuloDepositoRepository ??= new RepositoryGeneric<ArticuloDeposito>(_context);
        public IArticuloDepositoBulkRepository ArticuloDepositoBulkRepository => _articuloDepositoBulkRepository ??= new ArticuloDepositoBulkRepository(_context);
        public IRepositoryGeneric<ArticuloFormula> ArticuloFormulaRepository => _articuloFormulaRepository ??= new RepositoryGeneric<ArticuloFormula>(_context);
        public IRepositoryGeneric<ArticuloHistorialCosto> ArticuloHistorialCostoRepository => _articuloHistorialCostoRepository ??= new RepositoryGeneric<ArticuloHistorialCosto>(_context);
        public IRepositoryGeneric<ArticuloPrecio> ArticuloPrecioRepository => _articuloPrecioRepository ??= new RepositoryGeneric<ArticuloPrecio>(_context);
        public IRepositoryGeneric<ArticuloKit> ArticuloKitRepository => _articuloKitRepository ??= new RepositoryGeneric<ArticuloKit>(_context);
        public IRepositoryGeneric<Caja> CajaRepository => _cajaRepository ??= new RepositoryGeneric<Caja>(_context);
        public IRepositoryGeneric<CajaDetalle> CajaDetalleRepository => _cajaDetalleRepository ??= new RepositoryGeneric<CajaDetalle>(_context);
        public IRepositoryGeneric<Cliente> ClienteRepository => _clienteRepository ??= new RepositoryGeneric<Cliente>(_context);
        public IRepositoryGeneric<CondicionIva> CondicionIvaRepository => _condicionIvaRepository ??= new RepositoryGeneric<CondicionIva>(_context);
        public IRepositoryGeneric<Contador> ContadorRepository => _contadorRepository ??= new RepositoryGeneric<Contador>(_context);
        public IRepositoryGeneric<CuentaCorrienteCliente> CuentaCorrienteClienteRepository => _cuentaCorrienteClienteRepository ??= new RepositoryGeneric<CuentaCorrienteCliente>(_context);
        public IRepositoryGeneric<CuentaCorrienteProveedor> CuentaCorrienteProveedorRepository => _cuentaCorrienteProveedorRepository ??= new RepositoryGeneric<CuentaCorrienteProveedor>(_context);
        public IRepositoryGeneric<Deposito> DepositoRepository => _depositoRepository ??= new RepositoryGeneric<Deposito>(_context);
        public IRepositoryGeneric<ArticuloDeposito> DepositoArticuloRepository => _depositoArticuloRepository ??= new RepositoryGeneric<ArticuloDeposito>(_context);
        public IRepositoryGeneric<Variante> VarianteRepository => _escalaRepository ??= new RepositoryGeneric<Variante>(_context);
        public IRepositoryGeneric<VarianteValor> VarianteValorRepository => _escalaValorRepository ??= new RepositoryGeneric<VarianteValor>(_context);
        public IRepositoryGeneric<Familia> FamiliaRepository => _familiaRepository ??= new RepositoryGeneric<Familia>(_context);
        public IRepositoryGeneric<FamiliaCaja> FamiliaCajaRepository => _familiaCajaRepository ??= new RepositoryGeneric<FamiliaCaja>(_context);
        public IRepositoryGeneric<TipoGasto> TipoGastoRepository => _tipoGastoRepository ??= new RepositoryGeneric<TipoGasto>(_context);
        public IRepositoryGeneric<Gasto> GastoRepository => _gastoRepository ??= new RepositoryGeneric<Gasto>(_context);
        public IRepositoryGeneric<ListaPrecio> ListaPrecioRepository => _listaPrecioRepository ??= new RepositoryGeneric<ListaPrecio>(_context);
        public IRepositoryGeneric<Marca> MarcaRepository => _marcaRepository ??= new RepositoryGeneric<Marca>(_context);
        public IRepositoryGeneric<MotivoBaja> MotivoBajaRepository => _motivoBajaRepository ??= new RepositoryGeneric<MotivoBaja>(_context);
        public IRepositoryGeneric<MovimientoCaja> MovimientoCajaRepository => _movimientoCajaRepository ??= new RepositoryGeneric<MovimientoCaja>(_context);
        public IRepositoryGeneric<Tarjeta> TarjetaRepository => _tarjetaRepository ??= new RepositoryGeneric<Tarjeta>(_context);
        public IRepositoryGeneric<PlanTarjeta> PlanTarjetaRepository => _planTarjetaRepository ??= new RepositoryGeneric<PlanTarjeta>(_context);
        public IRepositoryGeneric<Proveedor> ProveedorRepository => _proveedorRepository ??= new RepositoryGeneric<Proveedor>(_context);
        public IRepositoryGeneric<Salon> SalonRepository => _salonRepository ??= new RepositoryGeneric<Salon>(_context);
        public IRepositoryGeneric<TipoDocumento> TipoDocumentoRepository => _tipoDocumentoRepository ??= new RepositoryGeneric<TipoDocumento>(_context);
        public IRepositoryGeneric<UnidadMedida> UnidadMedidaRepository => _unidadMedidaRepository ??= new RepositoryGeneric<UnidadMedida>(_context);
        public IRepositoryGeneric<Mesa> MesaRepository => _mesaRepository ??= new RepositoryGeneric<Mesa>(_context);
        public IRepositoryGeneric<ConfiguracionCore> ConfiguracionCoreRepository => _configuracionCoreRepository ??= new RepositoryGeneric<ConfiguracionCore>(_context);
        public IRepositoryGeneric<FamiliaListaPrecio> FamiliaListaPrecioRepository => _familiaListaPrecioRepository ??= new RepositoryGeneric<FamiliaListaPrecio>(_context);
        public IRepositoryGeneric<MarcaListaPrecio> MarcaListaPrecioRepository => _marcaListaPrecioRepository ??= new RepositoryGeneric<MarcaListaPrecio>(_context);
        public IRepositoryGeneric<OrdenFabricacion> OrdenFabricacionRepository => _ordenFabricacionRepository ??= new RepositoryGeneric<OrdenFabricacion>(_context);
        public IRepositoryGeneric<CostoFabricacion> CostoFabricacionRepository => _costoFabricacionRepository ??= new RepositoryGeneric<CostoFabricacion>(_context);
        public IRepositoryGeneric<ConfiguracionBalanza> ConfiguracionBalanzaRepository => _configuracionBalanzaRepository ??= new RepositoryGeneric<ConfiguracionBalanza>(_context);
        public IRepositoryGeneric<Banco> BancoRepository => _bancoRepository ??= new RepositoryGeneric<Banco>(_context);
        public IComprobanteVentaRepository ComprobanteVentaRepository => _comprobanteVentaRepository ??= new ComprobanteVentaRepository(_context);
        public IComprobanteGastoRepository ComprobanteGastoRepository => _comprobanteGastoRepository ??= new ComprobanteGastoRepository(_context);
        public IComprobanteTransferenciaRepository ComprobanteTransferenciaRepository => _comprobanteTransferenciaRepository ??= new ComprobanteTransferenciaRepository(_context);
        public IRepositoryGeneric<MedioPago> MedioPagoRepository => _medioPagoRepository ??= new RepositoryGeneric<MedioPago>(_context);
        public IRepositoryGeneric<PuestoTrabajo> PuestoTrabajoRepository => _puestoTrabajoRepository ??= new RepositoryGeneric<PuestoTrabajo>(_context);
        public IRepositoryGeneric<CajaPuestoTrabajo> CajaPuestoTrabajoRepository => _cajaPuestoTrabajoRepository ??= new RepositoryGeneric<CajaPuestoTrabajo>(_context);
        public IRepositoryGeneric<Pedido> PedidoRepository => _pedidoRepository ??= new RepositoryGeneric<Pedido>(_context);

        #endregion
    }
}