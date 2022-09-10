namespace SAGAS.Arqueo.Forms
{
    partial class FormPrecioCombustible
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
            DevExpress.XtraPivotGrid.PivotGridCustomTotal pivotGridCustomTotal1 = new DevExpress.XtraPivotGrid.PivotGridCustomTotal();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrecioCombustible));
            this.colDiferenciado = new DevExpress.XtraPivotGrid.PivotGridField();
            this.chkDif = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colProducto = new DevExpress.XtraPivotGrid.PivotGridField();
            this.rowZonaNombre = new DevExpress.XtraPivotGrid.PivotGridField();
            this.rowESNombres = new DevExpress.XtraPivotGrid.PivotGridField();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFechaInicial = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rDateInicio = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.colFechaFinal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rDateFin = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.splitContainerControlCenter = new DevExpress.XtraEditors.SplitContainerControl();
            this.pGridPCD = new DevExpress.XtraPivotGrid.PivotGridControl();
            this.dataPrecio = new DevExpress.XtraPivotGrid.PivotGridField();
            this.colCodigo = new DevExpress.XtraPivotGrid.PivotGridField();
            this.gridPCD = new DevExpress.XtraGrid.GridControl();
            this.bgvEs = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.bandEs = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colIDP = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colIDSUS = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.colES = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombreEstacion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControlTop = new DevExpress.XtraEditors.PanelControl();
            this.btnPrecioPorTurno = new DevExpress.XtraEditors.SimpleButton();
            this.btnCambiarRangoPrecio = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDif)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rDateInicio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rDateInicio.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rDateFin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rDateFin.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlCenter)).BeginInit();
            this.splitContainerControlCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pGridPCD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPCD)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgvEs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).BeginInit();
            this.panelControlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // colDiferenciado
            // 
            this.colDiferenciado.Appearance.Cell.Options.UseTextOptions = true;
            this.colDiferenciado.Appearance.Cell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDiferenciado.Appearance.Header.Options.UseTextOptions = true;
            this.colDiferenciado.Appearance.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colDiferenciado.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.colDiferenciado.AreaIndex = 0;
            this.colDiferenciado.Caption = "Dif";
            this.colDiferenciado.ExpandedInFieldsGroup = false;
            this.colDiferenciado.FieldName = "PrecioDiferenciado";
            this.colDiferenciado.Name = "colDiferenciado";
            this.colDiferenciado.Options.AllowExpand = DevExpress.Utils.DefaultBoolean.False;
            this.colDiferenciado.Options.ShowCustomTotals = false;
            this.colDiferenciado.Options.ShowGrandTotal = false;
            this.colDiferenciado.Options.ShowTotals = false;
            this.colDiferenciado.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            // 
            // chkDif
            // 
            this.chkDif.AutoHeight = false;
            this.chkDif.Caption = "Es Diferenciado";
            this.chkDif.Name = "chkDif";
            // 
            // colProducto
            // 
            this.colProducto.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.colProducto.AreaIndex = 2;
            this.colProducto.Caption = "Producto";
            this.colProducto.EmptyCellText = "0";
            this.colProducto.EmptyValueText = "0";
            this.colProducto.FieldName = "Nombre";
            this.colProducto.Name = "colProducto";
            this.colProducto.Options.AllowDragInCustomizationForm = DevExpress.Utils.DefaultBoolean.True;
            this.colProducto.SortOrder = DevExpress.XtraPivotGrid.PivotSortOrder.Descending;
            this.colProducto.UnboundFieldName = "ProductoID";
            this.colProducto.UnboundType = DevExpress.Data.UnboundColumnType.Integer;
            // 
            // rowZonaNombre
            // 
            this.rowZonaNombre.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.rowZonaNombre.AreaIndex = 0;
            this.rowZonaNombre.Caption = "Zona";
            pivotGridCustomTotal1.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Average;
            this.rowZonaNombre.CustomTotals.AddRange(new DevExpress.XtraPivotGrid.PivotGridCustomTotal[] {
            pivotGridCustomTotal1});
            this.rowZonaNombre.FieldName = "ZonaNombre";
            this.rowZonaNombre.Name = "rowZonaNombre";
            this.rowZonaNombre.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Average;
            this.rowZonaNombre.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.rowZonaNombre.Width = 60;
            // 
            // rowESNombres
            // 
            this.rowESNombres.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.rowESNombres.AreaIndex = 1;
            this.rowESNombres.Caption = "Estación de Servicio";
            this.rowESNombres.FieldName = "Estacion";
            this.rowESNombres.Name = "rowESNombres";
            this.rowESNombres.Width = 150;
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Excel";
            this.barButtonItem1.Id = 6;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Caption = "PDF";
            this.barButtonItem2.Id = 7;
            this.barButtonItem2.Name = "barButtonItem2";
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
            this.grid.EmbeddedNavigator.Buttons.EnabledAutoRepeat = false;
            this.grid.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.grid.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.grid.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.grid.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.MainView = this.gvData;
            this.grid.MenuManager = this.barManager;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.rDateInicio,
            this.rDateFin});
            this.grid.Size = new System.Drawing.Size(309, 515);
            this.grid.TabIndex = 5;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colFechaInicial,
            this.colFechaFinal});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsDetail.AllowZoomDetail = false;
            this.gvData.OptionsDetail.EnableMasterViewMode = false;
            this.gvData.OptionsDetail.ShowDetailTabs = false;
            this.gvData.OptionsDetail.SmartDetailExpand = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gvData.OptionsView.ShowGroupPanel = false;
            this.gvData.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colFechaFinal, DevExpress.Data.ColumnSortOrder.Descending)});
            this.gvData.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvData_FocusedRowChanged);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colFechaInicial
            // 
            this.colFechaInicial.Caption = "Fecha Inicial";
            this.colFechaInicial.ColumnEdit = this.rDateInicio;
            this.colFechaInicial.DisplayFormat.FormatString = "d";
            this.colFechaInicial.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colFechaInicial.FieldName = "FechaInicial";
            this.colFechaInicial.Name = "colFechaInicial";
            this.colFechaInicial.UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            this.colFechaInicial.Visible = true;
            this.colFechaInicial.VisibleIndex = 0;
            this.colFechaInicial.Width = 131;
            // 
            // rDateInicio
            // 
            this.rDateInicio.AutoHeight = false;
            this.rDateInicio.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rDateInicio.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.rDateInicio.Name = "rDateInicio";
            // 
            // colFechaFinal
            // 
            this.colFechaFinal.Caption = "Fecha Final";
            this.colFechaFinal.ColumnEdit = this.rDateFin;
            this.colFechaFinal.DisplayFormat.FormatString = "d";
            this.colFechaFinal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.colFechaFinal.FieldName = "FechaFinal";
            this.colFechaFinal.Name = "colFechaFinal";
            this.colFechaFinal.UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
            this.colFechaFinal.Visible = true;
            this.colFechaFinal.VisibleIndex = 1;
            this.colFechaFinal.Width = 147;
            // 
            // rDateFin
            // 
            this.rDateFin.AutoHeight = false;
            this.rDateFin.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rDateFin.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.rDateFin.Name = "rDateFin";
            // 
            // splitContainerControlCenter
            // 
            this.splitContainerControlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControlCenter.Location = new System.Drawing.Point(0, 39);
            this.splitContainerControlCenter.Name = "splitContainerControlCenter";
            this.splitContainerControlCenter.Panel1.Controls.Add(this.grid);
            this.splitContainerControlCenter.Panel1.Text = "Panel1";
            this.splitContainerControlCenter.Panel2.Controls.Add(this.pGridPCD);
            this.splitContainerControlCenter.Panel2.Controls.Add(this.gridPCD);
            this.splitContainerControlCenter.Panel2.Controls.Add(this.panelControlTop);
            this.splitContainerControlCenter.Panel2.Text = "Panel2";
            this.splitContainerControlCenter.Size = new System.Drawing.Size(818, 515);
            this.splitContainerControlCenter.SplitterPosition = 309;
            this.splitContainerControlCenter.TabIndex = 6;
            this.splitContainerControlCenter.Text = "splitContainerControl1";
            // 
            // pGridPCD
            // 
            this.pGridPCD.Appearance.Cell.BackColor = System.Drawing.Color.White;
            this.pGridPCD.Appearance.Cell.Font = new System.Drawing.Font("Tahoma", 8F);
            this.pGridPCD.Appearance.Cell.Options.UseBackColor = true;
            this.pGridPCD.Appearance.Cell.Options.UseFont = true;
            this.pGridPCD.Appearance.ColumnHeaderArea.BackColor = System.Drawing.Color.LightCyan;
            this.pGridPCD.Appearance.ColumnHeaderArea.Options.UseBackColor = true;
            this.pGridPCD.Appearance.ColumnHeaderArea.Options.UseTextOptions = true;
            this.pGridPCD.Appearance.ColumnHeaderArea.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pGridPCD.Appearance.FieldHeader.BackColor = System.Drawing.Color.LightCyan;
            this.pGridPCD.Appearance.FieldHeader.Options.UseBackColor = true;
            this.pGridPCD.Appearance.FieldValue.BackColor = System.Drawing.Color.AliceBlue;
            this.pGridPCD.Appearance.FieldValue.Options.UseBackColor = true;
            this.pGridPCD.Appearance.FilterHeaderArea.BackColor = System.Drawing.Color.LightCyan;
            this.pGridPCD.Appearance.FilterHeaderArea.Options.UseBackColor = true;
            this.pGridPCD.Appearance.HeaderArea.BackColor = System.Drawing.Color.LightGray;
            this.pGridPCD.Appearance.HeaderArea.Options.UseBackColor = true;
            this.pGridPCD.Appearance.HeaderGroupLine.BackColor = System.Drawing.Color.LightCyan;
            this.pGridPCD.Appearance.HeaderGroupLine.Options.UseBackColor = true;
            this.pGridPCD.Appearance.RowHeaderArea.BackColor = System.Drawing.Color.LightGray;
            this.pGridPCD.Appearance.RowHeaderArea.Options.UseBackColor = true;
            this.pGridPCD.BackColor = System.Drawing.Color.LightCyan;
            this.pGridPCD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pGridPCD.Fields.AddRange(new DevExpress.XtraPivotGrid.PivotGridField[] {
            this.rowZonaNombre,
            this.rowESNombres,
            this.colDiferenciado,
            this.colProducto,
            this.dataPrecio,
            this.colCodigo});
            this.pGridPCD.Location = new System.Drawing.Point(0, 29);
            this.pGridPCD.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Style3D;
            this.pGridPCD.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pGridPCD.Name = "pGridPCD";
            this.pGridPCD.OptionsBehavior.CopyToClipboardWithFieldValues = true;
            this.pGridPCD.OptionsCustomization.AllowDrag = false;
            this.pGridPCD.OptionsCustomization.AllowEdit = false;
            this.pGridPCD.OptionsCustomization.AllowSort = false;
            this.pGridPCD.OptionsView.ShowColumnGrandTotalHeader = false;
            this.pGridPCD.OptionsView.ShowColumnHeaders = false;
            this.pGridPCD.OptionsView.ShowColumnTotals = false;
            this.pGridPCD.OptionsView.ShowDataHeaders = false;
            this.pGridPCD.OptionsView.ShowFilterHeaders = false;
            this.pGridPCD.OptionsView.ShowRowGrandTotalHeader = false;
            this.pGridPCD.OptionsView.ShowRowGrandTotals = false;
            this.pGridPCD.OptionsView.ShowRowTotals = false;
            this.pGridPCD.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.chkDif});
            this.pGridPCD.Size = new System.Drawing.Size(504, 416);
            this.pGridPCD.TabIndex = 9;
            this.pGridPCD.FieldValueDisplayText += new DevExpress.XtraPivotGrid.PivotFieldDisplayTextEventHandler(this.pGridPCD_FieldValueDisplayText);
            this.pGridPCD.CustomDrawFieldHeader += new DevExpress.XtraPivotGrid.PivotCustomDrawFieldHeaderEventHandler(this.pGridPCD_CustomDrawFieldHeader);
            this.pGridPCD.CustomDrawFieldValue += new DevExpress.XtraPivotGrid.PivotCustomDrawFieldValueEventHandler(this.pGridPCD_CustomDrawFieldValue);
            // 
            // dataPrecio
            // 
            this.dataPrecio.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.dataPrecio.AreaIndex = 0;
            this.dataPrecio.Caption = "Precio";
            this.dataPrecio.CellFormat.FormatString = "n2";
            this.dataPrecio.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.dataPrecio.EmptyCellText = "00.00";
            this.dataPrecio.EmptyValueText = "00.00";
            this.dataPrecio.FieldName = "Precio";
            this.dataPrecio.Name = "dataPrecio";
            this.dataPrecio.Options.ShowCustomTotals = false;
            this.dataPrecio.Options.ShowGrandTotal = false;
            this.dataPrecio.Options.ShowTotals = false;
            // 
            // colCodigo
            // 
            this.colCodigo.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.colCodigo.AreaIndex = 1;
            this.colCodigo.Caption = "Codigo";
            this.colCodigo.ExpandedInFieldsGroup = false;
            this.colCodigo.FieldName = "Codigo";
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.Options.AllowExpand = DevExpress.Utils.DefaultBoolean.False;
            // 
            // gridPCD
            // 
            this.gridPCD.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridPCD.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gridPCD.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridPCD.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.gridPCD.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridPCD.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.gridPCD.EmbeddedNavigator.Buttons.Edit.Visible = false;
            this.gridPCD.EmbeddedNavigator.Buttons.EnabledAutoRepeat = false;
            this.gridPCD.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.gridPCD.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridPCD.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gridPCD.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridPCD.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gridPCD.Location = new System.Drawing.Point(0, 445);
            this.gridPCD.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Style3D;
            this.gridPCD.LookAndFeel.UseDefaultLookAndFeel = false;
            this.gridPCD.MainView = this.bgvEs;
            this.gridPCD.Name = "gridPCD";
            this.gridPCD.Size = new System.Drawing.Size(504, 70);
            this.gridPCD.TabIndex = 7;
            this.gridPCD.UseEmbeddedNavigator = true;
            this.gridPCD.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.bgvEs,
            this.gridView1});
            this.gridPCD.Visible = false;
            // 
            // bgvEs
            // 
            this.bgvEs.Appearance.BandPanel.BackColor = System.Drawing.Color.LightCyan;
            this.bgvEs.Appearance.BandPanel.BackColor2 = System.Drawing.Color.LightCyan;
            this.bgvEs.Appearance.BandPanel.Options.UseBackColor = true;
            this.bgvEs.Appearance.BandPanelBackground.BackColor = System.Drawing.Color.LightCyan;
            this.bgvEs.Appearance.BandPanelBackground.BackColor2 = System.Drawing.Color.LightCyan;
            this.bgvEs.Appearance.BandPanelBackground.Options.UseBackColor = true;
            this.bgvEs.Appearance.HeaderPanel.BackColor = System.Drawing.Color.Transparent;
            this.bgvEs.Appearance.HeaderPanel.BackColor2 = System.Drawing.Color.Transparent;
            this.bgvEs.Appearance.HeaderPanel.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.bgvEs.Appearance.HeaderPanel.Options.UseBackColor = true;
            this.bgvEs.Appearance.HeaderPanelBackground.ForeColor = System.Drawing.Color.Red;
            this.bgvEs.Appearance.HeaderPanelBackground.Options.UseForeColor = true;
            this.bgvEs.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.bandEs});
            this.bgvEs.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colIDSUS,
            this.colIDP,
            this.colES});
            this.bgvEs.GridControl = this.gridPCD;
            this.bgvEs.Name = "bgvEs";
            this.bgvEs.OptionsBehavior.Editable = false;
            this.bgvEs.OptionsCustomization.AllowBandMoving = false;
            this.bgvEs.OptionsCustomization.AllowBandResizing = false;
            this.bgvEs.OptionsCustomization.AllowColumnMoving = false;
            this.bgvEs.OptionsCustomization.AllowColumnResizing = false;
            this.bgvEs.OptionsCustomization.AllowFilter = false;
            this.bgvEs.OptionsCustomization.AllowSort = false;
            this.bgvEs.OptionsMenu.EnableColumnMenu = false;
            this.bgvEs.OptionsMenu.EnableFooterMenu = false;
            this.bgvEs.OptionsMenu.EnableGroupPanelMenu = false;
            this.bgvEs.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.bgvEs.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.bgvEs.OptionsView.ColumnAutoWidth = false;
            this.bgvEs.OptionsView.ShowGroupPanel = false;
            // 
            // bandEs
            // 
            this.bandEs.Caption = "<>";
            this.bandEs.Columns.Add(this.colIDP);
            this.bandEs.Columns.Add(this.colIDSUS);
            this.bandEs.Columns.Add(this.colES);
            this.bandEs.MinWidth = 20;
            this.bandEs.Name = "bandEs";
            this.bandEs.OptionsBand.AllowMove = false;
            this.bandEs.OptionsBand.AllowPress = false;
            this.bandEs.OptionsBand.ShowCaption = false;
            this.bandEs.VisibleIndex = 0;
            this.bandEs.Width = 176;
            // 
            // colIDP
            // 
            this.colIDP.FieldName = "IDP";
            this.colIDP.Name = "colIDP";
            // 
            // colIDSUS
            // 
            this.colIDSUS.FieldName = "IDSUS";
            this.colIDSUS.Name = "colIDSUS";
            // 
            // colES
            // 
            this.colES.AppearanceHeader.BackColor = System.Drawing.Color.LightCyan;
            this.colES.AppearanceHeader.BackColor2 = System.Drawing.Color.LightCyan;
            this.colES.AppearanceHeader.BorderColor = System.Drawing.Color.Transparent;
            this.colES.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.colES.AppearanceHeader.Options.UseBackColor = true;
            this.colES.AppearanceHeader.Options.UseForeColor = true;
            this.colES.Caption = "Estación de Servicio";
            this.colES.FieldName = "ES";
            this.colES.Name = "colES";
            this.colES.OptionsColumn.AllowEdit = false;
            this.colES.Visible = true;
            this.colES.Width = 176;
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.colNombreEstacion});
            this.gridView1.GridControl = this.gridPCD;
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsMenu.EnableColumnMenu = false;
            this.gridView1.OptionsMenu.EnableFooterMenu = false;
            this.gridView1.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridView1.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gridView1.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowAutoFilterRow = true;
            this.gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.FieldName = "ID";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // colNombreEstacion
            // 
            this.colNombreEstacion.Caption = "Estación de Servicio";
            this.colNombreEstacion.FieldName = "Nombre";
            this.colNombreEstacion.Name = "colNombreEstacion";
            this.colNombreEstacion.Visible = true;
            this.colNombreEstacion.VisibleIndex = 0;
            this.colNombreEstacion.Width = 265;
            // 
            // panelControlTop
            // 
            this.panelControlTop.Controls.Add(this.btnPrecioPorTurno);
            this.panelControlTop.Controls.Add(this.btnCambiarRangoPrecio);
            this.panelControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlTop.Location = new System.Drawing.Point(0, 0);
            this.panelControlTop.Name = "panelControlTop";
            this.panelControlTop.Size = new System.Drawing.Size(504, 29);
            this.panelControlTop.TabIndex = 8;
            // 
            // btnPrecioPorTurno
            // 
            this.btnPrecioPorTurno.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnPrecioPorTurno.Image = global::SAGAS.Arqueo.Properties.Resources.counter;
            this.btnPrecioPorTurno.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnPrecioPorTurno.Location = new System.Drawing.Point(236, 2);
            this.btnPrecioPorTurno.Name = "btnPrecioPorTurno";
            this.btnPrecioPorTurno.Size = new System.Drawing.Size(156, 25);
            this.btnPrecioPorTurno.TabIndex = 1;
            this.btnPrecioPorTurno.Text = "Agregar Precio por Turno";
            this.btnPrecioPorTurno.Click += new System.EventHandler(this.btnPrecioPorTurno_Click);
            // 
            // btnCambiarRangoPrecio
            // 
            this.btnCambiarRangoPrecio.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnCambiarRangoPrecio.Image = global::SAGAS.Arqueo.Properties.Resources.CambioPrecioCombustible;
            this.btnCambiarRangoPrecio.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnCambiarRangoPrecio.Location = new System.Drawing.Point(2, 2);
            this.btnCambiarRangoPrecio.Name = "btnCambiarRangoPrecio";
            this.btnCambiarRangoPrecio.Size = new System.Drawing.Size(234, 25);
            this.btnCambiarRangoPrecio.TabIndex = 0;
            this.btnCambiarRangoPrecio.Text = "Cambiar Precio de Combustible por Rango";
            this.btnCambiarRangoPrecio.Click += new System.EventHandler(this.btnCambiarRangoPrecio_Click);
            // 
            // FormPrecioCombustible
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 554);
            this.Controls.Add(this.splitContainerControlCenter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormPrecioCombustible";
            this.Text = "Lista Precio de Combustibles";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.splitContainerControlCenter, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkDif)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rDateInicio.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rDateInicio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rDateFin.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rDateFin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlCenter)).EndInit();
            this.splitContainerControlCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pGridPCD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridPCD)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bgvEs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).EndInit();
            this.panelControlTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colFechaInicial;
        private DevExpress.XtraGrid.Columns.GridColumn colFechaFinal;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit rDateInicio;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit rDateFin;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControlCenter;
        private DevExpress.XtraGrid.GridControl gridPCD;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView bgvEs;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colIDP;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colES;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn colNombreEstacion;
        private DevExpress.XtraEditors.PanelControl panelControlTop;
        private DevExpress.XtraEditors.SimpleButton btnCambiarRangoPrecio;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colIDSUS;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand bandEs;
        private DevExpress.XtraPivotGrid.PivotGridControl pGridPCD;
        private DevExpress.XtraPivotGrid.PivotGridField rowZonaNombre;
        private DevExpress.XtraPivotGrid.PivotGridField rowESNombres;
        private DevExpress.XtraPivotGrid.PivotGridField colProducto;
        private DevExpress.XtraPivotGrid.PivotGridField dataPrecio;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkDif;
        private DevExpress.XtraPivotGrid.PivotGridField colDiferenciado;
        private DevExpress.XtraPivotGrid.PivotGridField colCodigo;
        private DevExpress.XtraEditors.SimpleButton btnPrecioPorTurno;
    }
}

