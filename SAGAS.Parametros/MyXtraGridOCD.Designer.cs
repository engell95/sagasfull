namespace SAGAS.Parametros
{
    partial class MyXtraGridOCD
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gridDetalle = new DevExpress.XtraGrid.GridControl();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colProduct = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColAlmacen = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.speCantidad = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colCostoInical = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCostoTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.speCost = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colTaxValue = new DevExpress.XtraGrid.Columns.GridColumn();
            this.spTax = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colCost = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIDOCD = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboImpuestos = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.cboUnidadMedida = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridProductos = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.gridViewProductos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboAlmacen = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.spSubTotal = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.spMontoISC = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboImpuestos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSubTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMontoISC)).BeginInit();
            this.SuspendLayout();
            // 
            // gridDetalle
            // 
            this.gridDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetalle.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridDetalle.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gridDetalle.Location = new System.Drawing.Point(0, 0);
            this.gridDetalle.MainView = this.gvDetalle;
            this.gridDetalle.Name = "gridDetalle";
            this.gridDetalle.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cboImpuestos,
            this.speCantidad,
            this.speCost,
            this.cboUnidadMedida,
            this.gridProductos,
            this.cboAlmacen,
            this.spSubTotal,
            this.spMontoISC,
            this.spTax});
            this.gridDetalle.Size = new System.Drawing.Size(739, 466);
            this.gridDetalle.TabIndex = 9;
            this.gridDetalle.UseEmbeddedNavigator = true;
            this.gridDetalle.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetalle});
            // 
            // gvDetalle
            // 
            this.gvDetalle.ActiveFilterEnabled = false;
            this.gvDetalle.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colProduct,
            this.colUnit,
            this.ColAlmacen,
            this.colQuantity,
            this.colCostoInical,
            this.colCostoTotal,
            this.colTaxValue,
            this.colCost,
            this.colTotal,
            this.colIDOCD,
            this.gridColumn1,
            this.gridColumn2});
            this.gvDetalle.GridControl = this.gridDetalle;
            this.gvDetalle.Name = "gvDetalle";
            this.gvDetalle.OptionsBehavior.Editable = false;
            this.gvDetalle.OptionsCustomization.AllowColumnMoving = false;
            this.gvDetalle.OptionsCustomization.AllowFilter = false;
            this.gvDetalle.OptionsCustomization.AllowGroup = false;
            this.gvDetalle.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvDetalle.OptionsCustomization.AllowSort = false;
            this.gvDetalle.OptionsFilter.AllowFilterEditor = false;
            this.gvDetalle.OptionsFilter.AllowMRUFilterList = false;
            this.gvDetalle.OptionsLayout.Columns.AddNewColumns = false;
            this.gvDetalle.OptionsLayout.Columns.RemoveOldColumns = false;
            this.gvDetalle.OptionsSelection.MultiSelect = true;
            this.gvDetalle.OptionsView.ColumnAutoWidth = false;
            this.gvDetalle.OptionsView.ShowFooter = true;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            // 
            // colProduct
            // 
            this.colProduct.Caption = "Producto";
            this.colProduct.FieldName = "Producto";
            this.colProduct.Name = "colProduct";
            this.colProduct.OptionsColumn.AllowEdit = false;
            this.colProduct.OptionsColumn.AllowFocus = false;
            this.colProduct.UnboundExpression = "[ProductoCodigo] + \' | \' + [ProductoNombre]";
            this.colProduct.UnboundType = DevExpress.Data.UnboundColumnType.String;
            this.colProduct.Visible = true;
            this.colProduct.VisibleIndex = 0;
            this.colProduct.Width = 209;
            // 
            // colUnit
            // 
            this.colUnit.Caption = "Unidad de Medida";
            this.colUnit.FieldName = "UnidadMedidaNombre";
            this.colUnit.Name = "colUnit";
            this.colUnit.OptionsColumn.AllowEdit = false;
            this.colUnit.OptionsColumn.AllowFocus = false;
            this.colUnit.OptionsColumn.ReadOnly = true;
            this.colUnit.Visible = true;
            this.colUnit.VisibleIndex = 1;
            this.colUnit.Width = 117;
            // 
            // ColAlmacen
            // 
            this.ColAlmacen.Caption = "Almacen";
            this.ColAlmacen.FieldName = "AlmacenEntradaNombre";
            this.ColAlmacen.Name = "ColAlmacen";
            this.ColAlmacen.OptionsColumn.AllowEdit = false;
            this.ColAlmacen.OptionsColumn.AllowFocus = false;
            this.ColAlmacen.OptionsColumn.ReadOnly = true;
            this.ColAlmacen.Visible = true;
            this.ColAlmacen.VisibleIndex = 2;
            this.ColAlmacen.Width = 123;
            // 
            // colQuantity
            // 
            this.colQuantity.Caption = "Cantidad";
            this.colQuantity.ColumnEdit = this.speCantidad;
            this.colQuantity.DisplayFormat.FormatString = "n2";
            this.colQuantity.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colQuantity.FieldName = "CantidadEntrada";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.OptionsColumn.AllowEdit = false;
            this.colQuantity.OptionsColumn.AllowFocus = false;
            this.colQuantity.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colQuantity.Visible = true;
            this.colQuantity.VisibleIndex = 3;
            this.colQuantity.Width = 69;
            // 
            // speCantidad
            // 
            this.speCantidad.AutoHeight = false;
            this.speCantidad.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speCantidad.DisplayFormat.FormatString = "n2";
            this.speCantidad.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speCantidad.EditFormat.FormatString = "n2";
            this.speCantidad.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speCantidad.MaxValue = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.speCantidad.Name = "speCantidad";
            // 
            // colCostoInical
            // 
            this.colCostoInical.Caption = "Ult. Cto.";
            this.colCostoInical.DisplayFormat.FormatString = "n4";
            this.colCostoInical.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCostoInical.FieldName = "UltimoCosto";
            this.colCostoInical.Name = "colCostoInical";
            this.colCostoInical.OptionsColumn.AllowEdit = false;
            this.colCostoInical.OptionsColumn.AllowFocus = false;
            this.colCostoInical.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colCostoInical.Visible = true;
            this.colCostoInical.VisibleIndex = 4;
            this.colCostoInical.Width = 60;
            // 
            // colCostoTotal
            // 
            this.colCostoTotal.Caption = "Costo Total";
            this.colCostoTotal.ColumnEdit = this.speCost;
            this.colCostoTotal.DisplayFormat.FormatString = "n2";
            this.colCostoTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCostoTotal.FieldName = "CostoTotal";
            this.colCostoTotal.Name = "colCostoTotal";
            this.colCostoTotal.OptionsColumn.AllowEdit = false;
            this.colCostoTotal.OptionsColumn.AllowFocus = false;
            this.colCostoTotal.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "CostoTotal", "{0:#,0.00;(#,0.00)}")});
            this.colCostoTotal.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colCostoTotal.Visible = true;
            this.colCostoTotal.VisibleIndex = 6;
            this.colCostoTotal.Width = 87;
            // 
            // speCost
            // 
            this.speCost.AutoHeight = false;
            this.speCost.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speCost.DisplayFormat.FormatString = "2";
            this.speCost.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speCost.EditFormat.FormatString = "2";
            this.speCost.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speCost.MaxValue = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.speCost.Name = "speCost";
            // 
            // colTaxValue
            // 
            this.colTaxValue.Caption = "IVA";
            this.colTaxValue.ColumnEdit = this.spTax;
            this.colTaxValue.DisplayFormat.FormatString = "n2";
            this.colTaxValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTaxValue.FieldName = "ImpuestoTotal";
            this.colTaxValue.Name = "colTaxValue";
            this.colTaxValue.OptionsColumn.AllowEdit = false;
            this.colTaxValue.OptionsColumn.AllowFocus = false;
            this.colTaxValue.OptionsColumn.ReadOnly = true;
            this.colTaxValue.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "ImpuestoTotal", "{0:#,0.00;(#,0.00)}")});
            this.colTaxValue.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colTaxValue.Visible = true;
            this.colTaxValue.VisibleIndex = 7;
            this.colTaxValue.Width = 70;
            // 
            // spTax
            // 
            this.spTax.AutoHeight = false;
            this.spTax.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spTax.DisplayFormat.FormatString = "n2";
            this.spTax.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spTax.EditFormat.FormatString = "n2";
            this.spTax.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spTax.Name = "spTax";
            // 
            // colCost
            // 
            this.colCost.Caption = "Costo Unitario";
            this.colCost.DisplayFormat.FormatString = "n4";
            this.colCost.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCost.FieldName = "Costo";
            this.colCost.Name = "colCost";
            this.colCost.OptionsColumn.AllowEdit = false;
            this.colCost.OptionsColumn.AllowFocus = false;
            this.colCost.OptionsColumn.ReadOnly = true;
            this.colCost.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colCost.Visible = true;
            this.colCost.VisibleIndex = 5;
            this.colCost.Width = 86;
            // 
            // colTotal
            // 
            this.colTotal.Caption = "TOTAL";
            this.colTotal.DisplayFormat.FormatString = "n2";
            this.colTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTotal.FieldName = "Total";
            this.colTotal.Name = "colTotal";
            this.colTotal.OptionsColumn.AllowEdit = false;
            this.colTotal.OptionsColumn.AllowFocus = false;
            this.colTotal.OptionsColumn.ReadOnly = true;
            this.colTotal.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Total", "{0:#,0.00;(#,0.00)}")});
            this.colTotal.UnboundExpression = "CostoTotal + ImpuestoTotal";
            this.colTotal.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colTotal.Visible = true;
            this.colTotal.VisibleIndex = 8;
            this.colTotal.Width = 108;
            // 
            // colIDOCD
            // 
            this.colIDOCD.FieldName = "ID";
            this.colIDOCD.Name = "colIDOCD";
            // 
            // gridColumn1
            // 
            this.gridColumn1.FieldName = "ProductoCodigo";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.FieldName = "ProductoNombre";
            this.gridColumn2.Name = "gridColumn2";
            // 
            // cboImpuestos
            // 
            this.cboImpuestos.AutoHeight = false;
            this.cboImpuestos.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboImpuestos.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Name"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Value", "Value")});
            this.cboImpuestos.Name = "cboImpuestos";
            this.cboImpuestos.NullText = "";
            this.cboImpuestos.ShowHeader = false;
            // 
            // cboUnidadMedida
            // 
            this.cboUnidadMedida.AutoHeight = false;
            this.cboUnidadMedida.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboUnidadMedida.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Medidas")});
            this.cboUnidadMedida.Name = "cboUnidadMedida";
            this.cboUnidadMedida.NullText = "";
            // 
            // gridProductos
            // 
            this.gridProductos.AutoHeight = false;
            this.gridProductos.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.gridProductos.Name = "gridProductos";
            this.gridProductos.NullText = "";
            this.gridProductos.View = this.gridViewProductos;
            // 
            // gridViewProductos
            // 
            this.gridViewProductos.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7});
            this.gridViewProductos.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewProductos.Name = "gridViewProductos";
            this.gridViewProductos.OptionsBehavior.Editable = false;
            this.gridViewProductos.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewProductos.OptionsView.ShowAutoFilterRow = true;
            this.gridViewProductos.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn4
            // 
            this.gridColumn4.FieldName = "ID";
            this.gridColumn4.Name = "gridColumn4";
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Código";
            this.gridColumn5.FieldName = "Codigo";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 0;
            this.gridColumn5.Width = 206;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Producto";
            this.gridColumn6.FieldName = "Nombre";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 1;
            this.gridColumn6.Width = 434;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Und.  Medida";
            this.gridColumn7.FieldName = "UmidadName";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 2;
            this.gridColumn7.Width = 176;
            // 
            // cboAlmacen
            // 
            this.cboAlmacen.AutoHeight = false;
            this.cboAlmacen.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboAlmacen.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Almacen / Tanque")});
            this.cboAlmacen.Name = "cboAlmacen";
            this.cboAlmacen.NullText = "";
            // 
            // spSubTotal
            // 
            this.spSubTotal.AutoHeight = false;
            this.spSubTotal.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spSubTotal.Name = "spSubTotal";
            // 
            // spMontoISC
            // 
            this.spMontoISC.AutoHeight = false;
            this.spMontoISC.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spMontoISC.Name = "spMontoISC";
            // 
            // MyXtraGridOCD
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridDetalle);
            this.Name = "MyXtraGridOCD";
            this.Size = new System.Drawing.Size(739, 466);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboImpuestos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSubTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMontoISC)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraGrid.GridControl gridDetalle;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewProductos;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn colQuantity;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit speCantidad;
        private DevExpress.XtraGrid.Columns.GridColumn colCostoInical;
        private DevExpress.XtraGrid.Columns.GridColumn colCostoTotal;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit speCost;
        private DevExpress.XtraGrid.Columns.GridColumn colTaxValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spTax;
        private DevExpress.XtraGrid.Columns.GridColumn colCost;
        private DevExpress.XtraGrid.Columns.GridColumn colTotal;
        private DevExpress.XtraGrid.Columns.GridColumn colIDOCD;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboImpuestos;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spSubTotal;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spMontoISC;
        public DevExpress.XtraGrid.Columns.GridColumn colProduct;
        public DevExpress.XtraGrid.Columns.GridColumn colUnit;
        public DevExpress.XtraGrid.Columns.GridColumn ColAlmacen;
        public DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit gridProductos;
        public DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboUnidadMedida;
        public DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboAlmacen;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
    }
}
