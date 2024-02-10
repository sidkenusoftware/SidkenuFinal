﻿namespace SidkenuWF.Formularios.Core
{
    partial class _00129_CondicionIva_Abm
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
            panel1 = new Panel();
            label4 = new Label();
            label3 = new Label();
            chkEmpresa = new Base.Controles.SidkenuToggleButton();
            label2 = new Label();
            txtCodigo = new TextBox();
            label1 = new Label();
            txtDescripcion = new TextBox();
            nudCondicionIva = new NumericUpDown();
            label20 = new Label();
            chkActivarParaFacturaElectronica = new Base.Controles.SidkenuToggleButton();
            label5 = new Label();
            ((System.ComponentModel.ISupportInitialize)nudCondicionIva).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            panel1.BackColor = Color.LightGray;
            panel1.Location = new Point(9, 258);
            panel1.Name = "panel1";
            panel1.Size = new Size(522, 1);
            panel1.TabIndex = 188;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label4.AutoSize = true;
            label4.BackColor = Color.Transparent;
            label4.Font = new Font("Arial", 8.25F, FontStyle.Regular, GraphicsUnit.Point);
            label4.ForeColor = Color.FromArgb(192, 0, 0);
            label4.Location = new Point(248, 264);
            label4.Name = "label4";
            label4.Size = new Size(280, 56);
            label4.TabIndex = 187;
            label4.Text = "Nota: si activa esta opción los datos que esta \r\ncargando solo podra ser visualizado por la empresa que\r\nlo esta dando de alta y no en las sucursales en caso de \r\ntenerlas.\r\n";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label3.ForeColor = Color.FromArgb(64, 64, 64);
            label3.Location = new Point(54, 284);
            label3.Name = "label3";
            label3.Size = new Size(186, 16);
            label3.TabIndex = 186;
            label3.Text = "Pertenece solo a una Empresa";
            // 
            // chkEmpresa
            // 
            chkEmpresa.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            chkEmpresa.AutoSize = true;
            chkEmpresa.Location = new Point(9, 281);
            chkEmpresa.MinimumSize = new Size(45, 22);
            chkEmpresa.Name = "chkEmpresa";
            chkEmpresa.OffBackColor = Color.Gray;
            chkEmpresa.OffToggleColor = Color.Gainsboro;
            chkEmpresa.OnBackColor = Color.Green;
            chkEmpresa.OnToggleColor = Color.WhiteSmoke;
            chkEmpresa.Size = new Size(45, 22);
            chkEmpresa.TabIndex = 185;
            chkEmpresa.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = Color.FromArgb(64, 64, 64);
            label2.Location = new Point(15, 64);
            label2.Name = "label2";
            label2.Size = new Size(47, 16);
            label2.TabIndex = 184;
            label2.Text = "Codigo";
            // 
            // txtCodigo
            // 
            txtCodigo.BackColor = Color.WhiteSmoke;
            txtCodigo.BorderStyle = BorderStyle.FixedSingle;
            txtCodigo.Font = new Font("Arial", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            txtCodigo.ForeColor = Color.FromArgb(64, 64, 64);
            txtCodigo.Location = new Point(12, 85);
            txtCodigo.Name = "txtCodigo";
            txtCodigo.Size = new Size(158, 29);
            txtCodigo.TabIndex = 181;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.FromArgb(64, 64, 64);
            label1.Location = new Point(15, 131);
            label1.Name = "label1";
            label1.Size = new Size(150, 16);
            label1.TabIndex = 183;
            label1.Text = "Nombre Condicion de Iva";
            // 
            // txtDescripcion
            // 
            txtDescripcion.BackColor = Color.WhiteSmoke;
            txtDescripcion.BorderStyle = BorderStyle.FixedSingle;
            txtDescripcion.Font = new Font("Arial", 24F, FontStyle.Regular, GraphicsUnit.Point);
            txtDescripcion.ForeColor = Color.FromArgb(64, 64, 64);
            txtDescripcion.Location = new Point(12, 152);
            txtDescripcion.Name = "txtDescripcion";
            txtDescripcion.Size = new Size(516, 44);
            txtDescripcion.TabIndex = 182;
            // 
            // nudCondicionIva
            // 
            nudCondicionIva.DecimalPlaces = 2;
            nudCondicionIva.Font = new Font("Segoe UI", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            nudCondicionIva.Location = new Point(56, 215);
            nudCondicionIva.Maximum = new decimal(new int[] { 1215752192, 23, 0, 0 });
            nudCondicionIva.Name = "nudCondicionIva";
            nudCondicionIva.Size = new Size(95, 27);
            nudCondicionIva.TabIndex = 191;
            nudCondicionIva.TextAlign = HorizontalAlignment.Right;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.BackColor = Color.Transparent;
            label20.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label20.ForeColor = Color.FromArgb(64, 64, 64);
            label20.Location = new Point(281, 220);
            label20.Name = "label20";
            label20.Size = new Size(193, 16);
            label20.TabIndex = 190;
            label20.Text = "Marcar para Factura Electrónica";
            // 
            // chkActivarParaFacturaElectronica
            // 
            chkActivarParaFacturaElectronica.AutoSize = true;
            chkActivarParaFacturaElectronica.Location = new Point(480, 218);
            chkActivarParaFacturaElectronica.MinimumSize = new Size(45, 22);
            chkActivarParaFacturaElectronica.Name = "chkActivarParaFacturaElectronica";
            chkActivarParaFacturaElectronica.OffBackColor = Color.Gray;
            chkActivarParaFacturaElectronica.OffToggleColor = Color.Gainsboro;
            chkActivarParaFacturaElectronica.OnBackColor = Color.Green;
            chkActivarParaFacturaElectronica.OnToggleColor = Color.WhiteSmoke;
            chkActivarParaFacturaElectronica.Size = new Size(45, 22);
            chkActivarParaFacturaElectronica.TabIndex = 189;
            chkActivarParaFacturaElectronica.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Arial", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label5.ForeColor = Color.FromArgb(64, 64, 64);
            label5.Location = new Point(12, 220);
            label5.Name = "label5";
            label5.Size = new Size(36, 16);
            label5.TabIndex = 192;
            label5.Text = "Valor";
            // 
            // _00129_CondicionIva_Abm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(537, 363);
            Controls.Add(label5);
            Controls.Add(nudCondicionIva);
            Controls.Add(label20);
            Controls.Add(chkActivarParaFacturaElectronica);
            Controls.Add(panel1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(chkEmpresa);
            Controls.Add(label2);
            Controls.Add(txtCodigo);
            Controls.Add(label1);
            Controls.Add(txtDescripcion);
            MaximumSize = new Size(553, 402);
            MinimumSize = new Size(553, 402);
            Name = "_00129_CondicionIva_Abm";
            Text = "Condicion de Iva";
            Load += _00129_CondicionIva_Abm_Load;
            Controls.SetChildIndex(txtDescripcion, 0);
            Controls.SetChildIndex(label1, 0);
            Controls.SetChildIndex(txtCodigo, 0);
            Controls.SetChildIndex(label2, 0);
            Controls.SetChildIndex(chkEmpresa, 0);
            Controls.SetChildIndex(label3, 0);
            Controls.SetChildIndex(label4, 0);
            Controls.SetChildIndex(panel1, 0);
            Controls.SetChildIndex(chkActivarParaFacturaElectronica, 0);
            Controls.SetChildIndex(label20, 0);
            Controls.SetChildIndex(nudCondicionIva, 0);
            Controls.SetChildIndex(label5, 0);
            ((System.ComponentModel.ISupportInitialize)nudCondicionIva).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Label label4;
        private Label label3;
        private Base.Controles.SidkenuToggleButton chkEmpresa;
        private Label label2;
        private TextBox txtCodigo;
        private Label label1;
        private TextBox txtDescripcion;
        private NumericUpDown nudCondicionIva;
        private Label label20;
        private Base.Controles.SidkenuToggleButton chkActivarParaFacturaElectronica;
        private Label label5;
    }
}