namespace SAGAS.Reportes.Inventario.Hojas
{
    partial class SubRptKardexCierre
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
            this.components = new System.ComponentModel.Container();
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
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.xrEstacion = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
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
            this.TopMargin.HeightF = 8.833329F;
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
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrEstacion});
            this.PageHeader.HeightF = 46.875F;
            this.PageHeader.Name = "PageHeader";
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(SAGAS.Entidad.VistaGetInventarioCorte);
            // 
            // xrEstacion
            // 
            this.xrEstacion.AutoWidth = true;
            this.xrEstacion.BackColor = System.Drawing.Color.PaleGreen;
            this.xrEstacion.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.xrEstacion.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrEstacion.Name = "xrEstacion";
            this.xrEstacion.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrEstacion.SizeF = new System.Drawing.SizeF(81.24997F, 25F);
            this.xrEstacion.StylePriority.UseBackColor = false;
            this.xrEstacion.StylePriority.UseFont = false;
            this.xrEstacion.StylePriority.UseTextAlignment = false;
            this.xrEstacion.Text = "<>";
            this.xrEstacion.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrEstacion.WordWrap = false;
            // 
            // SubRptKardexCierre
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageHeader});
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
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(16, 10, 9, 0);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.ShowPrintMarginsWarning = false;
            this.Version = "14.1";
            this.Watermark.Font = new System.Drawing.Font("Verdana", 66F);
            this.Watermark.ForeColor = System.Drawing.Color.LightGray;
            this.Watermark.ShowBehind = false;
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
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
        private DevExpress.XtraReports.UI.PageHeaderBand PageHeader;
        private DevExpress.XtraReports.UI.XRLabel xrEstacion;
    }
}
