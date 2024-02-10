namespace SidkenuWF.Formularios.Core
{
    partial class _00158_CajaExterna
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            pnlInferior = new Panel();
            btnFacturar = new FontAwesome.Sharp.IconButton();
            pnlComprobantes = new Panel();
            dgvGrillaComprobante = new DataGridView();
            pnlBotoneraComprobante = new Panel();
            btnDesmarcar = new FontAwesome.Sharp.IconButton();
            btnMarcar = new FontAwesome.Sharp.IconButton();
            pnlBuscar = new Panel();
            btnBuscar = new FontAwesome.Sharp.IconButton();
            txtBuscar = new TextBox();
            label2 = new Label();
            pnlContenedorDetalle = new Panel();
            panel4 = new Panel();
            panel3 = new Panel();
            label1 = new Label();
            txtTotal = new TextBox();
            pnlInferior.SuspendLayout();
            pnlComprobantes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvGrillaComprobante).BeginInit();
            pnlBotoneraComprobante.SuspendLayout();
            pnlBuscar.SuspendLayout();
            pnlContenedorDetalle.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // pnlInferior
            // 
            pnlInferior.BackColor = Color.WhiteSmoke;
            pnlInferior.Controls.Add(btnFacturar);
            pnlInferior.Dock = DockStyle.Bottom;
            pnlInferior.Location = new Point(0, 406);
            pnlInferior.Name = "pnlInferior";
            pnlInferior.Size = new Size(800, 44);
            pnlInferior.TabIndex = 1;
            // 
            // btnFacturar
            // 
            btnFacturar.BackColor = Color.FromArgb(54, 74, 90);
            btnFacturar.FlatAppearance.BorderColor = Color.WhiteSmoke;
            btnFacturar.FlatAppearance.BorderSize = 0;
            btnFacturar.FlatStyle = FlatStyle.Flat;
            btnFacturar.ForeColor = Color.WhiteSmoke;
            btnFacturar.IconChar = FontAwesome.Sharp.IconChar.MoneyBills;
            btnFacturar.IconColor = Color.WhiteSmoke;
            btnFacturar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnFacturar.IconSize = 22;
            btnFacturar.Location = new Point(7, 7);
            btnFacturar.Name = "btnFacturar";
            btnFacturar.Size = new Size(140, 30);
            btnFacturar.TabIndex = 7;
            btnFacturar.Text = "(F5) Facturar";
            btnFacturar.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnFacturar.UseVisualStyleBackColor = false;
            btnFacturar.Click += BtnFacturar_Click;
            // 
            // pnlComprobantes
            // 
            pnlComprobantes.Controls.Add(dgvGrillaComprobante);
            pnlComprobantes.Controls.Add(pnlBotoneraComprobante);
            pnlComprobantes.Controls.Add(pnlBuscar);
            pnlComprobantes.Dock = DockStyle.Left;
            pnlComprobantes.Location = new Point(0, 59);
            pnlComprobantes.Name = "pnlComprobantes";
            pnlComprobantes.Size = new Size(349, 347);
            pnlComprobantes.TabIndex = 2;
            // 
            // dgvGrillaComprobante
            // 
            dgvGrillaComprobante.AllowUserToAddRows = false;
            dgvGrillaComprobante.AllowUserToDeleteRows = false;
            dgvGrillaComprobante.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.Red;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvGrillaComprobante.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvGrillaComprobante.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvGrillaComprobante.Dock = DockStyle.Fill;
            dgvGrillaComprobante.GridColor = Color.SteelBlue;
            dgvGrillaComprobante.Location = new Point(0, 40);
            dgvGrillaComprobante.Name = "dgvGrillaComprobante";
            dgvGrillaComprobante.RowHeadersVisible = false;
            dataGridViewCellStyle2.BackColor = Color.WhiteSmoke;
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(64, 64, 64);
            dataGridViewCellStyle2.SelectionBackColor = Color.SteelBlue;
            dataGridViewCellStyle2.SelectionForeColor = Color.WhiteSmoke;
            dgvGrillaComprobante.RowsDefaultCellStyle = dataGridViewCellStyle2;
            dgvGrillaComprobante.RowTemplate.Height = 25;
            dgvGrillaComprobante.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvGrillaComprobante.Size = new Size(349, 262);
            dgvGrillaComprobante.TabIndex = 5;
            // 
            // pnlBotoneraComprobante
            // 
            pnlBotoneraComprobante.BackColor = Color.Gainsboro;
            pnlBotoneraComprobante.Controls.Add(btnDesmarcar);
            pnlBotoneraComprobante.Controls.Add(btnMarcar);
            pnlBotoneraComprobante.Dock = DockStyle.Bottom;
            pnlBotoneraComprobante.Location = new Point(0, 302);
            pnlBotoneraComprobante.Name = "pnlBotoneraComprobante";
            pnlBotoneraComprobante.Size = new Size(349, 45);
            pnlBotoneraComprobante.TabIndex = 4;
            // 
            // btnDesmarcar
            // 
            btnDesmarcar.FlatStyle = FlatStyle.Flat;
            btnDesmarcar.IconChar = FontAwesome.Sharp.IconChar.Clone;
            btnDesmarcar.IconColor = Color.Black;
            btnDesmarcar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnDesmarcar.IconSize = 20;
            btnDesmarcar.Location = new Point(47, 6);
            btnDesmarcar.Name = "btnDesmarcar";
            btnDesmarcar.Size = new Size(33, 31);
            btnDesmarcar.TabIndex = 1;
            btnDesmarcar.UseVisualStyleBackColor = true;
            // 
            // btnMarcar
            // 
            btnMarcar.FlatStyle = FlatStyle.Flat;
            btnMarcar.ForeColor = Color.FromArgb(64, 64, 64);
            btnMarcar.IconChar = FontAwesome.Sharp.IconChar.CheckDouble;
            btnMarcar.IconColor = Color.FromArgb(64, 64, 64);
            btnMarcar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnMarcar.IconSize = 20;
            btnMarcar.Location = new Point(8, 6);
            btnMarcar.Name = "btnMarcar";
            btnMarcar.Size = new Size(33, 31);
            btnMarcar.TabIndex = 0;
            btnMarcar.UseVisualStyleBackColor = true;
            // 
            // pnlBuscar
            // 
            pnlBuscar.BackColor = Color.Gray;
            pnlBuscar.Controls.Add(btnBuscar);
            pnlBuscar.Controls.Add(txtBuscar);
            pnlBuscar.Controls.Add(label2);
            pnlBuscar.Dock = DockStyle.Top;
            pnlBuscar.Location = new Point(0, 0);
            pnlBuscar.Name = "pnlBuscar";
            pnlBuscar.Size = new Size(349, 40);
            pnlBuscar.TabIndex = 3;
            // 
            // btnBuscar
            // 
            btnBuscar.FlatAppearance.BorderColor = Color.WhiteSmoke;
            btnBuscar.FlatStyle = FlatStyle.Flat;
            btnBuscar.ForeColor = Color.WhiteSmoke;
            btnBuscar.IconChar = FontAwesome.Sharp.IconChar.MagnifyingGlass;
            btnBuscar.IconColor = Color.WhiteSmoke;
            btnBuscar.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnBuscar.IconSize = 15;
            btnBuscar.Location = new Point(287, 8);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(41, 23);
            btnBuscar.TabIndex = 2;
            btnBuscar.UseVisualStyleBackColor = true;
            // 
            // txtBuscar
            // 
            txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            txtBuscar.Location = new Point(58, 8);
            txtBuscar.Name = "txtBuscar";
            txtBuscar.Size = new Size(223, 23);
            txtBuscar.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.WhiteSmoke;
            label2.Location = new Point(10, 11);
            label2.Name = "label2";
            label2.Size = new Size(42, 15);
            label2.TabIndex = 0;
            label2.Text = "Buscar";
            // 
            // pnlContenedorDetalle
            // 
            pnlContenedorDetalle.Controls.Add(panel4);
            pnlContenedorDetalle.Controls.Add(panel3);
            pnlContenedorDetalle.Dock = DockStyle.Fill;
            pnlContenedorDetalle.Location = new Point(349, 59);
            pnlContenedorDetalle.Name = "pnlContenedorDetalle";
            pnlContenedorDetalle.Size = new Size(451, 347);
            pnlContenedorDetalle.TabIndex = 3;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(192, 64, 0);
            panel4.Dock = DockStyle.Left;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(2, 302);
            panel4.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.BackColor = Color.Gray;
            panel3.Controls.Add(label1);
            panel3.Controls.Add(txtTotal);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 302);
            panel3.Name = "panel3";
            panel3.Size = new Size(451, 45);
            panel3.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Right;
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(24, 7);
            label1.Name = "label1";
            label1.Size = new Size(164, 30);
            label1.TabIndex = 1;
            label1.Text = "TOTAL A PAGAR";
            // 
            // txtTotal
            // 
            txtTotal.Anchor = AnchorStyles.Right;
            txtTotal.BackColor = Color.FromArgb(64, 64, 64);
            txtTotal.BorderStyle = BorderStyle.FixedSingle;
            txtTotal.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point);
            txtTotal.Location = new Point(194, 3);
            txtTotal.Name = "txtTotal";
            txtTotal.Size = new Size(252, 39);
            txtTotal.TabIndex = 0;
            // 
            // _00158_CajaExterna
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(pnlContenedorDetalle);
            Controls.Add(pnlComprobantes);
            Controls.Add(pnlInferior);
            Name = "_00158_CajaExterna";
            Text = "Sidkenu";
            WindowState = FormWindowState.Maximized;
            Shown += _00158_CajaExterna_Shown;
            Controls.SetChildIndex(pnlInferior, 0);
            Controls.SetChildIndex(pnlComprobantes, 0);
            Controls.SetChildIndex(pnlContenedorDetalle, 0);
            pnlInferior.ResumeLayout(false);
            pnlComprobantes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvGrillaComprobante).EndInit();
            pnlBotoneraComprobante.ResumeLayout(false);
            pnlBuscar.ResumeLayout(false);
            pnlBuscar.PerformLayout();
            pnlContenedorDetalle.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlInferior;
        private Panel pnlComprobantes;
        private Panel pnlContenedorDetalle;
        private Panel panel3;
        private FontAwesome.Sharp.IconButton btnFacturar;
        private Label label1;
        private TextBox txtTotal;
        private Panel pnlBuscar;
        private FontAwesome.Sharp.IconButton btnBuscar;
        private TextBox txtBuscar;
        private Label label2;
        private Panel pnlBotoneraComprobante;
        private FontAwesome.Sharp.IconButton btnDesmarcar;
        private FontAwesome.Sharp.IconButton btnMarcar;
        public DataGridView dgvGrillaComprobante;
        private Panel panel4;
    }
}