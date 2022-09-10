namespace SAGAS.Administracion.Forms
{
    partial class FormUsuario
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUsuario));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.gridUser = new DevExpress.XtraGrid.GridControl();
            this.gvDataUser = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLogin = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colActivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rchkActivo = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.lkrEmpresa = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
            this.btnResetearContraseña = new DevExpress.XtraBars.BarButtonItem();
            this.splitContainerControlUser = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridAccesos = new DevExpress.XtraGrid.GridControl();
            this.gvDataAccesos = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colModuloPermitido = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colIDAccesoPermitido = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAccesoPermitido = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAgregar = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colModificar = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAnular = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colImprimir = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colExportar = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControlAccLeft = new DevExpress.XtraEditors.PanelControl();
            this.gridES = new DevExpress.XtraGrid.GridControl();
            this.gvDataES = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colIDES = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombreES = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.splitterControlAcc = new DevExpress.XtraEditors.SplitterControl();
            this.gridAccTareas = new DevExpress.XtraGrid.GridControl();
            this.gvDataTareas = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAccTareas = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panelControlTop = new DevExpress.XtraEditors.PanelControl();
            this.barCopiarUsuario = new DevExpress.XtraEditors.SimpleButton();
            this.btnResetearPass = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataUser)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrEmpresa)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlUser)).BeginInit();
            this.splitContainerControlUser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridAccesos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataAccesos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlAccLeft)).BeginInit();
            this.panelControlAccLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAccTareas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataTareas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).BeginInit();
            this.panelControlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Id = -1;
            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
            this.barButtonItem2.Id = -1;
            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // gridUser
            // 
            this.gridUser.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridUser.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gridUser.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridUser.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.gridUser.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridUser.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.gridUser.EmbeddedNavigator.Buttons.EnabledAutoRepeat = false;
            this.gridUser.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.gridUser.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridUser.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gridUser.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridUser.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gridUser.Location = new System.Drawing.Point(0, 0);
            this.gridUser.MainView = this.gvDataUser;
            this.gridUser.MenuManager = this.barManager;
            this.gridUser.Name = "gridUser";
            this.gridUser.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lkrEmpresa,
            this.rchkActivo});
            this.gridUser.Size = new System.Drawing.Size(393, 515);
            this.gridUser.TabIndex = 5;
            this.gridUser.UseEmbeddedNavigator = true;
            this.gridUser.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDataUser});
            // 
            // gvDataUser
            // 
            this.gvDataUser.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colNombre,
            this.colLogin,
            this.colActivo});
            this.gvDataUser.GridControl = this.gridUser;
            this.gvDataUser.Name = "gvDataUser";
            this.gvDataUser.OptionsBehavior.Editable = false;
            this.gvDataUser.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvDataUser.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvDataUser.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvDataUser.OptionsView.ColumnAutoWidth = false;
            this.gvDataUser.OptionsView.ShowAutoFilterRow = true;
            this.gvDataUser.OptionsView.ShowGroupPanel = false;
            this.gvDataUser.DoubleClick += new System.EventHandler(this.gvDataUser_DoubleClick);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colNombre
            // 
            this.colNombre.Caption = "Nombre";
            this.colNombre.FieldName = "Nombre";
            this.colNombre.Name = "colNombre";
            this.colNombre.Visible = true;
            this.colNombre.VisibleIndex = 0;
            this.colNombre.Width = 157;
            // 
            // colLogin
            // 
            this.colLogin.Caption = "Usuario";
            this.colLogin.FieldName = "Login";
            this.colLogin.Name = "colLogin";
            this.colLogin.Visible = true;
            this.colLogin.VisibleIndex = 1;
            this.colLogin.Width = 164;
            // 
            // colActivo
            // 
            this.colActivo.Caption = "Activo";
            this.colActivo.ColumnEdit = this.rchkActivo;
            this.colActivo.FieldName = "Activo";
            this.colActivo.Name = "colActivo";
            this.colActivo.OptionsColumn.AllowEdit = false;
            this.colActivo.OptionsColumn.ReadOnly = true;
            this.colActivo.Visible = true;
            this.colActivo.VisibleIndex = 2;
            // 
            // rchkActivo
            // 
            this.rchkActivo.AutoHeight = false;
            this.rchkActivo.Caption = "Check";
            this.rchkActivo.Name = "rchkActivo";
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
            // barButtonItem3
            // 
            this.barButtonItem3.Id = -1;
            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // btnResetearContraseña
            // 
            this.btnResetearContraseña.Caption = "Resetear Contraseña";
            this.btnResetearContraseña.Glyph = global::SAGAS.Administracion.Properties.Resources.resetPass24;
            this.btnResetearContraseña.Id = 4;
            this.btnResetearContraseña.Name = "btnResetearContraseña";
            this.btnResetearContraseña.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // splitContainerControlUser
            // 
            this.splitContainerControlUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControlUser.Location = new System.Drawing.Point(0, 39);
            this.splitContainerControlUser.Name = "splitContainerControlUser";
            this.splitContainerControlUser.Panel1.Controls.Add(this.gridUser);
            this.splitContainerControlUser.Panel1.Text = "Panel1";
            this.splitContainerControlUser.Panel2.Controls.Add(this.gridAccesos);
            this.splitContainerControlUser.Panel2.Controls.Add(this.panelControlAccLeft);
            this.splitContainerControlUser.Panel2.Controls.Add(this.panelControlTop);
            this.splitContainerControlUser.Panel2.Text = "Panel2";
            this.splitContainerControlUser.Size = new System.Drawing.Size(882, 515);
            this.splitContainerControlUser.SplitterPosition = 393;
            this.splitContainerControlUser.TabIndex = 6;
            this.splitContainerControlUser.Text = "splitContainerControl1";
            // 
            // gridAccesos
            // 
            this.gridAccesos.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridAccesos.Location = new System.Drawing.Point(229, 34);
            this.gridAccesos.MainView = this.gvDataAccesos;
            this.gridAccesos.Name = "gridAccesos";
            this.gridAccesos.Size = new System.Drawing.Size(255, 481);
            this.gridAccesos.TabIndex = 35;
            this.gridAccesos.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDataAccesos});
            // 
            // gvDataAccesos
            // 
            this.gvDataAccesos.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gvDataAccesos.Appearance.FooterPanel.Options.UseFont = true;
            this.gvDataAccesos.Appearance.GroupRow.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gvDataAccesos.Appearance.GroupRow.Options.UseFont = true;
            this.gvDataAccesos.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colModuloPermitido,
            this.colIDAccesoPermitido,
            this.colAccesoPermitido,
            this.colAgregar,
            this.colModificar,
            this.colAnular,
            this.colImprimir,
            this.colExportar});
            this.gvDataAccesos.GridControl = this.gridAccesos;
            this.gvDataAccesos.GroupCount = 1;
            this.gvDataAccesos.Name = "gvDataAccesos";
            this.gvDataAccesos.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDataAccesos.OptionsCustomization.AllowColumnMoving = false;
            this.gvDataAccesos.OptionsPrint.ExpandAllDetails = true;
            this.gvDataAccesos.OptionsPrint.PrintDetails = true;
            this.gvDataAccesos.OptionsPrint.PrintFilterInfo = true;
            this.gvDataAccesos.OptionsView.ColumnAutoWidth = false;
            this.gvDataAccesos.OptionsView.GroupDrawMode = DevExpress.XtraGrid.Views.Grid.GroupDrawMode.Office2003;
            this.gvDataAccesos.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden;
            this.gvDataAccesos.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gvDataAccesos.OptionsView.ShowGroupedColumns = true;
            this.gvDataAccesos.OptionsView.ShowGroupPanel = false;
            this.gvDataAccesos.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colModuloPermitido, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colAccesoPermitido, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // colModuloPermitido
            // 
            this.colModuloPermitido.Caption = "Modulo";
            this.colModuloPermitido.FieldName = "Modulo";
            this.colModuloPermitido.Name = "colModuloPermitido";
            this.colModuloPermitido.OptionsColumn.AllowEdit = false;
            // 
            // colIDAccesoPermitido
            // 
            this.colIDAccesoPermitido.FieldName = "ID";
            this.colIDAccesoPermitido.Name = "colIDAccesoPermitido";
            this.colIDAccesoPermitido.OptionsColumn.AllowEdit = false;
            this.colIDAccesoPermitido.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colIDAccesoPermitido.Width = 111;
            // 
            // colAccesoPermitido
            // 
            this.colAccesoPermitido.Caption = "Accesos Data Maestra";
            this.colAccesoPermitido.FieldName = "Acceso";
            this.colAccesoPermitido.Name = "colAccesoPermitido";
            this.colAccesoPermitido.OptionsColumn.AllowEdit = false;
            this.colAccesoPermitido.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colAccesoPermitido.Visible = true;
            this.colAccesoPermitido.VisibleIndex = 0;
            this.colAccesoPermitido.Width = 267;
            // 
            // colAgregar
            // 
            this.colAgregar.Caption = "Agregar";
            this.colAgregar.FieldName = "Agregar";
            this.colAgregar.Name = "colAgregar";
            this.colAgregar.Visible = true;
            this.colAgregar.VisibleIndex = 1;
            this.colAgregar.Width = 55;
            // 
            // colModificar
            // 
            this.colModificar.Caption = "Modificar";
            this.colModificar.FieldName = "Modificar";
            this.colModificar.Name = "colModificar";
            this.colModificar.Visible = true;
            this.colModificar.VisibleIndex = 2;
            this.colModificar.Width = 55;
            // 
            // colAnular
            // 
            this.colAnular.Caption = "Anular / Inactivar";
            this.colAnular.FieldName = "Anular";
            this.colAnular.Name = "colAnular";
            this.colAnular.Visible = true;
            this.colAnular.VisibleIndex = 3;
            this.colAnular.Width = 95;
            // 
            // colImprimir
            // 
            this.colImprimir.Caption = "Imprimir";
            this.colImprimir.FieldName = "Imprimir";
            this.colImprimir.Name = "colImprimir";
            this.colImprimir.Visible = true;
            this.colImprimir.VisibleIndex = 4;
            this.colImprimir.Width = 51;
            // 
            // colExportar
            // 
            this.colExportar.Caption = "Exportar";
            this.colExportar.FieldName = "Exportar";
            this.colExportar.Name = "colExportar";
            this.colExportar.Visible = true;
            this.colExportar.VisibleIndex = 5;
            // 
            // panelControlAccLeft
            // 
            this.panelControlAccLeft.Controls.Add(this.gridES);
            this.panelControlAccLeft.Controls.Add(this.splitterControlAcc);
            this.panelControlAccLeft.Controls.Add(this.gridAccTareas);
            this.panelControlAccLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelControlAccLeft.Location = new System.Drawing.Point(0, 34);
            this.panelControlAccLeft.Name = "panelControlAccLeft";
            this.panelControlAccLeft.Size = new System.Drawing.Size(229, 481);
            this.panelControlAccLeft.TabIndex = 36;
            // 
            // gridES
            // 
            this.gridES.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridES.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gridES.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridES.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.gridES.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridES.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.gridES.EmbeddedNavigator.Buttons.EnabledAutoRepeat = false;
            this.gridES.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.gridES.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridES.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gridES.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridES.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gridES.Location = new System.Drawing.Point(2, 2);
            this.gridES.MainView = this.gvDataES;
            this.gridES.MenuManager = this.barManager;
            this.gridES.Name = "gridES";
            this.gridES.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1});
            this.gridES.Size = new System.Drawing.Size(225, 184);
            this.gridES.TabIndex = 6;
            this.gridES.UseEmbeddedNavigator = true;
            this.gridES.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDataES});
            // 
            // gvDataES
            // 
            this.gvDataES.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colIDES,
            this.colNombreES});
            this.gvDataES.GridControl = this.gridES;
            this.gvDataES.Name = "gvDataES";
            this.gvDataES.OptionsBehavior.Editable = false;
            this.gvDataES.OptionsMenu.EnableColumnMenu = false;
            this.gvDataES.OptionsMenu.EnableFooterMenu = false;
            this.gvDataES.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDataES.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvDataES.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvDataES.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvDataES.OptionsView.ColumnAutoWidth = false;
            this.gvDataES.OptionsView.ShowGroupPanel = false;
            // 
            // colIDES
            // 
            this.colIDES.FieldName = "EstacionServicioID";
            this.colIDES.Name = "colIDES";
            // 
            // colNombreES
            // 
            this.colNombreES.Caption = "Estaciones de Servicio";
            this.colNombreES.FieldName = "Nombre";
            this.colNombreES.Name = "colNombreES";
            this.colNombreES.Visible = true;
            this.colNombreES.VisibleIndex = 0;
            this.colNombreES.Width = 157;
            // 
            // repositoryItemLookUpEdit1
            // 
            this.repositoryItemLookUpEdit1.AutoHeight = false;
            this.repositoryItemLookUpEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit1.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Empresa")});
            this.repositoryItemLookUpEdit1.Name = "repositoryItemLookUpEdit1";
            this.repositoryItemLookUpEdit1.NullText = "<No tiene Empresa Asignada>";
            // 
            // splitterControlAcc
            // 
            this.splitterControlAcc.Cursor = System.Windows.Forms.Cursors.HSplit;
            this.splitterControlAcc.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitterControlAcc.Location = new System.Drawing.Point(2, 186);
            this.splitterControlAcc.Name = "splitterControlAcc";
            this.splitterControlAcc.Size = new System.Drawing.Size(225, 5);
            this.splitterControlAcc.TabIndex = 8;
            this.splitterControlAcc.TabStop = false;
            // 
            // gridAccTareas
            // 
            this.gridAccTareas.Cursor = System.Windows.Forms.Cursors.Default;
            this.gridAccTareas.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridAccTareas.Location = new System.Drawing.Point(2, 191);
            this.gridAccTareas.MainView = this.gvDataTareas;
            this.gridAccTareas.Name = "gridAccTareas";
            this.gridAccTareas.Size = new System.Drawing.Size(225, 288);
            this.gridAccTareas.TabIndex = 36;
            this.gridAccTareas.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDataTareas});
            // 
            // gvDataTareas
            // 
            this.gvDataTareas.Appearance.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gvDataTareas.Appearance.FooterPanel.Options.UseFont = true;
            this.gvDataTareas.Appearance.GroupRow.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.gvDataTareas.Appearance.GroupRow.Options.UseFont = true;
            this.gvDataTareas.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.colAccTareas});
            this.gvDataTareas.GridControl = this.gridAccTareas;
            this.gvDataTareas.GroupCount = 1;
            this.gvDataTareas.Name = "gvDataTareas";
            this.gvDataTareas.OptionsBehavior.AutoExpandAllGroups = true;
            this.gvDataTareas.OptionsCustomization.AllowColumnMoving = false;
            this.gvDataTareas.OptionsPrint.ExpandAllDetails = true;
            this.gvDataTareas.OptionsPrint.PrintDetails = true;
            this.gvDataTareas.OptionsPrint.PrintFilterInfo = true;
            this.gvDataTareas.OptionsView.GroupDrawMode = DevExpress.XtraGrid.Views.Grid.GroupDrawMode.Office2003;
            this.gvDataTareas.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.Hidden;
            this.gvDataTareas.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
            this.gvDataTareas.OptionsView.ShowGroupedColumns = true;
            this.gvDataTareas.OptionsView.ShowGroupPanel = false;
            this.gvDataTareas.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.gridColumn1, DevExpress.Data.ColumnSortOrder.Ascending),
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colAccTareas, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Modulo";
            this.gridColumn1.FieldName = "Modulo";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            // 
            // gridColumn2
            // 
            this.gridColumn2.FieldName = "ID";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.gridColumn2.Width = 111;
            // 
            // colAccTareas
            // 
            this.colAccTareas.Caption = "Accesos Tareas Sistemas";
            this.colAccTareas.FieldName = "Acceso";
            this.colAccTareas.Name = "colAccTareas";
            this.colAccTareas.OptionsColumn.AllowEdit = false;
            this.colAccTareas.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colAccTareas.Visible = true;
            this.colAccTareas.VisibleIndex = 0;
            this.colAccTareas.Width = 600;
            // 
            // panelControlTop
            // 
            this.panelControlTop.Controls.Add(this.barCopiarUsuario);
            this.panelControlTop.Controls.Add(this.btnResetearPass);
            this.panelControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControlTop.Location = new System.Drawing.Point(0, 0);
            this.panelControlTop.Name = "panelControlTop";
            this.panelControlTop.Size = new System.Drawing.Size(484, 34);
            this.panelControlTop.TabIndex = 8;
            // 
            // barCopiarUsuario
            // 
            this.barCopiarUsuario.Dock = System.Windows.Forms.DockStyle.Left;
            this.barCopiarUsuario.Image = global::SAGAS.Administracion.Properties.Resources.User_Clients_icon1;
            this.barCopiarUsuario.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.barCopiarUsuario.Location = new System.Drawing.Point(144, 2);
            this.barCopiarUsuario.Name = "barCopiarUsuario";
            this.barCopiarUsuario.Size = new System.Drawing.Size(142, 30);
            this.barCopiarUsuario.TabIndex = 3;
            this.barCopiarUsuario.Text = "Copiar Usuario";
            this.barCopiarUsuario.Click += new System.EventHandler(this.barCopiarUsuario_Click);
            // 
            // btnResetearPass
            // 
            this.btnResetearPass.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnResetearPass.Image = global::SAGAS.Administracion.Properties.Resources.resetPass24;
            this.btnResetearPass.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleLeft;
            this.btnResetearPass.Location = new System.Drawing.Point(2, 2);
            this.btnResetearPass.Name = "btnResetearPass";
            this.btnResetearPass.Size = new System.Drawing.Size(142, 30);
            this.btnResetearPass.TabIndex = 2;
            this.btnResetearPass.Text = "Resetear Contraseña";
            this.btnResetearPass.Click += new System.EventHandler(this.btnResetearPass_Click);
            // 
            // FormUsuario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 554);
            this.Controls.Add(this.splitContainerControlUser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormUsuario";
            this.Text = "Lista de Usuarios";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.splitContainerControlUser, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataUser)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrEmpresa)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlUser)).EndInit();
            this.splitContainerControlUser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridAccesos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataAccesos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlAccLeft)).EndInit();
            this.panelControlAccLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAccTareas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataTareas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControlTop)).EndInit();
            this.panelControlTop.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl gridUser;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDataUser;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colNombre;
        private DevExpress.XtraGrid.Columns.GridColumn colLogin;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkrEmpresa;
        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
        private DevExpress.XtraBars.BarButtonItem btnResetearContraseña;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControlUser;
        private DevExpress.XtraGrid.GridControl gridES;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDataES;
        private DevExpress.XtraGrid.Columns.GridColumn colIDES;
        private DevExpress.XtraGrid.Columns.GridColumn colNombreES;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraEditors.PanelControl panelControlTop;
        private DevExpress.XtraEditors.SimpleButton btnResetearPass;
        private DevExpress.XtraGrid.GridControl gridAccesos;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDataAccesos;
        private DevExpress.XtraGrid.Columns.GridColumn colModuloPermitido;
        private DevExpress.XtraGrid.Columns.GridColumn colIDAccesoPermitido;
        private DevExpress.XtraGrid.Columns.GridColumn colAccesoPermitido;
        private DevExpress.XtraEditors.PanelControl panelControlAccLeft;
        private DevExpress.XtraEditors.SplitterControl splitterControlAcc;
        private DevExpress.XtraGrid.GridControl gridAccTareas;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDataTareas;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn colAccTareas;
        private DevExpress.XtraGrid.Columns.GridColumn colAgregar;
        private DevExpress.XtraGrid.Columns.GridColumn colModificar;
        private DevExpress.XtraGrid.Columns.GridColumn colAnular;
        private DevExpress.XtraGrid.Columns.GridColumn colImprimir;
        private DevExpress.XtraGrid.Columns.GridColumn colExportar;
        private DevExpress.XtraGrid.Columns.GridColumn colActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rchkActivo;
        private DevExpress.XtraEditors.SimpleButton barCopiarUsuario;
    }
}

