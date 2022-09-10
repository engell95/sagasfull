namespace SAGAS.Reportes.Contabilidad.Hojas
{
    partial class SubRptEstadoResultado
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow20 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrCeLabel = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCeAnterior = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCeActual = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrCeAcumulado = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ghMain = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.lblGrupo = new DevExpress.XtraReports.UI.XRLabel();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrSumaAnterior = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrSumaActual = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrSumaAcumulado = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable3});
            this.Detail.HeightF = 18F;
            this.Detail.MultiColumn.ColumnWidth = 90F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable3
            // 
            this.xrTable3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(105F, 0F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow20});
            this.xrTable3.SizeF = new System.Drawing.SizeF(590F, 18F);
            this.xrTable3.StylePriority.UseBorders = false;
            // 
            // xrTableRow20
            // 
            this.xrTableRow20.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrCeLabel,
            this.xrCeAnterior,
            this.xrCeActual,
            this.xrCeAcumulado});
            this.xrTableRow20.Name = "xrTableRow20";
            this.xrTableRow20.Weight = 1D;
            // 
            // xrCeLabel
            // 
            this.xrCeLabel.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrCeLabel.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrCeLabel.Multiline = true;
            this.xrCeLabel.Name = "xrCeLabel";
            this.xrCeLabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 1, 0, 0, 100F);
            this.xrCeLabel.StylePriority.UseBorders = false;
            this.xrCeLabel.StylePriority.UseFont = false;
            this.xrCeLabel.StylePriority.UsePadding = false;
            this.xrCeLabel.StylePriority.UseTextAlignment = false;
            this.xrCeLabel.Text = "xrCeLabel";
            this.xrCeLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrCeLabel.Weight = 2.159354460673987D;
            // 
            // xrCeAnterior
            // 
            this.xrCeAnterior.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrCeAnterior.Multiline = true;
            this.xrCeAnterior.Name = "xrCeAnterior";
            this.xrCeAnterior.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.xrCeAnterior.StylePriority.UseFont = false;
            this.xrCeAnterior.StylePriority.UsePadding = false;
            this.xrCeAnterior.StylePriority.UseTextAlignment = false;
            this.xrCeAnterior.Text = "<>";
            this.xrCeAnterior.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrCeAnterior.Weight = 0.68565386203219181D;
            // 
            // xrCeActual
            // 
            this.xrCeActual.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrCeActual.Name = "xrCeActual";
            this.xrCeActual.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.xrCeActual.StylePriority.UseFont = false;
            this.xrCeActual.StylePriority.UsePadding = false;
            this.xrCeActual.StylePriority.UseTextAlignment = false;
            this.xrCeActual.Text = "<>";
            this.xrCeActual.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrCeActual.Weight = 0.72812790636731961D;
            // 
            // xrCeAcumulado
            // 
            this.xrCeAcumulado.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrCeAcumulado.Name = "xrCeAcumulado";
            this.xrCeAcumulado.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.xrCeAcumulado.StylePriority.UseFont = false;
            this.xrCeAcumulado.StylePriority.UsePadding = false;
            this.xrCeAcumulado.StylePriority.UseTextAlignment = false;
            this.xrCeAcumulado.Text = "<>";
            this.xrCeAcumulado.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrCeAcumulado.Weight = 0.72281815925530091D;
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 0F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ghMain
            // 
            this.ghMain.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblGrupo});
            this.ghMain.HeightF = 26.04167F;
            this.ghMain.Name = "ghMain";
            // 
            // lblGrupo
            // 
            this.lblGrupo.Font = new System.Drawing.Font("Tahoma", 9F);
            this.lblGrupo.LocationFloat = new DevExpress.Utils.PointFloat(105F, 0F);
            this.lblGrupo.Name = "lblGrupo";
            this.lblGrupo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblGrupo.SizeF = new System.Drawing.SizeF(400.7292F, 26.04167F);
            this.lblGrupo.StylePriority.UseFont = false;
            this.lblGrupo.StylePriority.UseTextAlignment = false;
            this.lblGrupo.Text = "lblGrupo";
            this.lblGrupo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.GroupFooter1.HeightF = 18F;
            this.GroupFooter1.Name = "GroupFooter1";
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(105F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(589.9999F, 18F);
            this.xrTable1.StylePriority.UseBorders = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrSumaAnterior,
            this.xrSumaActual,
            this.xrSumaAcumulado});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrTableCell1.Font = new System.Drawing.Font("Tahoma", 8F, System.Drawing.FontStyle.Bold);
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrTableCell1.StylePriority.UseBorders = false;
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UsePadding = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Total";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell1.Weight = 1.7499468409330843D;
            // 
            // xrSumaAnterior
            // 
            this.xrSumaAnterior.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold);
            this.xrSumaAnterior.Multiline = true;
            this.xrSumaAnterior.Name = "xrSumaAnterior";
            this.xrSumaAnterior.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.xrSumaAnterior.StylePriority.UseFont = false;
            this.xrSumaAnterior.StylePriority.UsePadding = false;
            this.xrSumaAnterior.StylePriority.UseTextAlignment = false;
            this.xrSumaAnterior.Text = "<>";
            this.xrSumaAnterior.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrSumaAnterior.Weight = 0.55565561104217776D;
            // 
            // xrSumaActual
            // 
            this.xrSumaActual.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold);
            this.xrSumaActual.Name = "xrSumaActual";
            this.xrSumaActual.StylePriority.UseFont = false;
            this.xrSumaActual.StylePriority.UseTextAlignment = false;
            this.xrSumaActual.Text = "<>";
            this.xrSumaActual.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrSumaActual.Weight = 0.5900769896262632D;
            // 
            // xrSumaAcumulado
            // 
            this.xrSumaAcumulado.Font = new System.Drawing.Font("Tahoma", 7F, System.Drawing.FontStyle.Bold);
            this.xrSumaAcumulado.Name = "xrSumaAcumulado";
            this.xrSumaAcumulado.StylePriority.UseFont = false;
            this.xrSumaAcumulado.StylePriority.UseTextAlignment = false;
            this.xrSumaAcumulado.Text = "<>";
            this.xrSumaAcumulado.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrSumaAcumulado.Weight = 0.58577332584635777D;
            // 
            // SubRptEstadoResultado
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ghMain,
            this.GroupFooter1});
            this.Margins = new System.Drawing.Printing.Margins(0, 0, 0, 0);
            this.PageHeight = 54;
            this.PageWidth = 725;
            this.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.ShowPrintMarginsWarning = false;
            this.Version = "14.1";
            this.Watermark.Font = new System.Drawing.Font("Verdana", 66F);
            this.Watermark.ForeColor = System.Drawing.Color.LightGray;
            this.Watermark.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Zoom;
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        public DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRTable xrTable3;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow20;
        public DevExpress.XtraReports.UI.XRTableCell xrCeLabel;
        private DevExpress.XtraReports.UI.XRTableCell xrCeAnterior;
        private DevExpress.XtraReports.UI.GroupHeaderBand ghMain;
        public DevExpress.XtraReports.UI.XRLabel lblGrupo;
        private DevExpress.XtraReports.UI.GroupFooterBand GroupFooter1;
        private DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        public DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableCell xrSumaAnterior;
        private DevExpress.XtraReports.UI.XRTableCell xrCeActual;
        private DevExpress.XtraReports.UI.XRTableCell xrCeAcumulado;
        private DevExpress.XtraReports.UI.XRTableCell xrSumaActual;
        private DevExpress.XtraReports.UI.XRTableCell xrSumaAcumulado;
    }
}
