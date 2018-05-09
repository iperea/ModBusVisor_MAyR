using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataModel;

namespace SCADAWinForms
{
    public partial class Report : Form
    {
        Modelo mod;

        public Report(Modelo m)
        {
            InitializeComponent();
            mod = m;
        }

        private void Report_Load(object sender, EventArgs e)
        {
            try
            {
                // TODO: esta línea de código carga datos en la tabla 'hmibdDataSet.vista_historial_botones' Puede moverla o quitarla según sea necesario.
                this.vista_historial_botonesTableAdapter.Fill(this.hmibdDataSet.vista_historial_botones);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

    }
}
