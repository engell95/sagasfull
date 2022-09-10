using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Contabilidad.Hojas
{
    public partial class SubRptBalance : XtraReport
    {
        public SubRptBalance(string PrimerValor, string SegundoValor, string texto, object Lista, bool ShowEnd, decimal vPasivo)
        {
            InitializeComponent();
            this.DataSource = Lista;
            this.lblGrupo.Text = texto;
            this.xrCeLabel.DataBindings.Add("Text", null, PrimerValor, "");
            this.xrCeMonto.DataBindings.Add("Text", null, SegundoValor, "{0:#,0.00;(#,0.00)}");
            this.xrTableCell1.Text = "TOTAL  " + texto;

            /* FOOTER FINAL, PARA LA SUMA INDEPENDIENTE DEL PASIVO Y DEL PATRIMONEO */
            gfEnd.Visible = gfBegin.Visible = ShowEnd;
            this.xrTableCell4.Text = vPasivo.ToString("0:#,0.00;(#,0.00)");

            this.xrSuma.DataBindings.Add("Text", null, SegundoValor);
            this.xrSuma.Summary = new DevExpress.XtraReports.UI.XRSummary(DevExpress.XtraReports.UI.SummaryRunning.Report, DevExpress.XtraReports.UI.SummaryFunc.Sum, "{0:#,0.00;(#,0.00)}");

            this.xrCeSumaTotal.DataBindings.Add("Text", null, SegundoValor);
            this.xrCeSumaTotal.Summary = new DevExpress.XtraReports.UI.XRSummary(DevExpress.XtraReports.UI.SummaryRunning.Report, DevExpress.XtraReports.UI.SummaryFunc.Sum, "{0:#,0.00;(#,0.00)}");
            
        }
    }
}
