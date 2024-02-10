﻿using FontAwesome.Sharp;
using Serilog;
using Sidkenu.Servicio.DTOs.Seguridad.IngresoBruto;
using Sidkenu.Servicio.Interface.Seguridad;
using SidkenuWF.Formularios.Base;
using SidkenuWF.Formularios.Base.Constantes;

namespace SidkenuWF.Formularios.Seguridad
{
    public partial class _00013_IngresoBruto : FormularioConsulta
    {
        private readonly IIngresoBrutoServicio _ingresoBrutoServicio;
        public _00013_IngresoBruto(ISeguridadServicio seguridadServicio,
                                   IConfiguracionServicio configuracionServicio,
                                   ILogger logger,
                                   IIngresoBrutoServicio ingresoBrutoServicio)
                                   : base(seguridadServicio, configuracionServicio, logger)
        {
            InitializeComponent();

            base.BotoneraLateralVisible = (false, false); // (Panel Botonera, Panel Abrir-Cerrar)
            base.TituloFormulario = Base.Constantes.FormularioConstantes.Titulo;
            base.Titulo = "Ingresos Brutos";
            base.Logo = IconChar.BuildingFlag;

            _ingresoBrutoServicio = ingresoBrutoServicio;
        }

        public override bool EjecutarComandoNuevo(object sender, EventArgs e)
        {
            var formularioAbm = new _00014_IngresoBruto_Abm(base._seguridadServicio,
                                                            base._configuracionServicio,
                                                            base._logger,
                                                            Program.Container.GetInstance<IIngresoBrutoServicio>(),
                                                            TipoOperacion.Nuevo);

            FormularioSeguridad.VerificarAcceso(formularioAbm, base._seguridadServicio, base._logger, base._configuracionDTO);

            return formularioAbm.RealizoAlgunaOperacion;
        }

        public override bool EjecutarComandoEditar(object sender, EventArgs e)
        {
            var formularioAbm = new _00014_IngresoBruto_Abm(base._seguridadServicio,
                                                            base._configuracionServicio,
                                                            base._logger,
                                                            Program.Container.GetInstance<IIngresoBrutoServicio>(),
                                                            TipoOperacion.Modificar, EntidadId);

            FormularioSeguridad.VerificarAcceso(formularioAbm, base._seguridadServicio, base._logger, base._configuracionDTO);

            return formularioAbm.RealizoAlgunaOperacion;
        }

        public override bool EjecutarComandoEliminar(object sender, EventArgs e)
        {
            try
            {
                _ingresoBrutoServicio.Delete(new IngresoBrutoDeleteDTO { Id = base.EntidadId.Value }, Properties.Settings.Default.UserLogin);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public override void Buscar(string cadenaBuscar, bool verEliminados = false)
        {
            var result = _ingresoBrutoServicio.GetByFilter(new IngresoBrutoFilterDTO
            {
                CadenaBuscar = cadenaBuscar,
                VerEliminados = verEliminados
            });

            if (result.State)
            {
                this.dgvGrilla.DataSource = result.Data;

                base.Buscar(cadenaBuscar, verEliminados);
            }
            else
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error($"{base.Titulo}: error al obtener los datos. User: {Properties.Settings.Default.PersonaLogin}");
                }

                MessageBox.Show("Ocurrió un error al obtener los datos");
            }
        }

        public override void FormatearDatos(DataGridView dgvGrilla)
        {
            try
            {
                base.FormatearDatos(dgvGrilla);

                dgvGrilla.Columns["Descripcion"].Visible = true;
                dgvGrilla.Columns["Descripcion"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvGrilla.Columns["Descripcion"].HeaderText = "Ingreso Bruto";
                dgvGrilla.Columns["Descripcion"].DisplayIndex = 0;
                dgvGrilla.Columns["Descripcion"].ReadOnly = true;
            }
            catch (Exception ex)
            {
                if (base._configuracionDTO != null && base._configuracionDTO.LogError)
                {
                    _logger.Error(ex, $"Error en el formateo de las columnas en {base.Titulo}.");
                }

                MessageBox.Show("Los nombres de las columnas no coinciden");
            }
        }
    }
}
