using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Nomina.Hojas
{
    public partial class comprobantePago : DevExpress.XtraReports.UI.XtraReport
    {
        public comprobantePago(object Procedure)
        {
            try
            {
                InitializeComponent();
                this.DataSource = Procedure;

                System.Collections.Generic.IEnumerable<XRTableCell> celdas = this.AllControls<XRTableCell>();

                foreach (XRTableCell celda in celdas)
                {
                    if (!String.IsNullOrEmpty(celda.Tag.ToString()))
                    {
                        celda.DataBindings.Add("Text", Procedure, celda.Tag.ToString(), "{0:N2}");
                    }
                }

            }
            catch (Exception ex)
            {
                Parametros.General.DialogMsg(Parametros.Properties.Resources.MSGERROR, Parametros.MsgType.error, ex.ToString());
            }
        }

        private void xrTableCell17_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            XRTableCell celda = (XRTableCell)sender;

            if (!String.IsNullOrEmpty(celda.Text))
            {
                if (Convert.ToDecimal(celda.Text).Equals(0m))
                    celda.Text = "";

            }
            else
                celda.Text = "";
        }

    }
}
