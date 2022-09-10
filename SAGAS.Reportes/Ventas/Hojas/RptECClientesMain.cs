using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Ventas.Hojas
{
    public partial class RptECClientesMain : DevExpress.XtraReports.UI.XtraReport
    {
        public RptECClientesMain()
        {
            InitializeComponent();
        }

        private void PageFooter_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
           
        }

        private void xrCeSaldoFInal_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
           
        }

        private void xrTable2_PrintOnPage(object sender, PrintOnPageEventArgs e)
        {
            if (!e.PageCount.Equals((e.PageIndex + 1)))
                xrTable2.Visible = false;

        }
              
    }
}
