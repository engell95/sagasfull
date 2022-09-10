using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace SAGAS.Reportes.Arqueos.Hojas
{
    public partial class SubRptProductoCupon : XtraReport
    {
        public SubRptProductoCupon(int color)
        {
            InitializeComponent();
            this.xrCellProducto.BackColor = Color.FromArgb(color);
            this.xrCellProducto.DataBindings.Add("Text", null, "ProductoNombre");
            this.xrCellCant.DataBindings.Add("Text", null, "Cantidad", "{0:n2}");
            this.xrCellLitros.DataBindings.Add("Text", null, "Litros", "{0:n3}");
            this.xrCellGlns.DataBindings.Add("Text", null, "Galones", "{0:n2}");
            this.xrCellValores.DataBindings.Add("Text", null, "Valor", "{0:n2}");
            this.xrCellTotalCant.DataBindings.Add("Text", null, "Cantidad", "{0:n2}");
            this.xrCellTotalLitros.DataBindings.Add("Text", null, "Litros", "{0:n3}");
            this.xrCellTotalGlns.DataBindings.Add("Text", null, "Galones", "{0:n2}");
            this.xrCellTotalValores.DataBindings.Add("Text", null, "Valor", "{0:n2}");

            this.xrCellTotalCant.Summary = new XRSummary(SummaryRunning.Group, SummaryFunc.Sum, "{0:n2}");
            this.xrCellTotalLitros.Summary = new XRSummary(SummaryRunning.Group, SummaryFunc.Sum, "{0:n2}");
            this.xrCellTotalGlns.Summary = new XRSummary(SummaryRunning.Group, SummaryFunc.Sum, "{0:n2}");
            this.xrCellTotalValores.Summary = new XRSummary(SummaryRunning.Group, SummaryFunc.Sum, "{0:n2}");

        }

        public SubRptProductoCupon(int color, bool valor)
        {
            InitializeComponent();
            if (valor)
            {
                this.xrCellProducto.BackColor = Color.FromArgb(color);
                this.xrCellProducto.DataBindings.Add("Text", null, "ProductoNombre");

                this.xrTableCellCantTitle.Dispose();
                this.xrTableCellLtrsTitle.WidthF = 60f;
                this.xrTableCellGlnsTitle.WidthF = 60f;
                this.xrTableCellValTitle.WidthF = 60f;

                this.xrCellCant.Dispose();
                this.xrCellLitros.WidthF = 60f;
                this.xrCellGlns.WidthF = 60f;
                this.xrCellValores.WidthF = 60f;
                this.xrCellLitros.DataBindings.Add("Text", null, "Litros", "{0:n3}");
                this.xrCellGlns.DataBindings.Add("Text", null, "Galones", "{0:n2}");
                this.xrCellValores.DataBindings.Add("Text", null, "Valor", "{0:n2}");

                this.xrCellTotalCant.Dispose();
                this.xrCellTotalLitros.WidthF = 60f;
                this.xrCellTotalGlns.WidthF = 60f;
                this.xrCellTotalValores.WidthF = 60f;
                this.xrCellTotalLitros.DataBindings.Add("Text", null, "Litros", "{0:n3}");
                this.xrCellTotalGlns.DataBindings.Add("Text", null, "Galones", "{0:n2}");
                this.xrCellTotalValores.DataBindings.Add("Text", null, "Valor", "{0:n2}");

                this.xrCellTotalCant.Dispose();
                this.xrCellTotalLitros.Summary = new XRSummary(SummaryRunning.Group, SummaryFunc.Sum, "{0:n2}");
                this.xrCellTotalGlns.Summary = new XRSummary(SummaryRunning.Group, SummaryFunc.Sum, "{0:n2}");
                this.xrCellTotalValores.Summary = new XRSummary(SummaryRunning.Group, SummaryFunc.Sum, "{0:n2}");
            }
            else
            {
                this.xrCellProducto.BackColor = Color.FromArgb(color);
                this.xrCellProducto.DataBindings.Add("Text", null, "ProductoNombre");

                this.xrTableCellCantTitle.Dispose();
                this.xrTableCellLtrsTitle.WidthF = 60f;
                this.xrTableCellGlnsTitle.WidthF = 60f;
                this.xrTableCellValTitle.WidthF = 60f;

                this.xrCellCant.Dispose();
                this.xrCellLitros.WidthF = 60f;
                this.xrCellGlns.WidthF = 60f;
                this.xrCellValores.WidthF = 60f;
                this.xrCellLitros.DataBindings.Add("Text", null, "Litros", "{0:n3}");
                this.xrCellGlns.DataBindings.Add("Text", null, "Galones", "{0:n2}");
                this.xrCellValores.DataBindings.Add("Text", null, "Valor", "{0:n2}");
                this.xrTable1.Dispose();
            }
        }

        private void xrCellValores_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            XRTableCell cell = (XRTableCell)sender;
            if (cell.Text.Equals("0.00") || cell.Text.Equals("0.000"))
                cell.Text = "-";
        }

      

    }
}
