using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnGenerar_Click(object sender, EventArgs e)
        {
            tabla.GrowStyle = TableLayoutPanelGrowStyle.FixedSize;

            tabla.RowCount = (int)nudFila.Value;
            tabla.ColumnCount = (int)nudColumna.Value;

            for (int i = 0; i < nudFila.Value; i++) 
            {
                for (int j = 0; j < nudColumna.Value; j++) 
                {
                    var ctrol = new UserControl1
                    {
                        AutoSize = true,
                        Dock = DockStyle.Fill
                    };

                    tabla.Controls.Add(ctrol, j, i);
                }
            }
        }
    }
}