using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Arqueos.Hojas
{
    public partial class SubRptValorDistribucion : XtraReport
    {
        public SubRptValorDistribucion(string Valor, int Decimales, float Ancho)
        {
            InitializeComponent();
            this.xrTable3.WidthF = Ancho;
            this.xrTableFooter.WidthF = Ancho;
            this.xrCellList.DataBindings.Add("Text", null, Valor, "{0:n" + Decimales +"}");
            this.xrCellListTotal.DataBindings.Add("Text", null, Valor, "{0:n" + Decimales + "}");

            this.xrCellListTotal.Summary = new XRSummary(SummaryRunning.Group, SummaryFunc.Sum, "{0:n" + Decimales + "}");
        }

        private void xrCellList_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            if (cell.Text.Equals("0.00") || cell.Text.Equals("0.000"))
                cell.Text = "-";
        }

      

    }
}
