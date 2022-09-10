namespace SAGAS.Nomina.Forms
{
    partial class FormPlanVacaciones
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPlanVacaciones));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            this.layoutControlTop = new DevExpress.XtraLayout.LayoutControl();
            this.btnPreview = new DevExpress.XtraEditors.SimpleButton();
            this.lePlanilla = new DevExpress.XtraEditors.LookUpEdit();
            this.btnPDF = new DevExpress.XtraEditors.SimpleButton();
            this.btnExcel = new DevExpress.XtraEditors.SimpleButton();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.btnLoad = new DevExpress.XtraEditors.SimpleButton();
            this.dateFechaDesde = new DevExpress.XtraEditors.DateEdit();
            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
            this.lciFechaCorte = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciLoad = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciClose = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciExcel = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPDF = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciES = new DevExpress.XtraLayout.LayoutControlItem();
            this.lciPreview = new DevExpress.XtraLayout.LayoutControlItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.bdsDetalle = new System.Windows.Forms.BindingSource(this.components);
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkrEmpresa = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.chrSelectedSucursal = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.rspValor = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.chkSelected = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.errRequiredField = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.infDifES = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            this.errTC_Periodo = new DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).BeginInit();
            this.layoutControlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lePlanilla.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaDesde.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaDesde.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFechaCorte)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciLoad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciExcel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPDF)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrEmpresa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrSelectedSucursal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rspValor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errTC_Periodo)).BeginInit();
            this.SuspendLayout();
            // 
            // layoutControlTop
            // 
            this.layoutControlTop.Controls.Add(this.btnPreview);
            this.layoutControlTop.Controls.Add(this.lePlanilla);
            this.layoutControlTop.Controls.Add(this.btnPDF);
            this.layoutControlTop.Controls.Add(this.btnExcel);
            this.layoutControlTop.Controls.Add(this.btnClose);
            this.layoutControlTop.Controls.Add(this.btnLoad);
            this.layoutControlTop.Controls.Add(this.dateFechaDesde);
            this.layoutControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.layoutControlTop.Location = new System.Drawing.Point(2, 2);
            this.layoutControlTop.Name = "layoutControlTop";
            this.layoutControlTop.Root = this.layoutControlGroup1;
            this.layoutControlTop.Size = new System.Drawing.Size(1046, 68);
            this.layoutControlTop.TabIndex = 0;
            this.layoutControlTop.Text = "layoutControl1";
            // 
            // btnPreview
            // 
            this.btnPreview.Image = global::SAGAS.Nomina.Properties.Resources.sallary_deferrais;
            this.btnPreview.Location = new System.Drawing.Point(637, 25);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(94, 30);
            this.btnPreview.StyleController = this.layoutControlTop;
            this.btnPreview.TabIndex = 8;
            this.btnPreview.Text = "Vista previa";
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // lePlanilla
            // 
            this.lePlanilla.Location = new System.Drawing.Point(377, 25);
            this.lePlanilla.Name = "lePlanilla";
            this.lePlanilla.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.lePlanilla.Properties.Appearance.Options.UseFont = true;
            this.lePlanilla.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lePlanilla.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Estación")});
            this.lePlanilla.Properties.DisplayMember = "Nombre";
            this.lePlanilla.Properties.NullText = "<Seleccione la Planilla>";
            this.lePlanilla.Properties.ValueMember = "ID";
            this.lePlanilla.Size = new System.Drawing.Size(156, 30);
            this.lePlanilla.StyleController = this.layoutControlTop;
            this.lePlanilla.TabIndex = 7;
            // 
            // btnPDF
            // 
            this.btnPDF.Image = ((System.Drawing.Image)(resources.GetObject("btnPDF.Image")));
            this.btnPDF.Location = new System.Drawing.Point(808, 25);
            this.btnPDF.Name = "btnPDF";
            this.btnPDF.Size = new System.Drawing.Size(111, 30);
            this.btnPDF.StyleController = this.layoutControlTop;
            this.btnPDF.TabIndex = 6;
            this.btnPDF.Text = "Exportar a PFD";
            this.btnPDF.Click += new System.EventHandler(this.btnPDF_Click);
            // 
            // btnExcel
            // 
            this.btnExcel.Image = ((System.Drawing.Image)(resources.GetObject("btnExcel.Image")));
            this.btnExcel.Location = new System.Drawing.Point(923, 25);
            this.btnExcel.Name = "btnExcel";
            this.btnExcel.Size = new System.Drawing.Size(117, 30);
            this.btnExcel.StyleController = this.layoutControlTop;
            this.btnExcel.TabIndex = 6;
            this.btnExcel.Text = "Exportar a Excel";
            this.btnExcel.Click += new System.EventHandler(this.btnExcel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Image = global::SAGAS.Nomina.Properties.Resources.cancel20;
            this.btnClose.Location = new System.Drawing.Point(735, 25);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(69, 30);
            this.btnClose.StyleController = this.layoutControlTop;
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Cerrar";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnLoad
            // 
            this.btnLoad.Image = global::SAGAS.Nomina.Properties.Resources.Ok20;
            this.btnLoad.Location = new System.Drawing.Point(537, 25);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(96, 30);
            this.btnLoad.StyleController = this.layoutControlTop;
            this.btnLoad.TabIndex = 0;
            this.btnLoad.Text = "Cargar Lista";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // dateFechaDesde
            // 
            this.dateFechaDesde.EditValue = null;
            this.dateFechaDesde.Location = new System.Drawing.Point(124, 25);
            this.dateFechaDesde.Name = "dateFechaDesde";
            this.dateFechaDesde.Properties.AllowMouseWheel = false;
            this.dateFechaDesde.Properties.Appearance.Font = new System.Drawing.Font("Tahoma", 14F);
            this.dateFechaDesde.Properties.Appearance.Options.UseFont = true;
            this.dateFechaDesde.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dateFechaDesde.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.dateFechaDesde.Properties.DisplayFormat.FormatString = "MM/yyyy";
            this.dateFechaDesde.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateFechaDesde.Properties.EditFormat.FormatString = "MM/yyyy";
            this.dateFechaDesde.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.dateFechaDesde.Properties.Mask.EditMask = "MM/yyyy";
            this.dateFechaDesde.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearView;
            this.dateFechaDesde.Size = new System.Drawing.Size(164, 30);
            this.dateFechaDesde.StyleController = this.layoutControlTop;
            this.dateFechaDesde.TabIndex = 4;
            // 
            // layoutControlGroup1
            // 
            this.layoutControlGroup1.CustomizationFormText = "Datos de la Sede";
            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
            this.lciFechaCorte,
            this.lciLoad,
            this.lciClose,
            this.lciExcel,
            this.lciPDF,
            this.lciES,
            this.lciPreview});
            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
            this.layoutControlGroup1.Name = "layoutControlGroup1";
            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(3, 3, 3, 3);
            this.layoutControlGroup1.Size = new System.Drawing.Size(1046, 68);
            this.layoutControlGroup1.Text = "Parámetros de Vacaciones";
            // 
            // lciFechaCorte
            // 
            this.lciFechaCorte.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.lciFechaCorte.AppearanceItemCaption.Options.UseFont = true;
            this.lciFechaCorte.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciFechaCorte.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciFechaCorte.Control = this.dateFechaDesde;
            this.lciFechaCorte.CustomizationFormText = "Fecha Factura";
            this.lciFechaCorte.Location = new System.Drawing.Point(0, 0);
            this.lciFechaCorte.Name = "lciFechaCorte";
            this.lciFechaCorte.Size = new System.Drawing.Size(286, 41);
            this.lciFechaCorte.Text = "Fecha Corte";
            this.lciFechaCorte.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciFechaCorte.TextSize = new System.Drawing.Size(113, 20);
            this.lciFechaCorte.TextToControlDistance = 5;
            // 
            // lciLoad
            // 
            this.lciLoad.Control = this.btnLoad;
            this.lciLoad.CustomizationFormText = "lciLoad";
            this.lciLoad.Location = new System.Drawing.Point(531, 0);
            this.lciLoad.Name = "lciLoad";
            this.lciLoad.Size = new System.Drawing.Size(100, 41);
            this.lciLoad.Text = "lciLoad";
            this.lciLoad.TextSize = new System.Drawing.Size(0, 0);
            this.lciLoad.TextToControlDistance = 0;
            this.lciLoad.TextVisible = false;
            // 
            // lciClose
            // 
            this.lciClose.Control = this.btnClose;
            this.lciClose.CustomizationFormText = "lciClose";
            this.lciClose.Location = new System.Drawing.Point(729, 0);
            this.lciClose.Name = "lciClose";
            this.lciClose.Size = new System.Drawing.Size(73, 41);
            this.lciClose.Text = "lciClose";
            this.lciClose.TextSize = new System.Drawing.Size(0, 0);
            this.lciClose.TextToControlDistance = 0;
            this.lciClose.TextVisible = false;
            // 
            // lciExcel
            // 
            this.lciExcel.Control = this.btnExcel;
            this.lciExcel.CustomizationFormText = "lciExcel";
            this.lciExcel.Location = new System.Drawing.Point(917, 0);
            this.lciExcel.Name = "lciExcel";
            this.lciExcel.Size = new System.Drawing.Size(121, 41);
            this.lciExcel.Text = "lciExcel";
            this.lciExcel.TextSize = new System.Drawing.Size(0, 0);
            this.lciExcel.TextToControlDistance = 0;
            this.lciExcel.TextVisible = false;
            // 
            // lciPDF
            // 
            this.lciPDF.Control = this.btnPDF;
            this.lciPDF.CustomizationFormText = "Exp. PFD";
            this.lciPDF.Location = new System.Drawing.Point(802, 0);
            this.lciPDF.Name = "lciPDF";
            this.lciPDF.Size = new System.Drawing.Size(115, 41);
            this.lciPDF.Text = "Exp. PFD";
            this.lciPDF.TextSize = new System.Drawing.Size(0, 0);
            this.lciPDF.TextToControlDistance = 0;
            this.lciPDF.TextVisible = false;
            // 
            // lciES
            // 
            this.lciES.AppearanceItemCaption.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.lciES.AppearanceItemCaption.Options.UseFont = true;
            this.lciES.AppearanceItemCaption.Options.UseTextOptions = true;
            this.lciES.AppearanceItemCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.lciES.Control = this.lePlanilla;
            this.lciES.CustomizationFormText = "P";
            this.lciES.Location = new System.Drawing.Point(286, 0);
            this.lciES.Name = "lciES";
            this.lciES.Size = new System.Drawing.Size(245, 41);
            this.lciES.Text = "Planilla";
            this.lciES.TextAlignMode = DevExpress.XtraLayout.TextAlignModeItem.CustomSize;
            this.lciES.TextSize = new System.Drawing.Size(80, 30);
            this.lciES.TextToControlDistance = 5;
            // 
            // lciPreview
            // 
            this.lciPreview.Control = this.btnPreview;
            this.lciPreview.CustomizationFormText = "lciPreview";
            this.lciPreview.Location = new System.Drawing.Point(631, 0);
            this.lciPreview.Name = "lciPreview";
            this.lciPreview.Size = new System.Drawing.Size(98, 41);
            this.lciPreview.Text = "lciPreview";
            this.lciPreview.TextSize = new System.Drawing.Size(0, 0);
            this.lciPreview.TextToControlDistance = 0;
            this.lciPreview.TextVisible = false;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.splitContainerControl1);
            this.panelControl1.Controls.Add(this.layoutControlTop);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.LookAndFeel.SkinName = "McSkin";
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1050, 378);
            this.panelControl1.TabIndex = 0;
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(2, 70);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.grid);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.PanelVisibility = DevExpress.XtraEditors.SplitPanelVisibility.Panel1;
            this.splitContainerControl1.Size = new System.Drawing.Size(1046, 306);
            this.splitContainerControl1.SplitterPosition = 698;
            this.splitContainerControl1.TabIndex = 9;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // grid
            // 
            this.grid.Cursor = System.Windows.Forms.Cursors.Default;
            this.grid.DataSource = this.bdsDetalle;
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
            this.grid.Location = new System.Drawing.Point(0, 0);
            this.grid.MainView = this.gvData;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lkrEmpresa,
            this.chrSelectedSucursal,
            this.rspValor,
            this.chkSelected});
            this.grid.Size = new System.Drawing.Size(1046, 306);
            this.grid.TabIndex = 13;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // bdsDetalle
            // 
            this.bdsDetalle.AllowNew = true;
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn6,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.AutoPopulateColumns = false;
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsCustomization.AllowColumnMoving = false;
            this.gvData.OptionsCustomization.AllowGroup = false;
            this.gvData.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvData.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gvData.OptionsFilter.AllowFilterEditor = false;
            this.gvData.OptionsFilter.AllowMRUFilterList = false;
            this.gvData.OptionsMenu.EnableColumnMenu = false;
            this.gvData.OptionsMenu.EnableFooterMenu = false;
            this.gvData.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.OptionsView.ShowFooter = true;
            this.gvData.OptionsView.ShowGroupPanel = false;
            this.gvData.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn3, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn2, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "gridColumn1";
            this.gridColumn1.FieldName = "ID";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridColumn2.AppearanceCell.Options.UseBackColor = true;
            this.gridColumn2.Caption = "Trabajador";
            this.gridColumn2.FieldName = "Empleado";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 119;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Planilla ";
            this.gridColumn3.FieldName = "Planilla";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            this.gridColumn3.Width = 112;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Fecha Ingreso";
            this.gridColumn6.FieldName = "FechaIngreso";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 4;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Fecha Corte";
            this.gridColumn4.FieldName = "Fecha";
            this.gridColumn4.Name = "gridColumn4";
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn5.AppearanceCell.Options.UseFont = true;
            this.gridColumn5.Caption = "Días Acumulados";
            this.gridColumn5.DisplayFormat.FormatString = "n2";
            this.gridColumn5.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn5.FieldName = "Dias";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Dias", "{0:N2}")});
            this.gridColumn5.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 6;
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Salario Diario";
            this.gridColumn7.DisplayFormat.FormatString = "N2";
            this.gridColumn7.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn7.FieldName = "SalarioDiario";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 5;
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Monto";
            this.gridColumn8.DisplayFormat.FormatString = "n2";
            this.gridColumn8.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn8.FieldName = "gridColumn8";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "gridColumn8", "{0:N2}")});
            this.gridColumn8.UnboundExpression = "Round([SalarioDiario] * [Dias], 2)";
            this.gridColumn8.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 7;
            // 
            // gridColumn9
            // 
            this.gridColumn9.Caption = "Código";
            this.gridColumn9.FieldName = "Codigo";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 0;
            this.gridColumn9.Width = 111;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "Cargo";
            this.gridColumn10.FieldName = "Cargo";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 2;
            this.gridColumn10.Width = 93;
            // 
            // lkrEmpresa
            // 
            this.lkrEmpresa.AutoHeight = false;
            this.lkrEmpresa.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkrEmpresa.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Empresa")});
            this.lkrEmpresa.Name = "lkrEmpresa";
            this.lkrEmpresa.NullText = "<No tiene Empresa Asignada>";
            // 
            // chrSelectedSucursal
            // 
            this.chrSelectedSucursal.AutoHeight = false;
            this.chrSelectedSucursal.Caption = "Check";
            this.chrSelectedSucursal.Name = "chrSelectedSucursal";
            // 
            // rspValor
            // 
            this.rspValor.AllowMouseWheel = false;
            this.rspValor.AutoHeight = false;
            this.rspValor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, false, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, "", null, null, true)});
            this.rspValor.Mask.EditMask = "n4";
            this.rspValor.MaxValue = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.rspValor.Name = "rspValor";
            // 
            // chkSelected
            // 
            this.chkSelected.AutoHeight = false;
            this.chkSelected.Caption = "Check";
            this.chkSelected.Name = "chkSelected";
            // 
            // errRequiredField
            // 
            this.errRequiredField.ContainerControl = this;
            // 
            // infDifES
            // 
            this.infDifES.ContainerControl = this;
            // 
            // errTC_Periodo
            // 
            this.errTC_Periodo.ContainerControl = this;
            // 
            // FormPlanVacaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 378);
            this.Controls.Add(this.panelControl1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormPlanVacaciones";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Saldo de Vacaciones";
            this.Load += new System.EventHandler(this.DialogUser_Load);
            this.Shown += new System.EventHandler(this.DialogCompras_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlTop)).EndInit();
            this.layoutControlTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.lePlanilla.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaDesde.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dateFechaDesde.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciFechaCorte)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciLoad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciExcel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPDF)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lciPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsDetalle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrEmpresa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chrSelectedSucursal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rspValor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chkSelected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errRequiredField)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.infDifES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errTC_Periodo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraLayout.LayoutControl layoutControlTop;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errRequiredField;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider infDifES;
        private DevExpress.XtraEditors.DateEdit dateFechaDesde;
        private DevExpress.XtraLayout.LayoutControlItem lciFechaCorte;
        private System.Windows.Forms.BindingSource bdsDetalle;
        private DevExpress.XtraEditors.DXErrorProvider.DXErrorProvider errTC_Periodo;
        public DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
        private DevExpress.XtraEditors.SimpleButton btnLoad;
        private DevExpress.XtraLayout.LayoutControlItem lciLoad;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chkSelected;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkrEmpresa;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit chrSelectedSucursal;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rspValor;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraLayout.LayoutControlItem lciClose;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.SimpleButton btnPDF;
        private DevExpress.XtraEditors.SimpleButton btnExcel;
        private DevExpress.XtraLayout.LayoutControlItem lciExcel;
        private DevExpress.XtraLayout.LayoutControlItem lciPDF;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraEditors.LookUpEdit lePlanilla;
        private DevExpress.XtraLayout.LayoutControlItem lciES;
        private DevExpress.XtraEditors.SimpleButton btnPreview;
        private DevExpress.XtraLayout.LayoutControlItem lciPreview;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
    }
}