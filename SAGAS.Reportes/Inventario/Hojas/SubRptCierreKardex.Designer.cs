namespace SAGAS.Reportes.Inventario.Hojas
{
    partial class SubRptCierreKardex
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
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrPivot = new DevExpress.XtraReports.UI.XRPivotGrid();
            this.fieldProductoCodigo1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldProductoNombre1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldCostoUnitario1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldCostoTotal1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldAreaNombre1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldClaseNombre1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldAlmacenNombre1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.fieldCantidadFinal1 = new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField();
            this.xrArea = new DevExpress.XtraReports.UI.XRLabel();
            this.cfMenosLitro = new DevExpress.XtraReports.UI.CalculatedField();
            this.cfEntradasGalones = new DevExpress.XtraReports.UI.CalculatedField();
            this.cfSalidaGalones = new DevExpress.XtraReports.UI.CalculatedField();
            this.cfNumRef = new DevExpress.XtraReports.UI.CalculatedField();
            this.cfAlmacen = new DevExpress.XtraReports.UI.CalculatedField();
            this.bdSource = new System.Windows.Forms.BindingSource();
            this.cfEstacion = new DevExpress.XtraReports.UI.CalculatedField();
            this.cfEstaciones = new DevExpress.XtraReports.UI.CalculatedField();
            this.cfAlmacenes = new DevExpress.XtraReports.UI.CalculatedField();
            this.cfEntradas = new DevExpress.XtraReports.UI.CalculatedField();
            this.cfSalidas = new DevExpress.XtraReports.UI.CalculatedField();
            this.cfCosto = new DevExpress.XtraReports.UI.CalculatedField();
            this.cfVEntrada = new DevExpress.XtraReports.UI.CalculatedField();
            this.cfVSalida = new DevExpress.XtraReports.UI.CalculatedField();
            ((System.ComponentModel.ISupportInitialize)(this.bdSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.HeightF = 0F;
            this.Detail.KeepTogether = true;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.SortFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("Fecha", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
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
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPivot,
            this.xrArea});
            this.GroupHeader1.HeightF = 98.95834F;
            this.GroupHeader1.Name = "GroupHeader1";
            this.GroupHeader1.RepeatEveryPage = true;
            // 
            // xrPivot
            // 
            this.xrPivot.Appearance.Cell.Font = new System.Drawing.Font("Tahoma", 8F);
            this.xrPivot.Appearance.FieldValue.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.xrPivot.Fields.AddRange(new DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField[] {
            this.fieldProductoCodigo1,
            this.fieldProductoNombre1,
            this.fieldCostoUnitario1,
            this.fieldCostoTotal1,
            this.fieldAreaNombre1,
            this.fieldClaseNombre1,
            this.fieldAlmacenNombre1,
            this.fieldCantidadFinal1});
            this.xrPivot.LocationFloat = new DevExpress.Utils.PointFloat(0F, 30.83334F);
            this.xrPivot.Name = "xrPivot";
            this.xrPivot.OptionsChartDataSource.ProvideColumnGrandTotals = true;
            this.xrPivot.OptionsChartDataSource.ProvideColumnTotals = true;
            this.xrPivot.OptionsChartDataSource.ProvideRowCustomTotals = true;
            this.xrPivot.OptionsChartDataSource.ProvideRowGrandTotals = true;
            this.xrPivot.OptionsChartDataSource.ProvideRowTotals = true;
            this.xrPivot.OptionsPrint.FilterSeparatorBarPadding = 3;
            this.xrPivot.OptionsView.ShowColumnHeaders = false;
            this.xrPivot.OptionsView.ShowCustomTotalsForSingleValues = true;
            this.xrPivot.OptionsView.ShowDataHeaders = false;
            this.xrPivot.OptionsView.ShowFilterHeaders = false;
            this.xrPivot.OptionsView.ShowGrandTotalsForSingleValues = true;
            this.xrPivot.SizeF = new System.Drawing.SizeF(1064F, 68.125F);
            this.xrPivot.CustomSummary += new System.EventHandler<DevExpress.XtraReports.UI.PivotGrid.PivotGridCustomSummaryEventArgs>(this.xrPivot_CustomSummary);
            // 
            // fieldProductoCodigo1
            // 
            this.fieldProductoCodigo1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldProductoCodigo1.AreaIndex = 0;
            this.fieldProductoCodigo1.Caption = "Código";
            this.fieldProductoCodigo1.FieldName = "ProductoCodigo";
            this.fieldProductoCodigo1.Name = "fieldProductoCodigo1";
            this.fieldProductoCodigo1.Width = 60;
            // 
            // fieldProductoNombre1
            // 
            this.fieldProductoNombre1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldProductoNombre1.AreaIndex = 1;
            this.fieldProductoNombre1.Caption = "Nombre del Producto";
            this.fieldProductoNombre1.FieldName = "ProductoNombre";
            this.fieldProductoNombre1.Name = "fieldProductoNombre1";
            this.fieldProductoNombre1.Width = 230;
            // 
            // fieldCostoUnitario1
            // 
            this.fieldCostoUnitario1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldCostoUnitario1.AreaIndex = 3;
            this.fieldCostoUnitario1.CellFormat.FormatString = "n2";
            this.fieldCostoUnitario1.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldCostoUnitario1.FieldName = "CostoUnitario";
            this.fieldCostoUnitario1.GrandTotalCellFormat.FormatString = "n2";
            this.fieldCostoUnitario1.GrandTotalCellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldCostoUnitario1.Name = "fieldCostoUnitario1";
            this.fieldCostoUnitario1.TotalCellFormat.FormatString = "n2";
            this.fieldCostoUnitario1.TotalCellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldCostoUnitario1.TotalValueFormat.FormatString = "n2";
            this.fieldCostoUnitario1.TotalValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldCostoUnitario1.ValueFormat.FormatString = "n2";
            this.fieldCostoUnitario1.ValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldCostoUnitario1.Width = 80;
            // 
            // fieldCostoTotal1
            // 
            this.fieldCostoTotal1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldCostoTotal1.AreaIndex = 1;
            this.fieldCostoTotal1.CellFormat.FormatString = "n2";
            this.fieldCostoTotal1.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldCostoTotal1.FieldName = "CostoTotal";
            this.fieldCostoTotal1.GrandTotalCellFormat.FormatString = "n2";
            this.fieldCostoTotal1.GrandTotalCellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldCostoTotal1.Name = "fieldCostoTotal1";
            this.fieldCostoTotal1.Options.ShowValues = false;
            this.fieldCostoTotal1.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Custom;
            this.fieldCostoTotal1.TotalCellFormat.FormatString = "n2";
            this.fieldCostoTotal1.TotalCellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldCostoTotal1.TotalValueFormat.FormatString = "n2";
            this.fieldCostoTotal1.TotalValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldCostoTotal1.ValueFormat.FormatString = "n2";
            this.fieldCostoTotal1.ValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            // 
            // fieldAreaNombre1
            // 
            this.fieldAreaNombre1.AreaIndex = 0;
            this.fieldAreaNombre1.FieldName = "AreaNombre";
            this.fieldAreaNombre1.Name = "fieldAreaNombre1";
            // 
            // fieldClaseNombre1
            // 
            this.fieldClaseNombre1.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.fieldClaseNombre1.AreaIndex = 2;
            this.fieldClaseNombre1.Caption = "Clase";
            this.fieldClaseNombre1.FieldName = "ClaseNombre";
            this.fieldClaseNombre1.Name = "fieldClaseNombre1";
            this.fieldClaseNombre1.SortMode = DevExpress.XtraPivotGrid.PivotSortMode.DisplayText;
            this.fieldClaseNombre1.Width = 170;
            // 
            // fieldAlmacenNombre1
            // 
            this.fieldAlmacenNombre1.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.fieldAlmacenNombre1.AreaIndex = 0;
            this.fieldAlmacenNombre1.FieldName = "AlmacenNombre";
            this.fieldAlmacenNombre1.Name = "fieldAlmacenNombre1";
            this.fieldAlmacenNombre1.TotalValueFormat.FormatString = "n2";
            this.fieldAlmacenNombre1.TotalValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldAlmacenNombre1.Width = 70;
            // 
            // fieldCantidadFinal1
            // 
            this.fieldCantidadFinal1.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.fieldCantidadFinal1.AreaIndex = 0;
            this.fieldCantidadFinal1.CellFormat.FormatString = "n2";
            this.fieldCantidadFinal1.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldCantidadFinal1.FieldName = "CantidadFinal";
            this.fieldCantidadFinal1.GrandTotalCellFormat.FormatString = "n2";
            this.fieldCantidadFinal1.GrandTotalCellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldCantidadFinal1.Name = "fieldCantidadFinal1";
            this.fieldCantidadFinal1.TotalCellFormat.FormatString = "n2";
            this.fieldCantidadFinal1.TotalCellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldCantidadFinal1.TotalValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.fieldCantidadFinal1.ValueFormat.FormatString = "n2";
            this.fieldCantidadFinal1.ValueFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            // 
            // xrArea
            // 
            this.xrArea.AutoWidth = true;
            this.xrArea.BackColor = System.Drawing.Color.LightCyan;
            this.xrArea.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.xrArea.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrArea.Name = "xrArea";
            this.xrArea.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrArea.SizeF = new System.Drawing.SizeF(81.24997F, 25F);
            this.xrArea.StylePriority.UseBackColor = false;
            this.xrArea.StylePriority.UseFont = false;
            this.xrArea.StylePriority.UseTextAlignment = false;
            this.xrArea.Text = "<>";
            this.xrArea.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrArea.WordWrap = false;
            // 
            // cfMenosLitro
            // 
            this.cfMenosLitro.Expression = "[CantidadEntrada] - [CantidadSalida]";
            this.cfMenosLitro.Name = "cfMenosLitro";
            // 
            // cfEntradasGalones
            // 
            this.cfEntradasGalones.Expression = "Round([CantidadEntrada] / 3.7854, 3)";
            this.cfEntradasGalones.Name = "cfEntradasGalones";
            // 
            // cfSalidaGalones
            // 
            this.cfSalidaGalones.Expression = "Round([CantidadSalida] / 3.7854, 3)";
            this.cfSalidaGalones.Name = "cfSalidaGalones";
            // 
            // cfNumRef
            // 
            this.cfNumRef.Expression = "Iif([Numero] == 0, [Referencia] ,[Numero] )";
            this.cfNumRef.Name = "cfNumRef";
            // 
            // cfAlmacen
            // 
            this.cfAlmacen.DataMember = "VistaKardexes";
            this.cfAlmacen.Expression = "Iif(IsNullOrEmpty([AlmacenEntradaNombre]) ,[AlmacenSalidaNombre]  ,[AlmacenEntrad" +
    "aNombre] )";
            this.cfAlmacen.Name = "cfAlmacen";
            // 
            // bdSource
            // 
            this.bdSource.DataSource = typeof(SAGAS.Entidad.VistaKardex);
            // 
            // cfEstacion
            // 
            this.cfEstacion.DataMember = "VistaKardexes";
            this.cfEstacion.Expression = "Iif(IsNullOrEmpty([SubEstacionNombre]), [EstacionNombre], Concat([EstacionNombre]" +
    ", \' / \', [SubEstacionNombre]))";
            this.cfEstacion.Name = "cfEstacion";
            // 
            // cfEstaciones
            // 
            this.cfEstaciones.Expression = "Iif(IsNullOrEmpty([SubEstacionNombre]),[EstacionNombre]  , Concat([EstacionNombre" +
    "], \' / \' ,[SubEstacionNombre]) )";
            this.cfEstaciones.Name = "cfEstaciones";
            // 
            // cfAlmacenes
            // 
            this.cfAlmacenes.Expression = "Iif(IsNullOrEmpty([AlmacenEntradaNombre]), [AlmacenSalidaNombre] ,[AlmacenEntrada" +
    "Nombre] )";
            this.cfAlmacenes.Name = "cfAlmacenes";
            // 
            // cfEntradas
            // 
            this.cfEntradas.Expression = "Iif([CantidadEntrada]  ==  0, \'\', [CantidadEntrada])";
            this.cfEntradas.Name = "cfEntradas";
            // 
            // cfSalidas
            // 
            this.cfSalidas.Expression = "Iif([CantidadSalida]  == 0, \'\', [CantidadSalida])";
            this.cfSalidas.Name = "cfSalidas";
            // 
            // cfCosto
            // 
            this.cfCosto.Expression = "Iif([CantidadEntrada] > 0,[CostoEntrada] , [CostoSalida])";
            this.cfCosto.Name = "cfCosto";
            // 
            // cfVEntrada
            // 
            this.cfVEntrada.Expression = "Iif([CantidadEntrada]  == 0, \'\', [CostoTotal])";
            this.cfVEntrada.Name = "cfVEntrada";
            // 
            // cfVSalida
            // 
            this.cfVSalida.Expression = "Iif([CantidadSalida]  == 0, \'\', [CostoTotal])";
            this.cfVSalida.Name = "cfVSalida";
            // 
            // SubRptCierreKardex
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.GroupHeader1});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.cfMenosLitro,
            this.cfEntradasGalones,
            this.cfSalidaGalones,
            this.cfNumRef,
            this.cfAlmacen,
            this.cfEstacion,
            this.cfEstaciones,
            this.cfAlmacenes,
            this.cfEntradas,
            this.cfSalidas,
            this.cfCosto,
            this.cfVEntrada,
            this.cfVSalida});
            this.DataSource = this.bdSource;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(11, 11, 0, 0);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.ShowPrintMarginsWarning = false;
            this.Version = "14.1";
            this.Watermark.Font = new System.Drawing.Font("Verdana", 66F);
            this.Watermark.ForeColor = System.Drawing.Color.LightGray;
            this.Watermark.ImageViewMode = DevExpress.XtraPrinting.Drawing.ImageViewMode.Zoom;
            ((System.ComponentModel.ISupportInitialize)(this.bdSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        public DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.GroupHeaderBand GroupHeader1;
        private System.Windows.Forms.BindingSource bdSource;
        private DevExpress.XtraReports.UI.CalculatedField cfMenosLitro;
        private DevExpress.XtraReports.UI.CalculatedField cfEntradasGalones;
        private DevExpress.XtraReports.UI.CalculatedField cfSalidaGalones;
        private DevExpress.XtraReports.UI.CalculatedField cfNumRef;
        private DevExpress.XtraReports.UI.CalculatedField cfAlmacen;
        private DevExpress.XtraReports.UI.CalculatedField cfEstacion;
        private DevExpress.XtraReports.UI.CalculatedField cfEstaciones;
        private DevExpress.XtraReports.UI.CalculatedField cfAlmacenes;
        private DevExpress.XtraReports.UI.CalculatedField cfEntradas;
        private DevExpress.XtraReports.UI.CalculatedField cfSalidas;
        private DevExpress.XtraReports.UI.CalculatedField cfCosto;
        private DevExpress.XtraReports.UI.CalculatedField cfVEntrada;
        private DevExpress.XtraReports.UI.CalculatedField cfVSalida;
        private DevExpress.XtraReports.UI.XRPivotGrid xrPivot;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldProductoCodigo1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldProductoNombre1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldCostoUnitario1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldCostoTotal1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldAreaNombre1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldClaseNombre1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldAlmacenNombre1;
        private DevExpress.XtraReports.UI.PivotGrid.XRPivotGridField fieldCantidadFinal1;
        public DevExpress.XtraReports.UI.XRLabel xrArea;
    }
}
