using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace SAGAS.Parametros
{
    public partial class MyXtraGridCajaChica : DevExpress.XtraEditors.XtraUserControl
    {
        public MyXtraGridCajaChica()
        {
            InitializeComponent();
        }

        private void chkMostrar_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMostrar.Checked)
            {
                this.splitContainerControlBottom.PanelVisibility = SplitPanelVisibility.Both;
                this.chkMostrar.Image = Parametros.Properties.Resources.Ocultar;
                this.chkMostrar.Text = "Ocultar Retenciones";
            }
            else if (!chkMostrar.Checked)
            {
                this.splitContainerControlBottom.PanelVisibility = SplitPanelVisibility.Panel1;
                this.chkMostrar.Image = Parametros.Properties.Resources.Mostrar;
                this.chkMostrar.Text = "Mostrar Retenciones";
            }
        }

        
    }
}
