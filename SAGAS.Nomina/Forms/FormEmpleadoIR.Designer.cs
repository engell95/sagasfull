namespace SAGAS.Nomina.Forms
{
    partial class FormEmpleadoIR
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEmpleadoIR));
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombres = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colApellidos = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSalarioActual = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFechaIngreso = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkrPlanilla = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.cboGenero = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.lkGenero = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridIR = new DevExpress.XtraGrid.GridControl();
            this.bdsIR = new System.Windows.Forms.BindingSource(this.components);
            this.gvDataIR = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmpleadoID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFechaIR = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSalarioBasico = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOtrosIngresos = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colOtrosIngresosVirtual = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colINSSLaboral = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colINSSLaboralVirtual = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSueldoAcumulado = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMesesTranscurridos = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSueldoPromedioMensual = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEspectativaAnual = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colImpuestoBase = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPorcentajeAplicable = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSobreExceso = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTotal = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRetencionAcumulada = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSaldoPorRetener = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMesesPorRetener = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRetencionMes = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIRRetenidoI = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIRRetenidoII = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIRAnualSobre12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIRMesesTranscurridos = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repositoryItemLookUpEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrPlanilla)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboGenero)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkGenero)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
            this.splitContainerControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridIR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsIR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataIR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).BeginInit();
            this.SuspendLayout();
            // 
            // bdsManejadorDatos
            // 
            this.bdsManejadorDatos.DataSource = typeof(SAGAS.Entidad.VistaEmleados);
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(804, 33);
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
            this.grid.DataSource = this.bdsManejadorDatos;
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
            this.lkrPlanilla,
            this.cboGenero,
            this.lkGenero});
            this.grid.Size = new System.Drawing.Size(405, 521);
            this.grid.TabIndex = 5;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colCodigo,
            this.colNombres,
            this.colApellidos,
            this.colSalarioActual,
            this.colFechaIngreso});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.DoubleClick += new System.EventHandler(this.gvData_DoubleClick);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colCodigo
            // 
            this.colCodigo.FieldName = "Codigo";
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.Visible = true;
            this.colCodigo.VisibleIndex = 0;
            this.colCodigo.Width = 63;
            // 
            // colNombres
            // 
            this.colNombres.FieldName = "Nombres";
            this.colNombres.Name = "colNombres";
            this.colNombres.Visible = true;
            this.colNombres.VisibleIndex = 1;
            this.colNombres.Width = 90;
            // 
            // colApellidos
            // 
            this.colApellidos.FieldName = "Apellidos";
            this.colApellidos.Name = "colApellidos";
            this.colApellidos.Visible = true;
            this.colApellidos.VisibleIndex = 2;
            // 
            // colSalarioActual
            // 
            this.colSalarioActual.DisplayFormat.FormatString = "n2";
            this.colSalarioActual.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colSalarioActual.FieldName = "SalarioActual";
            this.colSalarioActual.Name = "colSalarioActual";
            this.colSalarioActual.Visible = true;
            this.colSalarioActual.VisibleIndex = 4;
            this.colSalarioActual.Width = 82;
            // 
            // colFechaIngreso
            // 
            this.colFechaIngreso.FieldName = "FechaIngreso";
            this.colFechaIngreso.Name = "colFechaIngreso";
            this.colFechaIngreso.Visible = true;
            this.colFechaIngreso.VisibleIndex = 3;
            this.colFechaIngreso.Width = 74;
            // 
            // lkrPlanilla
            // 
            this.lkrPlanilla.AutoHeight = false;
            this.lkrPlanilla.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkrPlanilla.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Planilla")});
            this.lkrPlanilla.Name = "lkrPlanilla";
            // 
            // cboGenero
            // 
            this.cboGenero.AutoHeight = false;
            this.cboGenero.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cboGenero.Items.AddRange(new object[] {
            "M",
            "F"});
            this.cboGenero.Name = "cboGenero";
            this.cboGenero.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // lkGenero
            // 
            this.lkGenero.AutoHeight = false;
            this.lkGenero.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkGenero.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Genero", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.lkGenero.DisplayMember = "Nombre";
            this.lkGenero.Name = "lkGenero";
            this.lkGenero.ValueMember = "ID";
            // 
            // splitContainerControl1
            // 
            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControl1.Location = new System.Drawing.Point(0, 33);
            this.splitContainerControl1.Name = "splitContainerControl1";
            this.splitContainerControl1.Panel1.Controls.Add(this.grid);
            this.splitContainerControl1.Panel1.Text = "Panel1";
            this.splitContainerControl1.Panel2.Controls.Add(this.gridIR);
            this.splitContainerControl1.Panel2.Text = "Panel2";
            this.splitContainerControl1.Size = new System.Drawing.Size(804, 521);
            this.splitContainerControl1.SplitterPosition = 405;
            this.splitContainerControl1.TabIndex = 6;
            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // gridIR
            // 
            this.gridIR.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridIR.DataSource = this.bdsIR;
            this.gridIR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridIR.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gridIR.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridIR.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.gridIR.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridIR.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.gridIR.EmbeddedNavigator.Buttons.EnabledAutoRepeat = false;
            this.gridIR.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.gridIR.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridIR.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gridIR.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridIR.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gridIR.Location = new System.Drawing.Point(0, 0);
            this.gridIR.MainView = this.gvDataIR;
            this.gridIR.MenuManager = this.barManager;
            this.gridIR.Name = "gridIR";
            this.gridIR.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1,
            this.repositoryItemComboBox1,
            this.repositoryItemLookUpEdit2});
            this.gridIR.Size = new System.Drawing.Size(395, 521);
            this.gridIR.TabIndex = 6;
            this.gridIR.UseEmbeddedNavigator = true;
            this.gridIR.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDataIR});
            // 
            // bdsIR
            // 
            this.bdsIR.DataSource = typeof(SAGAS.Entidad.IRRetenido);
            // 
            // gvDataIR
            // 
            this.gvDataIR.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID1,
            this.colEmpleadoID,
            this.colFechaIR,
            this.colSalarioBasico,
            this.colOtrosIngresos,
            this.colOtrosIngresosVirtual,
            this.colINSSLaboral,
            this.colINSSLaboralVirtual,
            this.colSueldoAcumulado,
            this.colMesesTranscurridos,
            this.colSueldoPromedioMensual,
            this.colEspectativaAnual,
            this.colImpuestoBase,
            this.colPorcentajeAplicable,
            this.colSobreExceso,
            this.colTotal,
            this.colRetencionAcumulada,
            this.colSaldoPorRetener,
            this.colMesesPorRetener,
            this.colRetencionMes,
            this.colIRRetenidoI,
            this.colIRRetenidoII,
            this.colIRAnualSobre12,
            this.colIRMesesTranscurridos});
            this.gvDataIR.GridControl = this.gridIR;
            this.gvDataIR.Name = "gvDataIR";
            this.gvDataIR.OptionsCustomization.AllowColumnMoving = false;
            this.gvDataIR.OptionsCustomization.AllowFilter = false;
            this.gvDataIR.OptionsCustomization.AllowGroup = false;
            this.gvDataIR.OptionsCustomization.AllowQuickHideColumns = false;
            this.gvDataIR.OptionsCustomization.AllowSort = false;
            this.gvDataIR.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvDataIR.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvDataIR.OptionsView.ColumnAutoWidth = false;
            this.gvDataIR.OptionsView.ShowGroupPanel = false;
            this.gvDataIR.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colFechaIR, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.gvDataIR.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.gvDataIR_CellValueChanged);
            // 
            // colID1
            // 
            this.colID1.FieldName = "ID";
            this.colID1.Name = "colID1";
            this.colID1.OptionsColumn.AllowEdit = false;
            this.colID1.OptionsColumn.AllowFocus = false;
            // 
            // colEmpleadoID
            // 
            this.colEmpleadoID.FieldName = "EmpleadoID";
            this.colEmpleadoID.Name = "colEmpleadoID";
            this.colEmpleadoID.OptionsColumn.AllowEdit = false;
            this.colEmpleadoID.OptionsColumn.AllowFocus = false;
            // 
            // colFechaIR
            // 
            this.colFechaIR.FieldName = "FechaIR";
            this.colFechaIR.Name = "colFechaIR";
            this.colFechaIR.OptionsColumn.AllowEdit = false;
            this.colFechaIR.OptionsColumn.AllowFocus = false;
            this.colFechaIR.Visible = true;
            this.colFechaIR.VisibleIndex = 0;
            // 
            // colSalarioBasico
            // 
            this.colSalarioBasico.DisplayFormat.FormatString = "N2";
            this.colSalarioBasico.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colSalarioBasico.FieldName = "SalarioBasico";
            this.colSalarioBasico.Name = "colSalarioBasico";
            this.colSalarioBasico.OptionsColumn.AllowEdit = false;
            this.colSalarioBasico.OptionsColumn.AllowFocus = false;
            this.colSalarioBasico.Visible = true;
            this.colSalarioBasico.VisibleIndex = 1;
            // 
            // colOtrosIngresos
            // 
            this.colOtrosIngresos.DisplayFormat.FormatString = "N2";
            this.colOtrosIngresos.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colOtrosIngresos.FieldName = "OtrosIngresos";
            this.colOtrosIngresos.Name = "colOtrosIngresos";
            this.colOtrosIngresos.OptionsColumn.AllowEdit = false;
            this.colOtrosIngresos.OptionsColumn.AllowFocus = false;
            this.colOtrosIngresos.Visible = true;
            this.colOtrosIngresos.VisibleIndex = 2;
            // 
            // colOtrosIngresosVirtual
            // 
            this.colOtrosIngresosVirtual.DisplayFormat.FormatString = "N2";
            this.colOtrosIngresosVirtual.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colOtrosIngresosVirtual.FieldName = "OtrosIngresosVirtual";
            this.colOtrosIngresosVirtual.Name = "colOtrosIngresosVirtual";
            this.colOtrosIngresosVirtual.OptionsColumn.AllowEdit = false;
            this.colOtrosIngresosVirtual.OptionsColumn.AllowFocus = false;
            // 
            // colINSSLaboral
            // 
            this.colINSSLaboral.DisplayFormat.FormatString = "N2";
            this.colINSSLaboral.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colINSSLaboral.FieldName = "INSSLaboral";
            this.colINSSLaboral.Name = "colINSSLaboral";
            this.colINSSLaboral.OptionsColumn.AllowEdit = false;
            this.colINSSLaboral.OptionsColumn.AllowFocus = false;
            this.colINSSLaboral.Visible = true;
            this.colINSSLaboral.VisibleIndex = 3;
            // 
            // colINSSLaboralVirtual
            // 
            this.colINSSLaboralVirtual.DisplayFormat.FormatString = "N2";
            this.colINSSLaboralVirtual.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colINSSLaboralVirtual.FieldName = "INSSLaboralVirtual";
            this.colINSSLaboralVirtual.Name = "colINSSLaboralVirtual";
            this.colINSSLaboralVirtual.OptionsColumn.AllowEdit = false;
            this.colINSSLaboralVirtual.OptionsColumn.AllowFocus = false;
            // 
            // colSueldoAcumulado
            // 
            this.colSueldoAcumulado.DisplayFormat.FormatString = "N2";
            this.colSueldoAcumulado.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colSueldoAcumulado.FieldName = "SueldoAcumulado";
            this.colSueldoAcumulado.Name = "colSueldoAcumulado";
            this.colSueldoAcumulado.Visible = true;
            this.colSueldoAcumulado.VisibleIndex = 4;
            // 
            // colMesesTranscurridos
            // 
            this.colMesesTranscurridos.FieldName = "MesesTranscurridos";
            this.colMesesTranscurridos.Name = "colMesesTranscurridos";
            this.colMesesTranscurridos.OptionsColumn.AllowEdit = false;
            this.colMesesTranscurridos.OptionsColumn.AllowFocus = false;
            this.colMesesTranscurridos.Visible = true;
            this.colMesesTranscurridos.VisibleIndex = 5;
            // 
            // colSueldoPromedioMensual
            // 
            this.colSueldoPromedioMensual.DisplayFormat.FormatString = "N2";
            this.colSueldoPromedioMensual.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colSueldoPromedioMensual.FieldName = "SueldoPromedioMensual";
            this.colSueldoPromedioMensual.Name = "colSueldoPromedioMensual";
            this.colSueldoPromedioMensual.Visible = true;
            this.colSueldoPromedioMensual.VisibleIndex = 6;
            // 
            // colEspectativaAnual
            // 
            this.colEspectativaAnual.DisplayFormat.FormatString = "N2";
            this.colEspectativaAnual.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colEspectativaAnual.FieldName = "EspectativaAnual";
            this.colEspectativaAnual.Name = "colEspectativaAnual";
            this.colEspectativaAnual.Visible = true;
            this.colEspectativaAnual.VisibleIndex = 7;
            // 
            // colImpuestoBase
            // 
            this.colImpuestoBase.DisplayFormat.FormatString = "N2";
            this.colImpuestoBase.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colImpuestoBase.FieldName = "ImpuestoBase";
            this.colImpuestoBase.Name = "colImpuestoBase";
            this.colImpuestoBase.Visible = true;
            this.colImpuestoBase.VisibleIndex = 8;
            // 
            // colPorcentajeAplicable
            // 
            this.colPorcentajeAplicable.DisplayFormat.FormatString = "N2";
            this.colPorcentajeAplicable.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colPorcentajeAplicable.FieldName = "PorcentajeAplicable";
            this.colPorcentajeAplicable.Name = "colPorcentajeAplicable";
            this.colPorcentajeAplicable.Visible = true;
            this.colPorcentajeAplicable.VisibleIndex = 9;
            // 
            // colSobreExceso
            // 
            this.colSobreExceso.DisplayFormat.FormatString = "N2";
            this.colSobreExceso.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colSobreExceso.FieldName = "SobreExceso";
            this.colSobreExceso.Name = "colSobreExceso";
            this.colSobreExceso.Visible = true;
            this.colSobreExceso.VisibleIndex = 10;
            // 
            // colTotal
            // 
            this.colTotal.DisplayFormat.FormatString = "N2";
            this.colTotal.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colTotal.FieldName = "Total";
            this.colTotal.Name = "colTotal";
            this.colTotal.Visible = true;
            this.colTotal.VisibleIndex = 11;
            // 
            // colRetencionAcumulada
            // 
            this.colRetencionAcumulada.DisplayFormat.FormatString = "N2";
            this.colRetencionAcumulada.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colRetencionAcumulada.FieldName = "RetencionAcumulada";
            this.colRetencionAcumulada.Name = "colRetencionAcumulada";
            this.colRetencionAcumulada.Visible = true;
            this.colRetencionAcumulada.VisibleIndex = 12;
            // 
            // colSaldoPorRetener
            // 
            this.colSaldoPorRetener.DisplayFormat.FormatString = "N2";
            this.colSaldoPorRetener.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colSaldoPorRetener.FieldName = "SaldoPorRetener";
            this.colSaldoPorRetener.Name = "colSaldoPorRetener";
            this.colSaldoPorRetener.OptionsColumn.AllowEdit = false;
            this.colSaldoPorRetener.OptionsColumn.AllowFocus = false;
            // 
            // colMesesPorRetener
            // 
            this.colMesesPorRetener.DisplayFormat.FormatString = "N2";
            this.colMesesPorRetener.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colMesesPorRetener.FieldName = "MesesPorRetener";
            this.colMesesPorRetener.Name = "colMesesPorRetener";
            this.colMesesPorRetener.OptionsColumn.AllowEdit = false;
            this.colMesesPorRetener.OptionsColumn.AllowFocus = false;
            // 
            // colRetencionMes
            // 
            this.colRetencionMes.DisplayFormat.FormatString = "N2";
            this.colRetencionMes.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colRetencionMes.FieldName = "RetencionMes";
            this.colRetencionMes.Name = "colRetencionMes";
            this.colRetencionMes.Visible = true;
            this.colRetencionMes.VisibleIndex = 13;
            // 
            // colIRRetenidoI
            // 
            this.colIRRetenidoI.DisplayFormat.FormatString = "N2";
            this.colIRRetenidoI.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colIRRetenidoI.FieldName = "IRRetenidoI";
            this.colIRRetenidoI.Name = "colIRRetenidoI";
            this.colIRRetenidoI.Visible = true;
            this.colIRRetenidoI.VisibleIndex = 14;
            // 
            // colIRRetenidoII
            // 
            this.colIRRetenidoII.DisplayFormat.FormatString = "N2";
            this.colIRRetenidoII.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colIRRetenidoII.FieldName = "IRRetenidoII";
            this.colIRRetenidoII.Name = "colIRRetenidoII";
            this.colIRRetenidoII.Visible = true;
            this.colIRRetenidoII.VisibleIndex = 15;
            // 
            // colIRAnualSobre12
            // 
            this.colIRAnualSobre12.DisplayFormat.FormatString = "N2";
            this.colIRAnualSobre12.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colIRAnualSobre12.FieldName = "IRAnualSobre12";
            this.colIRAnualSobre12.Name = "colIRAnualSobre12";
            this.colIRAnualSobre12.Visible = true;
            this.colIRAnualSobre12.VisibleIndex = 16;
            // 
            // colIRMesesTranscurridos
            // 
            this.colIRMesesTranscurridos.DisplayFormat.FormatString = "N2";
            this.colIRMesesTranscurridos.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colIRMesesTranscurridos.FieldName = "IRMesesTranscurridos";
            this.colIRMesesTranscurridos.Name = "colIRMesesTranscurridos";
            this.colIRMesesTranscurridos.Visible = true;
            this.colIRMesesTranscurridos.VisibleIndex = 17;
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Planilla")});
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "M",
            "F"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            this.repositoryItemComboBox1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // repositoryItemLookUpEdit2
            // 
            this.repositoryItemLookUpEdit2.AutoHeight = false;
            this.repositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit2.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Genero", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.repositoryItemLookUpEdit2.DisplayMember = "Nombre";
            this.repositoryItemLookUpEdit2.Name = "repositoryItemLookUpEdit2";
            this.repositoryItemLookUpEdit2.ValueMember = "ID";
            // 
            // FormEmpleadoIR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.splitContainerControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormEmpleadoIR";
            this.Text = "Retención de Empleados";
            this.Load += new System.EventHandler(this.FormSucursal_Load);
            this.Controls.SetChildIndex(this.splitContainerControl1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrPlanilla)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cboGenero)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkGenero)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
            this.splitContainerControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridIR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsIR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataIR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkrPlanilla;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colCodigo;
        private DevExpress.XtraGrid.Columns.GridColumn colNombres;
        private DevExpress.XtraGrid.Columns.GridColumn colApellidos;
        private DevExpress.XtraGrid.Columns.GridColumn colSalarioActual;
        private DevExpress.XtraGrid.Columns.GridColumn colFechaIngreso;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cboGenero;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkGenero;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
        private DevExpress.XtraGrid.GridControl gridIR;
        private System.Windows.Forms.BindingSource bdsIR;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDataIR;
        private DevExpress.XtraGrid.Columns.GridColumn colID1;
        private DevExpress.XtraGrid.Columns.GridColumn colEmpleadoID;
        private DevExpress.XtraGrid.Columns.GridColumn colFechaIR;
        private DevExpress.XtraGrid.Columns.GridColumn colSalarioBasico;
        private DevExpress.XtraGrid.Columns.GridColumn colOtrosIngresos;
        private DevExpress.XtraGrid.Columns.GridColumn colOtrosIngresosVirtual;
        private DevExpress.XtraGrid.Columns.GridColumn colINSSLaboral;
        private DevExpress.XtraGrid.Columns.GridColumn colINSSLaboralVirtual;
        private DevExpress.XtraGrid.Columns.GridColumn colSueldoAcumulado;
        private DevExpress.XtraGrid.Columns.GridColumn colMesesTranscurridos;
        private DevExpress.XtraGrid.Columns.GridColumn colSueldoPromedioMensual;
        private DevExpress.XtraGrid.Columns.GridColumn colEspectativaAnual;
        private DevExpress.XtraGrid.Columns.GridColumn colImpuestoBase;
        private DevExpress.XtraGrid.Columns.GridColumn colPorcentajeAplicable;
        private DevExpress.XtraGrid.Columns.GridColumn colSobreExceso;
        private DevExpress.XtraGrid.Columns.GridColumn colTotal;
        private DevExpress.XtraGrid.Columns.GridColumn colRetencionAcumulada;
        private DevExpress.XtraGrid.Columns.GridColumn colSaldoPorRetener;
        private DevExpress.XtraGrid.Columns.GridColumn colMesesPorRetener;
        private DevExpress.XtraGrid.Columns.GridColumn colRetencionMes;
        private DevExpress.XtraGrid.Columns.GridColumn colIRRetenidoI;
        private DevExpress.XtraGrid.Columns.GridColumn colIRRetenidoII;
        private DevExpress.XtraGrid.Columns.GridColumn colIRAnualSobre12;
        private DevExpress.XtraGrid.Columns.GridColumn colIRMesesTranscurridos;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
    }
}

