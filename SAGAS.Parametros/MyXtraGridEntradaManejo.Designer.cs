namespace SAGAS.Parametros
{
    partial class MyXtraGridEntradaManejo
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.gridDetalle = new DevExpress.XtraGrid.GridControl();
            this.gvDetalle = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colProduct = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colUnit = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColAlmacenEntrada = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colQuantity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.speCantidad = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboImpuestos = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.speCost = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.cboUnidadMedida = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridProductos = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.gridViewProductos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.cboAlmacenEntrada = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.spSubTotal = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.spMontoISC = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.spCostUnit = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.cboAlmacenSalida = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboImpuestos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenEntrada)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSubTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMontoISC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spCostUnit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenSalida)).BeginInit();
            this.SuspendLayout();
            // 
            // gridDetalle
            // 
            this.gridDetalle.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetalle.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridDetalle.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gridDetalle.EmbeddedNavigator.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.gridDetalle_EmbeddedNavigator_ButtonClick);
            this.gridDetalle.Location = new System.Drawing.Point(0, 0);
            this.gridDetalle.MainView = this.gvDetalle;
            this.gridDetalle.Name = "gridDetalle";
            this.gridDetalle.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.cboImpuestos,
            this.speCantidad,
            this.speCost,
            this.cboUnidadMedida,
            this.gridProductos,
            this.cboAlmacenEntrada,
            this.spSubTotal,
            this.spMontoISC,
            this.spCostUnit,
            this.cboAlmacenSalida});
            this.gridDetalle.Size = new System.Drawing.Size(739, 466);
            this.gridDetalle.TabIndex = 9;
            this.gridDetalle.UseEmbeddedNavigator = true;
            this.gridDetalle.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetalle});
            // 
            // gvDetalle
            // 
            this.gvDetalle.ActiveFilterEnabled = false;
            this.gvDetalle.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gvDetalle.Appearance.FooterPanel.Options.UseFont = true;
            this.gvDetalle.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colProduct,
            this.colUnit,
            this.ColAlmacenEntrada,
            this.colQuantity,
            this.colID,
            this.gridColumn8});
            this.gvDetalle.GridControl = this.gridDetalle;
            this.gvDetalle.Name = "gvDetalle";
            this.gvDetalle.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.gvDetalle.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
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
            this.gvDetalle.OptionsNavigation.AutoFocusNewRow = true;
            this.gvDetalle.OptionsView.ShowFooter = true;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            // 
            // colProduct
            // 
            this.colProduct.Caption = "Producto";
            this.colProduct.FieldName = "Producto";
            this.colProduct.Name = "colProduct";
            this.colProduct.Visible = true;
            this.colProduct.VisibleIndex = 0;
            this.colProduct.Width = 268;
            // 
            // colUnit
            // 
            this.colUnit.Caption = "Unidad de Medida";
            this.colUnit.FieldName = "UnidadMedidaNombre";
            this.colUnit.Name = "colUnit";
            this.colUnit.OptionsColumn.ReadOnly = true;
            this.colUnit.Visible = true;
            this.colUnit.VisibleIndex = 1;
            this.colUnit.Width = 176;
            // 
            // ColAlmacenEntrada
            // 
            this.ColAlmacenEntrada.Caption = "Tanque";
            this.ColAlmacenEntrada.FieldName = "Almacen";
            this.ColAlmacenEntrada.Name = "ColAlmacenEntrada";
            this.ColAlmacenEntrada.Visible = true;
            this.ColAlmacenEntrada.VisibleIndex = 2;
            this.ColAlmacenEntrada.Width = 183;
            // 
            // colQuantity
            // 
            this.colQuantity.AppearanceHeader.Options.UseTextOptions = true;
            this.colQuantity.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colQuantity.Caption = "Litros";
            this.colQuantity.ColumnEdit = this.speCantidad;
            this.colQuantity.DisplayFormat.FormatString = "n3";
            this.colQuantity.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colQuantity.FieldName = "Cantidad";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Cantidad", "{0:N3}")});
            this.colQuantity.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colQuantity.Visible = true;
            this.colQuantity.VisibleIndex = 3;
            this.colQuantity.Width = 113;
            // 
            // speCantidad
            // 
            this.speCantidad.AutoHeight = false;
            this.speCantidad.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speCantidad.DisplayFormat.FormatString = "n3";
            this.speCantidad.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speCantidad.EditFormat.FormatString = "n3";
            this.speCantidad.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.speCantidad.MaxValue = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.speCantidad.Name = "speCantidad";
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // gridColumn8
            // 
            this.gridColumn8.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.gridColumn8.Caption = "Galones";
            this.gridColumn8.DisplayFormat.FormatString = "N2";
            this.gridColumn8.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn8.FieldName = "Total";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Total", "{0:N2}")});
            this.gridColumn8.UnboundExpression = "Round([Cantidad]  / 3.785, 2)";
            this.gridColumn8.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 4;
            this.gridColumn8.Width = 121;
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
            this.gridProductos.DisplayMember = "Display";
            this.gridProductos.Name = "gridProductos";
            this.gridProductos.NullText = "";
            this.gridProductos.ValueMember = "ID";
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
            // cboAlmacenEntrada
            // 
            this.cboAlmacenEntrada.AutoHeight = false;
            this.cboAlmacenEntrada.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboAlmacenEntrada.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Almacen / Tanque")});
            this.cboAlmacenEntrada.DisplayMember = "Display";
            this.cboAlmacenEntrada.Name = "cboAlmacenEntrada";
            this.cboAlmacenEntrada.NullText = "";
            this.cboAlmacenEntrada.ValueMember = "ID";
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
            // spCostUnit
            // 
            this.spCostUnit.AllowMouseWheel = false;
            this.spCostUnit.AutoHeight = false;
            this.spCostUnit.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.spCostUnit.DisplayFormat.FormatString = "n4";
            this.spCostUnit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spCostUnit.EditFormat.FormatString = "n4";
            this.spCostUnit.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spCostUnit.Name = "spCostUnit";
            // 
            // cboAlmacenSalida
            // 
            this.cboAlmacenSalida.AutoHeight = false;
            this.cboAlmacenSalida.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboAlmacenSalida.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Almacen")});
            this.cboAlmacenSalida.DisplayMember = "Display";
            this.cboAlmacenSalida.Name = "cboAlmacenSalida";
            this.cboAlmacenSalida.NullText = "";
            this.cboAlmacenSalida.ValueMember = "ID";
            // 
            // MyXtraGridEntradaManejo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridDetalle);
            this.Name = "MyXtraGridEntradaManejo";
            this.Size = new System.Drawing.Size(739, 466);
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboImpuestos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenEntrada)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSubTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMontoISC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spCostUnit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacenSalida)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public DevExpress.XtraGrid.GridControl gridDetalle;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDetalle;
        private DevExpress.XtraGrid.Columns.GridColumn colProduct;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit gridProductos;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewProductos;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn colUnit;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboUnidadMedida;
        private DevExpress.XtraGrid.Columns.GridColumn ColAlmacenEntrada;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboAlmacenEntrada;
        private DevExpress.XtraGrid.Columns.GridColumn colQuantity;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit speCantidad;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboImpuestos;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit speCost;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spSubTotal;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spMontoISC;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spCostUnit;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboAlmacenSalida;

    }
}
