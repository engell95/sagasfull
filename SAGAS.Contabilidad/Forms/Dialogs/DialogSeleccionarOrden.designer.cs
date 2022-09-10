namespace SAGAS.Contabilidad.Forms.Dialogs
{
    partial class DialogSeleccionarOrden
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DialogSeleccionarOrden));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.splitCentral = new DevExpress.XtraEditors.SplitContainerControl();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNumero = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFecha = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEstacionID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpEstacionServicio = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpIslas = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpTecnico = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpEstado = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.lkSES = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.lkEstado = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.lkMoneda = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.lkMes = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemDateEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.rpChkAprobado = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridDetalle = new DevExpress.XtraGrid.GridControl();
            this.gvDetalle = new DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView();
            this.colProduct = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colUnit = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colGlnPendiente = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.ColAlmacen = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colCostoTotal = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colQuantity = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colCost = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colCostoSubTotal = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.calcSubTotal = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.colISC = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.spMontoISC = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colTaxValue = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.spTax = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.colTotal = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colDetalle = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colExentoIVA = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colEsISC = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colPrecio = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.cboImpuestos = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.speCantidad = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.speCost = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.cboUnidadMedida = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridProductos = new DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit();
            this.gridViewProductos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.cboAlmacen = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.spSubTotal = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.calcCantidad = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.calcCost = new DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.errRequiredField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider();
            this.bandedGridColumn1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitCentral)).BeginInit();
            this.splitCentral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstacionServicio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpIslas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTecnico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkSES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkEstado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkMoneda)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkMes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpChkAprobado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.calcSubTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMontoISC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboImpuestos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedida)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSubTotal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.calcCantidad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.calcCost)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.splitCentral);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(785, 329);
            this.panelControl1.TabIndex = 0;
            // 
            // splitCentral
            // 
            this.splitCentral.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitCentral.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
            this.splitCentral.Location = new System.Drawing.Point(2, 2);
            this.splitCentral.Name = "splitCentral";
            this.splitCentral.Panel1.Controls.Add(this.grid);
            this.splitCentral.Panel1.Text = "Panel1";
            this.splitCentral.Panel2.Controls.Add(this.gridDetalle);
            this.splitCentral.Panel2.Text = "Panel2";
            this.splitCentral.Size = new System.Drawing.Size(781, 325);
            this.splitCentral.SplitterPosition = 367;
            this.splitCentral.TabIndex = 7;
            this.splitCentral.Text = "splitContainerControl1";
            // 
            // grid
            // 
            this.grid.Cursor = System.Windows.Forms.Cursors.Default;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.EnabledAutoRepeat = false;
            this.grid.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grid.EmbeddedNavigator.TextStringFormat = " {0} de {1}";
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.MainView = this.gvData;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rpEstacionServicio,
            this.rpIslas,
            this.rpTecnico,
            this.rpEstado,
            this.lkSES,
            this.lkEstado,
            this.lkMoneda,
            this.lkMes,
            this.repositoryItemDateEdit1,
            this.rpChkAprobado});
            this.grid.Size = new System.Drawing.Size(367, 325);
            this.grid.TabIndex = 5;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colNumero,
            this.colFecha,
            this.colEstacionID,
            this.gridColumn5,
            this.gridColumn1,
            this.gridColumn2});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsDetail.AllowZoomDetail = false;
            this.gvData.OptionsDetail.EnableMasterViewMode = false;
            this.gvData.OptionsDetail.ShowDetailTabs = false;
            this.gvData.OptionsDetail.SmartDetailExpand = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.OptionsView.ShowGroupPanel = false;
            this.gvData.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvData_FocusedRowChanged);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colNumero
            // 
            this.colNumero.Caption = "Número";
            this.colNumero.FieldName = "Numero";
            this.colNumero.Name = "colNumero";
            this.colNumero.OptionsColumn.AllowEdit = false;
            this.colNumero.OptionsColumn.AllowFocus = false;
            this.colNumero.Visible = true;
            this.colNumero.VisibleIndex = 2;
            this.colNumero.Width = 62;
            // 
            // colFecha
            // 
            this.colFecha.Caption = "Fecha Pedido";
            this.colFecha.FieldName = "FechaCreado";
            this.colFecha.Name = "colFecha";
            this.colFecha.OptionsColumn.AllowEdit = false;
            this.colFecha.OptionsColumn.AllowFocus = false;
            this.colFecha.Visible = true;
            this.colFecha.VisibleIndex = 1;
            this.colFecha.Width = 72;
            // 
            // colEstacionID
            // 
            this.colEstacionID.Caption = "gridColumn1";
            this.colEstacionID.FieldName = "EstacionServicioID";
            this.colEstacionID.Name = "colEstacionID";
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Estación Servicio";
            this.gridColumn5.FieldName = "Estacion";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsColumn.AllowFocus = false;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 0;
            this.gridColumn5.Width = 142;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Comentario";
            this.gridColumn1.FieldName = "Comentario";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowFocus = false;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 4;
            this.gridColumn1.Width = 162;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "IECC";
            this.gridColumn2.FieldName = "EsIECC";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 3;
            this.gridColumn2.Width = 38;
            // 
            // rpEstacionServicio
            // 
            this.rpEstacionServicio.AutoHeight = false;
            this.rpEstacionServicio.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpEstacionServicio.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Estaciones de Servicio")});
            this.rpEstacionServicio.Name = "rpEstacionServicio";
            // 
            // rpIslas
            // 
            this.rpIslas.AutoHeight = false;
            this.rpIslas.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpIslas.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Islas")});
            this.rpIslas.Name = "rpIslas";
            // 
            // rpTecnico
            // 
            this.rpTecnico.AutoHeight = false;
            this.rpTecnico.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpTecnico.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Técnicos")});
            this.rpTecnico.Name = "rpTecnico";
            // 
            // rpEstado
            // 
            this.rpEstado.AutoHeight = false;
            this.rpEstado.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpEstado.Name = "rpEstado";
            // 
            // lkSES
            // 
            this.lkSES.AutoHeight = false;
            this.lkSES.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkSES.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Sub Estación")});
            this.lkSES.Name = "lkSES";
            // 
            // lkEstado
            // 
            this.lkEstado.AutoHeight = false;
            this.lkEstado.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkEstado.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Estados")});
            this.lkEstado.DisplayMember = "Name";
            this.lkEstado.Name = "lkEstado";
            this.lkEstado.NullText = "<N/A>";
            this.lkEstado.ValueMember = "ID";
            // 
            // lkMoneda
            // 
            this.lkMoneda.AutoHeight = false;
            this.lkMoneda.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkMoneda.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Monedas")});
            this.lkMoneda.DisplayMember = "Display";
            this.lkMoneda.Name = "lkMoneda";
            this.lkMoneda.ValueMember = "ID";
            // 
            // lkMes
            // 
            this.lkMes.AutoHeight = false;
            this.lkMes.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkMes.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Name", "Mes")});
            this.lkMes.DisplayMember = "Name";
            this.lkMes.Name = "lkMes";
            this.lkMes.ValueMember = "ID";
            // 
            // repositoryItemDateEdit1
            // 
            this.repositoryItemDateEdit1.AutoHeight = false;
            this.repositoryItemDateEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemDateEdit1.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemDateEdit1.Name = "repositoryItemDateEdit1";
            // 
            // rpChkAprobado
            // 
            this.rpChkAprobado.AutoHeight = false;
            this.rpChkAprobado.Name = "rpChkAprobado";
            // 
            // gridDetalle
            // 
            this.gridDetalle.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridDetalle.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gridDetalle.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridDetalle.EmbeddedNavigator.TextStringFormat = "{0} of {1}";
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
            this.spTax,
            this.calcCantidad,
            this.calcCost,
            this.calcSubTotal});
            this.gridDetalle.Size = new System.Drawing.Size(410, 325);
            this.gridDetalle.TabIndex = 10;
            this.gridDetalle.UseEmbeddedNavigator = true;
            this.gridDetalle.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDetalle});
            // 
            // gvDetalle
            // 
            this.gvDetalle.ActiveFilterEnabled = false;
            this.gvDetalle.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1});
            this.gvDetalle.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colProduct,
            this.colUnit,
            this.ColAlmacen,
            this.colQuantity,
            this.colCostoTotal,
            this.colTaxValue,
            this.colCost,
            this.colTotal,
            this.colISC,
            this.colCostoSubTotal,
            this.colDetalle,
            this.colExentoIVA,
            this.colEsISC,
            this.colPrecio,
            this.colGlnPendiente,
            this.bandedGridColumn1});
            this.gvDetalle.GridControl = this.gridDetalle;
            this.gvDetalle.Name = "gvDetalle";
            this.gvDetalle.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvDetalle.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gvDetalle.OptionsBehavior.Editable = false;
            this.gvDetalle.OptionsCustomization.AllowBandMoving = false;
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
            this.gvDetalle.OptionsView.ShowBands = false;
            this.gvDetalle.OptionsView.ShowFooter = true;
            this.gvDetalle.OptionsView.ShowGroupPanel = false;
            this.gvDetalle.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colProduct, DevExpress.Data.ColumnSortOrder.Descending)});
            // 
            // colProduct
            // 
            this.colProduct.Caption = "Producto";
            this.colProduct.FieldName = "Producto";
            this.colProduct.Name = "colProduct";
            this.colProduct.Visible = true;
            this.colProduct.Width = 83;
            // 
            // colUnit
            // 
            this.colUnit.Caption = "Gln Pedido";
            this.colUnit.DisplayFormat.FormatString = "N2";
            this.colUnit.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colUnit.FieldName = "Galones";
            this.colUnit.Name = "colUnit";
            this.colUnit.OptionsColumn.ReadOnly = true;
            this.colUnit.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Galones", "{0:N2}")});
            this.colUnit.Visible = true;
            this.colUnit.Width = 76;
            // 
            // colGlnPendiente
            // 
            this.colGlnPendiente.Caption = "Gln Pendiente";
            this.colGlnPendiente.DisplayFormat.FormatString = "N2";
            this.colGlnPendiente.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colGlnPendiente.FieldName = "GlnPendiente";
            this.colGlnPendiente.Name = "colGlnPendiente";
            this.colGlnPendiente.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "GlnPendiente", "{0:N2}")});
            this.colGlnPendiente.UnboundExpression = "Round([Litros] / 3.785, 2)";
            this.colGlnPendiente.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colGlnPendiente.Visible = true;
            this.colGlnPendiente.Width = 79;
            // 
            // ColAlmacen
            // 
            this.ColAlmacen.Caption = "Ltr Pendiente";
            this.ColAlmacen.DisplayFormat.FormatString = "N2";
            this.ColAlmacen.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.ColAlmacen.FieldName = "Litros";
            this.ColAlmacen.Name = "ColAlmacen";
            this.ColAlmacen.OptionsColumn.ReadOnly = true;
            this.ColAlmacen.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Litros", "{0:N2}")});
            this.ColAlmacen.Visible = true;
            this.ColAlmacen.Width = 90;
            // 
            // colCostoTotal
            // 
            this.colCostoTotal.Caption = "Cto x Gln";
            this.colCostoTotal.DisplayFormat.FormatString = "n2";
            this.colCostoTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCostoTotal.FieldName = "Costo";
            this.colCostoTotal.Name = "colCostoTotal";
            this.colCostoTotal.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colCostoTotal.Visible = true;
            this.colCostoTotal.Width = 87;
            // 
            // colQuantity
            // 
            this.colQuantity.Caption = "CostoTotal";
            this.colQuantity.DisplayFormat.FormatString = "n2";
            this.colQuantity.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colQuantity.FieldName = "CostoTotal";
            this.colQuantity.Name = "colQuantity";
            this.colQuantity.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "CostoTotal", "{0:n2}")});
            this.colQuantity.UnboundExpression = "Round([Costo] * [Galones], 2)";
            this.colQuantity.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colQuantity.Visible = true;
            this.colQuantity.Width = 50;
            // 
            // colCost
            // 
            this.colCost.Caption = "Costo Unitario";
            this.colCost.DisplayFormat.FormatString = "n4";
            this.colCost.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCost.FieldName = "CostoEntrada";
            this.colCost.Name = "colCost";
            this.colCost.OptionsColumn.ReadOnly = true;
            this.colCost.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colCost.Width = 86;
            // 
            // colCostoSubTotal
            // 
            this.colCostoSubTotal.Caption = "SubTotal";
            this.colCostoSubTotal.ColumnEdit = this.calcSubTotal;
            this.colCostoSubTotal.DisplayFormat.FormatString = "n2";
            this.colCostoSubTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colCostoSubTotal.FieldName = "SubTotal";
            this.colCostoSubTotal.Name = "colCostoSubTotal";
            this.colCostoSubTotal.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "SubTotal", "{0:n2}")});
            this.colCostoSubTotal.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            // 
            // calcSubTotal
            // 
            this.calcSubTotal.AllowMouseWheel = false;
            this.calcSubTotal.AutoHeight = false;
            this.calcSubTotal.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.calcSubTotal.DisplayFormat.FormatString = "n2";
            this.calcSubTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.calcSubTotal.EditFormat.FormatString = "n2";
            this.calcSubTotal.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.calcSubTotal.Mask.EditMask = "n2";
            this.calcSubTotal.Name = "calcSubTotal";
            this.calcSubTotal.Precision = 2;
            // 
            // colISC
            // 
            this.colISC.Caption = "ISC";
            this.colISC.ColumnEdit = this.spMontoISC;
            this.colISC.DisplayFormat.FormatString = "n4";
            this.colISC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colISC.FieldName = "MontoISC";
            this.colISC.Name = "colISC";
            this.colISC.OptionsColumn.ReadOnly = true;
            this.colISC.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "MontoISC", "{0:n2}")});
            this.colISC.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            // 
            // spMontoISC
            // 
            this.spMontoISC.AutoHeight = false;
            this.spMontoISC.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.spMontoISC.DisplayFormat.FormatString = "N4";
            this.spMontoISC.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spMontoISC.EditFormat.FormatString = "N4";
            this.spMontoISC.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spMontoISC.Mask.EditMask = "N4";
            this.spMontoISC.Name = "spMontoISC";
            // 
            // colTaxValue
            // 
            this.colTaxValue.Caption = "IVA";
            this.colTaxValue.ColumnEdit = this.spTax;
            this.colTaxValue.DisplayFormat.FormatString = "n2";
            this.colTaxValue.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTaxValue.FieldName = "ImpuestoTotal";
            this.colTaxValue.Name = "colTaxValue";
            this.colTaxValue.OptionsColumn.ReadOnly = true;
            this.colTaxValue.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "ImpuestoTotal", "{0:n2}")});
            this.colTaxValue.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colTaxValue.Width = 44;
            // 
            // spTax
            // 
            this.spTax.AllowMouseWheel = false;
            this.spTax.AutoHeight = false;
            this.spTax.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, editorButtonImageOptions1)});
            this.spTax.DisplayFormat.FormatString = "n2";
            this.spTax.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spTax.EditFormat.FormatString = "n2";
            this.spTax.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.spTax.Name = "spTax";
            // 
            // colTotal
            // 
            this.colTotal.Caption = "TOTAL";
            this.colTotal.DisplayFormat.FormatString = "n2";
            this.colTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTotal.FieldName = "Total";
            this.colTotal.Name = "colTotal";
            this.colTotal.OptionsColumn.ReadOnly = true;
            this.colTotal.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Total", "{0:n2}")});
            this.colTotal.UnboundExpression = "CostoTotal + ImpuestoTotal";
            this.colTotal.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.colTotal.Width = 67;
            // 
            // colDetalle
            // 
            this.colDetalle.Caption = "Información";
            this.colDetalle.FieldName = "DetalleCombustible";
            this.colDetalle.Name = "colDetalle";
            this.colDetalle.OptionsColumn.AllowEdit = false;
            this.colDetalle.OptionsColumn.AllowFocus = false;
            this.colDetalle.RowIndex = 1;
            // 
            // colExentoIVA
            // 
            this.colExentoIVA.FieldName = "EsManejo";
            this.colExentoIVA.Name = "colExentoIVA";
            // 
            // colEsISC
            // 
            this.colEsISC.FieldName = "EsProducto";
            this.colEsISC.Name = "colEsISC";
            // 
            // colPrecio
            // 
            this.colPrecio.FieldName = "Precio";
            this.colPrecio.Name = "colPrecio";
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
            // speCantidad
            // 
            this.speCantidad.AutoHeight = false;
            this.speCantidad.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.speCantidad.MaxValue = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.speCantidad.Name = "speCantidad";
            // 
            // speCost
            // 
            this.speCost.AutoHeight = false;
            this.speCost.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
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
            this.gridProductos.Name = "gridProductos";
            this.gridProductos.NullText = "";
            this.gridProductos.View = this.gridViewProductos;
            // 
            // gridViewProductos
            // 
            this.gridViewProductos.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewProductos.Name = "gridViewProductos";
            this.gridViewProductos.OptionsBehavior.Editable = false;
            this.gridViewProductos.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewProductos.OptionsView.ShowAutoFilterRow = true;
            this.gridViewProductos.OptionsView.ShowGroupPanel = false;
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
            // calcCantidad
            // 
            this.calcCantidad.AllowMouseWheel = false;
            this.calcCantidad.AutoHeight = false;
            this.calcCantidad.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.calcCantidad.DisplayFormat.FormatString = "n3";
            this.calcCantidad.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.calcCantidad.EditFormat.FormatString = "n3";
            this.calcCantidad.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.calcCantidad.Mask.EditMask = "n3";
            this.calcCantidad.Name = "calcCantidad";
            this.calcCantidad.Precision = 3;
            // 
            // calcCost
            // 
            this.calcCost.AllowMouseWheel = false;
            this.calcCost.AutoHeight = false;
            this.calcCost.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.calcCost.DisplayFormat.FormatString = "n2";
            this.calcCost.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.calcCost.EditFormat.FormatString = "n2";
            this.calcCost.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.calcCost.Mask.EditMask = "n2";
            this.calcCost.Name = "calcCost";
            this.calcCost.Precision = 2;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnCancel);
            this.panelControl2.Controls.Add(this.btnOK);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 329);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(785, 34);
            this.panelControl2.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCancel.ImageOptions.Image = global::SAGAS.Contabilidad.Properties.Resources.cancel20;
            this.btnCancel.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnCancel.Location = new System.Drawing.Point(92, 2);
            this.btnCancel.LookAndFeel.SkinName = "McSkin";
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(87, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnOK.ImageOptions.Image = global::SAGAS.Contabilidad.Properties.Resources.Ok20;
            this.btnOK.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnOK.Location = new System.Drawing.Point(2, 2);
            this.btnOK.LookAndFeel.SkinName = "McSkin";
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(90, 30);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "Seleccionar";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click_1);
            // 
            // errRequiredField
            // 
            this.errRequiredField.ContainerControl = this;
            // 
            // bandedGridColumn1
            // 
            this.bandedGridColumn1.Caption = "Fecha Carga";
            this.bandedGridColumn1.FieldName = "FechaSolicitud";
            this.bandedGridColumn1.Name = "bandedGridColumn1";
            this.bandedGridColumn1.OptionsColumn.AllowEdit = false;
            this.bandedGridColumn1.OptionsColumn.AllowFocus = false;
            this.bandedGridColumn1.Visible = true;
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = "gridBand1";
            this.gridBand1.Columns.Add(this.bandedGridColumn1);
            this.gridBand1.Columns.Add(this.colProduct);
            this.gridBand1.Columns.Add(this.colUnit);
            this.gridBand1.Columns.Add(this.colGlnPendiente);
            this.gridBand1.Columns.Add(this.ColAlmacen);
            this.gridBand1.Columns.Add(this.colCostoTotal);
            this.gridBand1.Columns.Add(this.colQuantity);
            this.gridBand1.Columns.Add(this.colCost);
            this.gridBand1.Columns.Add(this.colCostoSubTotal);
            this.gridBand1.Columns.Add(this.colISC);
            this.gridBand1.Columns.Add(this.colTaxValue);
            this.gridBand1.Columns.Add(this.colTotal);
            this.gridBand1.Columns.Add(this.colDetalle);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 0;
            this.gridBand1.Width = 540;
            // 
            // DialogSeleccionarOrden
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(785, 363);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DialogSeleccionarOrden";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "<>";
            this.Load += new System.EventHandler(this.DialogUser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitCentral)).EndInit();
            this.splitCentral.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstacionServicio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpIslas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpTecnico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpEstado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkSES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkEstado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkMoneda)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkMes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemDateEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpChkAprobado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.calcSubTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spMontoISC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboImpuestos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboUnidadMedida)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewProductos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboAlmacen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSubTotal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.calcCantidad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.calcCost)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errRequiredField;
        private DevExpress.XtraEditors.SplitContainerControl splitCentral;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colNumero;
        private DevExpress.XtraGrid.Columns.GridColumn colFecha;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rpChkAprobado;
        private DevExpress.XtraGrid.Columns.GridColumn colEstacionID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpEstacionServicio;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpIslas;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpTecnico;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpEstado;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkSES;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkEstado;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkMoneda;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkMes;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit repositoryItemDateEdit1;
        public DevExpress.XtraGrid.GridControl gridDetalle;
        private DevExpress.XtraGrid.Views.BandedGrid.AdvBandedGridView gvDetalle;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colProduct;
        private DevExpress.XtraEditors.Repository.RepositoryItemGridLookUpEdit gridProductos;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewProductos;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colUnit;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboUnidadMedida;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn ColAlmacen;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboAlmacen;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colQuantity;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit calcCantidad;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCost;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCostoSubTotal;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit calcSubTotal;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colISC;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spMontoISC;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCostoTotal;
        private DevExpress.XtraEditors.Repository.RepositoryItemCalcEdit calcCost;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colTaxValue;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spTax;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colTotal;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDetalle;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colExentoIVA;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colEsISC;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colPrecio;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit cboImpuestos;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit speCantidad;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit speCost;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit spSubTotal;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colGlnPendiente;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn1;
    }
}