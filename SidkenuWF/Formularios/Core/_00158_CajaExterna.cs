using Serilog;
using Sidkenu.Aplicacion.CadenaConexion;
using Sidkenu.Dominio.Entidades.Core;
using Sidkenu.Servicio.DTOs.Core.Caja;
using Sidkenu.Servicio.DTOs.Core.Comprobante;
using Sidkenu.Servicio.Implementacion.Core.Comprobante;
using Sidkenu.Servicio.Interface.Core;
using Sidkenu.Servicio.Interface.Seguridad;
using SidkenuWF.Formularios.Base;
using SidkenuWF.Formularios.Base.Constantes;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.EventArgs;

namespace SidkenuWF.Formularios.Core
{
    public partial class _00158_CajaExterna : FormularioComun
    {
        private readonly IComprobanteServicio _comprobanteServicio;
        private readonly IConexionServicio _conexionServicio;
        private CajaDTO _caja;

        public bool RealizoAlgunaOperacion { get; set; }

        private SqlTableDependency<Sidkenu.Dominio.Entidades.Core.Comprobante> _comprobanteDependency;
        private List<ComprobanteVentaDTO> _comprobantes;

        public _00158_CajaExterna(ISeguridadServicio seguridadServicio,
                                  IConfiguracionServicio configuracionServicio,
                                  ILogger logger,
                                  IComprobanteServicio comprobanteServicio,
                                  IConexionServicio conexionServicio,
                                  CajaDTO caja)
                                  : base(seguridadServicio, configuracionServicio, logger)
        {
            InitializeComponent();

            base.Titulo = caja.Descripcion;
            base.TituloFormulario = FormularioConstantes.Titulo;
            base.Logo = FontAwesome.Sharp.IconChar.CashRegister;

            _comprobanteServicio = comprobanteServicio;
            _conexionServicio = conexionServicio;
            _caja = caja;

            _comprobantes ??= new List<ComprobanteVentaDTO>();
        }

        private void Start_ordenFabricacion_table_dependency()
        {
            _comprobanteDependency = new SqlTableDependency<Sidkenu.Dominio.Entidades.Core.Comprobante>(_conexionServicio.ObtenerCadenaConexion(MotoBaseDatos.Obtener), "Comprobantes");
            _comprobanteDependency.OnChanged += ComprobanteDependency_OnChanged;
            _comprobanteDependency.Start();
        }

        private void Stop_ordenFabricacion_table_dependency()
        {
            if (_comprobanteDependency != null)
            {
                _comprobanteDependency.OnChanged -= ComprobanteDependency_OnChanged;
                _comprobanteDependency.Stop();
            }
        }

        private void ComprobanteDependency_OnChanged(object sender, RecordChangedEventArgs<Sidkenu.Dominio.Entidades.Core.Comprobante> e)
        {
            if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Insert)
            {
                RefreshComprobantes();
            }

            if (e.ChangeType == TableDependency.SqlClient.Base.Enums.ChangeType.Update)
            {
                RefreshComprobantes();
            }
        }

        private void RefreshComprobantes()
        {
            var result = _comprobanteServicio.GetComprobantesPendientesCobroVentaMostrador(_caja.Id,
                                                                                           Properties.Settings.Default.EmpresaId,
                                                                                           txtBuscar.Text);

            ThreadSafe(() =>
            {
                if (result != null && result.Data != null)
                {
                    var listaComprobantes = (List<ComprobanteDTO>)result.Data;

                    dgvGrillaComprobante.DataSource = listaComprobantes;

                    FormatearGrillaComprobante(dgvGrillaComprobante);
                }
            });
        }

        private void FormatearGrillaComprobante(DataGridView dgvGrillaComprobante)
        {

        }

        private void ThreadSafe(MethodInvoker method)
        {
            try
            {
                if (InvokeRequired)
                    Invoke(method);
                else
                    method();
            }
            catch (ObjectDisposedException) { }
        }

        private void _00158_CajaExterna_Shown(object sender, EventArgs e)
        {
            Start_ordenFabricacion_table_dependency();
        }

        private void BtnFacturar_Click(object sender, EventArgs e)
        {
            if (_comprobantes.Count == 0)
            {
                MessageBox.Show("Por favor seleccione una Factura para cobrar", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var fMediosDePagos = new _00148_FormaPago(base._seguridadServicio,
                                                      base._configuracionServicio,
                                                      base._logger,
                                                      Program.Container.GetInstance<ITarjetaServicio>(),
                                                      Program.Container.GetInstance<IPlanTarjetaServicio>(),
                                                      Program.Container.GetInstance<IConfiguracionCoreServicio>(),
                                                      Program.Container.GetInstance<IComprobanteServicio>(),
                                                      Program.Container.GetInstance<ICajaServicio>(),
                                                      _comprobantes);

            fMediosDePagos.ShowDialog();
        }
    }
}
