namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            label2 = new Label();
            label1 = new Label();
            btnGenerar = new Button();
            nudColumna = new NumericUpDown();
            nudFila = new NumericUpDown();
            tabla = new TableLayoutPanel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudColumna).BeginInit();
            ((System.ComponentModel.ISupportInitialize)nudFila).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(224, 224, 224);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(btnGenerar);
            panel1.Controls.Add(nudColumna);
            panel1.Controls.Add(nudFila);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 100);
            panel1.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(154, 35);
            label2.Name = "label2";
            label2.Size = new Size(56, 15);
            label2.TabIndex = 4;
            label2.Text = "Columna";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 35);
            label1.Name = "label1";
            label1.Size = new Size(25, 15);
            label1.TabIndex = 3;
            label1.Text = "Fila";
            // 
            // btnGenerar
            // 
            btnGenerar.Location = new Point(268, 53);
            btnGenerar.Name = "btnGenerar";
            btnGenerar.Size = new Size(75, 23);
            btnGenerar.TabIndex = 2;
            btnGenerar.Text = "Generar";
            btnGenerar.UseVisualStyleBackColor = true;
            btnGenerar.Click += BtnGenerar_Click;
            // 
            // nudColumna
            // 
            nudColumna.Location = new Point(154, 52);
            nudColumna.Name = "nudColumna";
            nudColumna.Size = new Size(108, 23);
            nudColumna.TabIndex = 1;
            // 
            // nudFila
            // 
            nudFila.Location = new Point(23, 53);
            nudFila.Name = "nudFila";
            nudFila.Size = new Size(120, 23);
            nudFila.TabIndex = 0;
            // 
            // tabla
            // 
            tabla.AutoSize = true;
            tabla.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            tabla.ColumnCount = 1;
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tabla.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tabla.Dock = DockStyle.Fill;
            tabla.Location = new Point(0, 100);
            tabla.Name = "tabla";
            tabla.RowCount = 1;
            tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tabla.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tabla.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tabla.Size = new Size(800, 350);
            tabla.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabla);
            Controls.Add(panel1);
            Name = "Form1";
            Text = "Form1";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudColumna).EndInit();
            ((System.ComponentModel.ISupportInitialize)nudFila).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private TableLayoutPanel tabla;
        private Label label2;
        private Label label1;
        private Button btnGenerar;
        private NumericUpDown nudColumna;
        private NumericUpDown nudFila;
    }
}