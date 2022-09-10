namespace SAGAS.Reportes.Contabilidad.Hojas
{
    partial class RptDetalleCuentas
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
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
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
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CeEmpresa = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.CeTitulo = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrCeRango = new DevExpress.XtraReports.UI.XRTableCell();
            this.PicLogo = new DevExpress.XtraReports.UI.XRPictureBox();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.bindingSource1 = new System.Windows.Forms.BindingSource();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
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
            this.TopMargin.HeightF = 22.375F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 12.36439F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageFooter
            // 
            this.PageFooter.HeightF = 0F;
            this.PageFooter.Name = "PageFooter";
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
            this.xrTable1,
            this.PicLogo});
            this.PageHeader.HeightF = 89.58337F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrTable1
            // 
            this.xrTable1.BorderColor = System.Drawing.Color.DimGray;
            this.xrTable1.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(148.125F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1,
            this.xrTableRow3,
            this.xrTableRow4});
            this.xrTable1.SizeF = new System.Drawing.SizeF(776.4583F, 65.62502F);
            this.xrTable1.StylePriority.UseBorderColor = false;
            this.xrTable1.StylePriority.UseFont = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CeEmpresa});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // CeEmpresa
            // 
            this.CeEmpresa.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.CeEmpresa.Name = "CeEmpresa";
            this.CeEmpresa.StylePriority.UseFont = false;
            this.CeEmpresa.StylePriority.UseTextAlignment = false;
            this.CeEmpresa.Text = "CeEmpresa";
            this.CeEmpresa.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CeEmpresa.Weight = 3D;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.CeTitulo});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // CeTitulo
            // 
            this.CeTitulo.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.CeTitulo.Name = "CeTitulo";
            this.CeTitulo.StylePriority.UseFont = false;
            this.CeTitulo.StylePriority.UseTextAlignment = false;
            this.CeTitulo.Text = "Movimiento Detalles de Mayor";
            this.CeTitulo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.CeTitulo.Weight = 3D;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrCeRango});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrCeRango
            // 
            this.xrCeRango.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.xrCeRango.Name = "xrCeRango";
            this.xrCeRango.StylePriority.UseFont = false;
            this.xrCeRango.StylePriority.UseTextAlignment = false;
            this.xrCeRango.Text = "<>";
            this.xrCeRango.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrCeRango.Weight = 3D;
            // 
            // PicLogo
            // 
            this.PicLogo.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 0F);
            this.PicLogo.Name = "PicLogo";
            this.PicLogo.SizeF = new System.Drawing.SizeF(138.125F, 65.62502F);
            this.PicLogo.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.HeightF = 0F;
            this.GroupHeader1.Name = "GroupHeader1";
            this.GroupHeader1.PageBreak = DevExpress.XtraReports.UI.PageBreak.BeforeBand;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataSource = typeof(SAGAS.Entidad.VistaActaManejo);
            // 
            // RptDetalleCuentas
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.PageHeader,
            this.GroupHeader1});
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
            this.Margins = new System.Drawing.Printing.Margins(16, 10, 22, 12);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.ShowPrintMarginsWarning = false;
            this.Version = "14.1";
            this.Watermark.Font = new System.Drawing.Font("Verdana", 66F);
            this.Watermark.ForeColor = System.Drawing.Color.LightGray;
            this.Watermark.ShowBehind = false;
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        public DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.PageFooterBand PageFooter;
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
        public DevExpress.XtraReports.UI.XRTable xrTable1;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow1;
        public DevExpress.XtraReports.UI.XRTableCell CeEmpresa;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow3;
        public DevExpress.XtraReports.UI.XRTableCell CeTitulo;
        public DevExpress.XtraReports.UI.XRPictureBox PicLogo;
        private DevExpress.XtraReports.UI.XRTableRow xrTableRow4;
        public DevExpress.XtraReports.UI.XRTableCell xrCeRango;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
    }
}
