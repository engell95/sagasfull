namespace SAGAS.Reportes.Ventas.Hojas
{
    partial class RptECClientesMain
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
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.formattingRuleDebito = new DevExpress.XtraReports.UI.FormattingRule();
            this.formattingRuleCredito = new DevExpress.XtraReports.UI.FormattingRule();
            this.EsContado = new DevExpress.XtraReports.UI.CalculatedField();
            this.HasFechaVencimiento = new DevExpress.XtraReports.UI.CalculatedField();
            this.HasPlazo = new DevExpress.XtraReports.UI.CalculatedField();
            this.FechaVencimientoValue = new DevExpress.XtraReports.UI.CalculatedField();
            this.PlazoValue = new DevExpress.XtraReports.UI.CalculatedField();
            this.TotalK = new DevExpress.XtraReports.UI.CalculatedField();
            this.AlmacenNombre = new DevExpress.XtraReports.UI.CalculatedField();
            this.HasProveedorReferencia = new DevExpress.XtraReports.UI.CalculatedField();
            this.ReferenciaValue = new DevExpress.XtraReports.UI.CalculatedField();
            this.CantidadValue = new DevExpress.XtraReports.UI.CalculatedField();
            this.CostoValue = new DevExpress.XtraReports.UI.CalculatedField();
            this.ProveedorReferenciaValue = new DevExpress.XtraReports.UI.CalculatedField();
            this.TotalOD = new DevExpress.XtraReports.UI.CalculatedField();
            this.GroupH = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.bindingSource1 = new System.Windows.Forms.BindingSource();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrCeSaldoFInal = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
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
            // formattingRuleDebito
            // 
            this.formattingRuleDebito.Condition = "[Debito] == 0";
            this.formattingRuleDebito.DataMember = "VistaComprobantes";
            // 
            // 
            // 
            this.formattingRuleDebito.Formatting.ForeColor = System.Drawing.Color.Transparent;
            this.formattingRuleDebito.Name = "formattingRuleDebito";
            // 
            // formattingRuleCredito
            // 
            this.formattingRuleCredito.Condition = "[Credito] == 0";
            this.formattingRuleCredito.DataMember = "VistaComprobantes";
            // 
            // 
            // 
            this.formattingRuleCredito.Formatting.ForeColor = System.Drawing.Color.Transparent;
            this.formattingRuleCredito.Name = "formattingRuleCredito";
            // 
            // EsContado
            // 
            this.EsContado.Expression = "Iif([Credito],\'CREDITO\',\'CONTADO\' )";
            this.EsContado.Name = "EsContado";
            // 
            // HasFechaVencimiento
            // 
            this.HasFechaVencimiento.Expression = "Iif([Credito],\'Fecha Vencimiento   :\',\' \' )";
            this.HasFechaVencimiento.Name = "HasFechaVencimiento";
            // 
            // HasPlazo
            // 
            this.HasPlazo.Expression = "Iif([Credito],\'Plazo   :\',\' \' )";
            this.HasPlazo.Name = "HasPlazo";
            // 
            // FechaVencimientoValue
            // 
            this.FechaVencimientoValue.Expression = "Iif([Credito],[FechaVencimiento],\' \')";
            this.FechaVencimientoValue.FieldType = DevExpress.XtraReports.UI.FieldType.DateTime;
            this.FechaVencimientoValue.Name = "FechaVencimientoValue";
            // 
            // PlazoValue
            // 
            this.PlazoValue.Expression = "Iif([Credito],DateDiffDay([FechaFisico],[FechaVencimiento]),\' \')";
            this.PlazoValue.Name = "PlazoValue";
            // 
            // TotalK
            // 
            this.TotalK.DataMember = "VistaKardexes";
            this.TotalK.Expression = "[ImpuestoTotal]+[CostoTotal]";
            this.TotalK.Name = "TotalK";
            // 
            // AlmacenNombre
            // 
            this.AlmacenNombre.DataMember = "VistaKardexes";
            this.AlmacenNombre.Expression = "Iif([AlmacenEntradaID] != 0,  [AlmacenEntradaNombre],[AlmacenSalidaNombre] )";
            this.AlmacenNombre.Name = "AlmacenNombre";
            // 
            // HasProveedorReferencia
            // 
            this.HasProveedorReferencia.Expression = "Iif([Credito],\' \',\'Proveedor Referencia:  \' )";
            this.HasProveedorReferencia.Name = "HasProveedorReferencia";
            // 
            // ReferenciaValue
            // 
            this.ReferenciaValue.Name = "ReferenciaValue";
            // 
            // CantidadValue
            // 
            this.CantidadValue.DataMember = "VistaKardexes";
            this.CantidadValue.Expression = "Iif([AlmacenEntradaID] != 0,  [CantidadEntrada],[CantidadSalida] )";
            this.CantidadValue.Name = "CantidadValue";
            // 
            // CostoValue
            // 
            this.CostoValue.DataMember = "VistaKardexes";
            this.CostoValue.Expression = "Iif([AlmacenEntradaID] != 0,  [CostoEntrada],[CostoSalida] )";
            this.CostoValue.Name = "CostoValue";
            // 
            // ProveedorReferenciaValue
            // 
            this.ProveedorReferenciaValue.Expression = "Iif([Credito],\' \',[PreveedorReferencia])";
            this.ProveedorReferenciaValue.Name = "ProveedorReferenciaValue";
            // 
            // TotalOD
            // 
            this.TotalOD.DataMember = "VistaOrdenesComprasDetalles";
            this.TotalOD.Expression = "[ImpuestoTotal]+[CostoTotal]";
            this.TotalOD.Name = "TotalOD";
            // 
            // GroupH
            // 
            this.GroupH.HeightF = 0F;
            this.GroupH.Name = "GroupH";
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(SAGAS.Entidad.VistaActaManejo);
            // 
            // ReportFooter
            // 
            this.ReportFooter.HeightF = 21.875F;
            this.ReportFooter.KeepTogether = true;
            this.ReportFooter.Name = "ReportFooter";
            this.ReportFooter.PrintAtBottom = true;
            // 
            // xrCeSaldoFInal
            // 
            this.xrCeSaldoFInal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.xrCeSaldoFInal.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.xrCeSaldoFInal.Name = "xrCeSaldoFInal";
            this.xrCeSaldoFInal.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 0, 0, 0, 100F);
            this.xrCeSaldoFInal.StylePriority.UseBackColor = false;
            this.xrCeSaldoFInal.StylePriority.UseFont = false;
            this.xrCeSaldoFInal.StylePriority.UsePadding = false;
            this.xrCeSaldoFInal.StylePriority.UseTextAlignment = false;
            this.xrCeSaldoFInal.Text = "<>";
            this.xrCeSaldoFInal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrCeSaldoFInal.Weight = 0.55299539170506906D;
            this.xrCeSaldoFInal.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrCeSaldoFInal_PrintOnPage);
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 2, 0, 0, 100F);
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UsePadding = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Total Saldo Final";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell1.Weight = 2.310483870967742D;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrCeSaldoFInal});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTable2
            // 
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(651F, 20F);
            this.xrTable2.Visible = false;
            this.xrTable2.PrintOnPage += new DevExpress.XtraReports.UI.PrintOnPageEventHandler(this.xrTable2_PrintOnPage);
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable2});
            this.PageFooter.HeightF = 23.95833F;
            this.PageFooter.Name = "PageFooter";
            this.PageFooter.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.PageFooter_BeforePrint);
            // 
            // RptECClientesMain
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupH,
            this.ReportFooter,
            this.PageFooter});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.EsContado,
            this.HasFechaVencimiento,
            this.HasPlazo,
            this.FechaVencimientoValue,
            this.PlazoValue,
            this.TotalK,
            this.AlmacenNombre,
            this.HasProveedorReferencia,
            this.ReferenciaValue,
            this.CantidadValue,
            this.CostoValue,
            this.ProveedorReferenciaValue,
            this.TotalOD});
            this.DataSource = this.bindingSource1;
            this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.formattingRuleDebito,
            this.formattingRuleCredito});
            this.Margins = new System.Drawing.Printing.Margins(16, 10, 0, 0);
            this.ShowPrintMarginsWarning = false;
            this.Version = "14.1";
            this.Watermark.Font = new System.Drawing.Font("Verdana", 66F);
            this.Watermark.ForeColor = System.Drawing.Color.LightGray;
            this.Watermark.ShowBehind = false;
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        public DevExpress.XtraReports.UI.DetailBand Detail;
        private System.Windows.Forms.BindingSource bindingSource1;
        private DevExpress.XtraReports.UI.CalculatedField EsContado;
        private DevExpress.XtraReports.UI.CalculatedField HasFechaVencimiento;
        private DevExpress.XtraReports.UI.CalculatedField HasPlazo;
        private DevExpress.XtraReports.UI.CalculatedField FechaVencimientoValue;
        private DevExpress.XtraReports.UI.CalculatedField PlazoValue;
        private DevExpress.XtraReports.UI.CalculatedField TotalK;
        private DevExpress.XtraReports.UI.CalculatedField AlmacenNombre;
        private DevExpress.XtraReports.UI.FormattingRule formattingRuleDebito;
        private DevExpress.XtraReports.UI.FormattingRule formattingRuleCredito;
        private DevExpress.XtraReports.UI.CalculatedField HasProveedorReferencia;
        private DevExpress.XtraReports.UI.CalculatedField ReferenciaValue;
        private DevExpress.XtraReports.UI.CalculatedField CantidadValue;
        private DevExpress.XtraReports.UI.CalculatedField CostoValue;
        private DevExpress.XtraReports.UI.CalculatedField ProveedorReferenciaValue;
        private DevExpress.XtraReports.UI.CalculatedField TotalOD;
        public DevExpress.XtraReports.UI.GroupHeaderBand GroupH;
        private DevExpress.XtraReports.UI.ReportFooterBand ReportFooter;
        public DevExpress.XtraReports.UI.XRTableCell xrCeSaldoFInal;
        private DevExpress.XtraReports.UI.XRTableCell xrTableCell1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow2;
        private DevExpress.XtraReports.UI.XRTable xrTable2;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
    }
}
