using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Tesoreria.Hojas
{
    public partial class SubRptECProveedor : XtraReport
    {       
        public SubRptECProveedor()
        {
            InitializeComponent();
        }

        public SubRptECProveedor(DateTime vFecha)
        {
            InitializeComponent();
            this.Parameters["pFecha"].Value = vFecha;
            this.xrTableCell1.Text += vFecha.ToShortDateString();
        }

        private void xrTableCell28_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //_Saldo += (!String.IsNullOrEmpty(xrTableCell23.Text) ? Convert.ToDecimal(xrTableCell23.Text) : 0m);
            //xrTableCell28.Text = Convert.ToDecimal(_Saldo).ToString("#,0.00");
        }

        private void xrTableCell26_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //xrTableCell26.Text = Convert.ToDecimal(_Saldo).ToString("#,0.000");
            //xrTableCell27.Text = Decimal.Round(_Saldo / 3.7854m, 2, MidpointRounding.AwayFromZero).ToString("#,0.00");
        }



      

    }
}
