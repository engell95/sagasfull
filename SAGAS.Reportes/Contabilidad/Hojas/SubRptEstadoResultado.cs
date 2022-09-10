using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Contabilidad.Hojas
{
    public partial class SubRptEstadoResultado : XtraReport
    {
        public SubRptEstadoResultado(string PrimerValor, string MontoA, string MontoB, string MontoC, string texto, object Lista, bool MostsarEncabezado, bool MostrarPie)
        {
            InitializeComponent();
            this.DataSource = Lista;
            this.lblGrupo.Text = texto;
            this.xrCeLabel.DataBindings.Add("Text", null, PrimerValor, "");
            this.xrCeAnterior.DataBindings.Add("Text", null, MontoA, "{0:#,0.00;(#,0.00)}");
            this.xrCeActual.DataBindings.Add("Text", null, MontoB, "{0:#,0.00;(#,0.00)}");
            this.xrCeAcumulado.DataBindings.Add("Text", null, MontoC, "{0:#,0.00;(#,0.00)}");
            this.xrTableCell1.Text = "TOTAL  " + texto;

            this.xrSumaAnterior.DataBindings.Add("Text", null, MontoA);
            this.xrSumaAnterior.Summary = new DevExpress.XtraReports.UI.XRSummary(DevExpress.XtraReports.UI.SummaryRunning.Report, DevExpress.XtraReports.UI.SummaryFunc.Sum, "{0:#,0.00;(#,0.00)}");
            this.xrSumaActual.DataBindings.Add("Text", null, MontoB);
            this.xrSumaActual.Summary = new DevExpress.XtraReports.UI.XRSummary(DevExpress.XtraReports.UI.SummaryRunning.Report, DevExpress.XtraReports.UI.SummaryFunc.Sum, "{0:#,0.00;(#,0.00)}");
            this.xrSumaAcumulado.DataBindings.Add("Text", null, MontoC);
            this.xrSumaAcumulado.Summary = new DevExpress.XtraReports.UI.XRSummary(DevExpress.XtraReports.UI.SummaryRunning.Report, DevExpress.XtraReports.UI.SummaryFunc.Sum, "{0:#,0.00;(#,0.00)}");

            ghMain.Visible = MostsarEncabezado;
            GroupFooter1.Visible = MostrarPie;
        }
    }
}
