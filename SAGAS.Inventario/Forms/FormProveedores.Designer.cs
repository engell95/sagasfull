namespace SAGAS.Inventario.Forms
{
    partial class FormProveedores
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProveedores));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRuc = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colMasivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTipoProveedor = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkTipoProveedor = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpClaseID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colNombreComercial = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colContacto = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDireccion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDepartamento = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkDepartamento = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colTelefono1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTelefono2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTelefono3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmail = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColWebsite = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLimiteCredito = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPlazo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodigoCuenta = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombreCuenta = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAplicaRetencion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRetencion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAplicaIVA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAplicaAlcaldia = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colActivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rchkActivo = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.lkRetencion = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.splitContainerControlCenter = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridES = new DevExpress.XtraGrid.GridControl();
            this.gvDataES = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colIDES = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombreES = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridAlcaldia = new DevExpress.XtraGrid.GridControl();
            this.gvDataAlcaldia = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkTipoProveedor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpClaseID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkDepartamento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkRetencion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlCenter)).BeginInit();
            this.splitContainerControlCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAlcaldia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataAlcaldia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).BeginInit();
            this.SuspendLayout();
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
            this.lkTipoProveedor,
            this.lkDepartamento,
            this.lkRetencion,
            this.rpClaseID,
            this.rchkActivo});
            this.grid.Size = new System.Drawing.Size(626, 515);
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
            this.colNombre,
            this.colRuc,
            this.colMasivo,
            this.colTipoProveedor,
            this.gridColumn3,
            this.colNombreComercial,
            this.colContacto,
            this.colDireccion,
            this.colDepartamento,
            this.colTelefono1,
            this.colTelefono2,
            this.colTelefono3,
            this.colEmail,
            this.ColWebsite,
            this.colLimiteCredito,
            this.colPlazo,
            this.colCodigoCuenta,
            this.colNombreCuenta,
            this.colAplicaRetencion,
            this.colRetencion,
            this.colAplicaIVA,
            this.colAplicaAlcaldia,
            this.colActivo});
            this.gvData.GridControl = this.grid;
            this.gvData.Name = "gvData";
            this.gvData.OptionsBehavior.Editable = false;
            this.gvData.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvData.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvData.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvData.OptionsView.ColumnAutoWidth = false;
            this.gvData.OptionsView.ShowAutoFilterRow = true;
            this.gvData.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gvData_FocusedRowChanged);
            // 
            // colID
            // 
            this.colID.FieldName = "ID";
            this.colID.Name = "colID";
            // 
            // colCodigo
            // 
            this.colCodigo.Caption = "Código";
            this.colCodigo.FieldName = "Codigo";
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.Visible = true;
            this.colCodigo.VisibleIndex = 0;
            this.colCodigo.Width = 86;
            // 
            // colNombre
            // 
            this.colNombre.Caption = "Nombre";
            this.colNombre.FieldName = "Nombre";
            this.colNombre.Name = "colNombre";
            this.colNombre.Visible = true;
            this.colNombre.VisibleIndex = 1;
            this.colNombre.Width = 164;
            // 
            // colRuc
            // 
            this.colRuc.Caption = "RUC / Cedula";
            this.colRuc.FieldName = "RUC";
            this.colRuc.Name = "colRuc";
            this.colRuc.Visible = true;
            this.colRuc.VisibleIndex = 2;
            // 
            // colMasivo
            // 
            this.colMasivo.Caption = "Es Masivo";
            this.colMasivo.FieldName = "PagoMasivo";
            this.colMasivo.Name = "colMasivo";
            this.colMasivo.Visible = true;
            this.colMasivo.VisibleIndex = 3;
            // 
            // colTipoProveedor
            // 
            this.colTipoProveedor.Caption = "Tipo de Proveedor";
            this.colTipoProveedor.ColumnEdit = this.lkTipoProveedor;
            this.colTipoProveedor.FieldName = "TipoProveedorID";
            this.colTipoProveedor.Name = "colTipoProveedor";
            this.colTipoProveedor.Visible = true;
            this.colTipoProveedor.VisibleIndex = 6;
            this.colTipoProveedor.Width = 124;
            // 
            // lkTipoProveedor
            // 
            this.lkTipoProveedor.AutoHeight = false;
            this.lkTipoProveedor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkTipoProveedor.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Tipo de Proveedor")});
            this.lkTipoProveedor.Name = "lkTipoProveedor";
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Clase de Proveedor";
            this.gridColumn3.ColumnEdit = this.rpClaseID;
            this.gridColumn3.FieldName = "ProveedorClaseID";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 7;
            // 
            // rpClaseID
            // 
            this.rpClaseID.AutoHeight = false;
            this.rpClaseID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpClaseID.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Clase de Proveedor")});
            this.rpClaseID.Name = "rpClaseID";
            // 
            // colNombreComercial
            // 
            this.colNombreComercial.Caption = "Nombre Comercial";
            this.colNombreComercial.FieldName = "NombreComercial";
            this.colNombreComercial.Name = "colNombreComercial";
            this.colNombreComercial.Visible = true;
            this.colNombreComercial.VisibleIndex = 8;
            this.colNombreComercial.Width = 119;
            // 
            // colContacto
            // 
            this.colContacto.Caption = "Contacto";
            this.colContacto.FieldName = "Contacto";
            this.colContacto.Name = "colContacto";
            this.colContacto.Visible = true;
            this.colContacto.VisibleIndex = 9;
            // 
            // colDireccion
            // 
            this.colDireccion.Caption = "Dirección";
            this.colDireccion.FieldName = "Direccion";
            this.colDireccion.Name = "colDireccion";
            this.colDireccion.Visible = true;
            this.colDireccion.VisibleIndex = 10;
            // 
            // colDepartamento
            // 
            this.colDepartamento.Caption = "Departamento";
            this.colDepartamento.ColumnEdit = this.lkDepartamento;
            this.colDepartamento.FieldName = "DepartamentoID";
            this.colDepartamento.Name = "colDepartamento";
            this.colDepartamento.Visible = true;
            this.colDepartamento.VisibleIndex = 11;
            this.colDepartamento.Width = 100;
            // 
            // lkDepartamento
            // 
            this.lkDepartamento.AutoHeight = false;
            this.lkDepartamento.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkDepartamento.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Departamentos")});
            this.lkDepartamento.Name = "lkDepartamento";
            // 
            // colTelefono1
            // 
            this.colTelefono1.Caption = "Telefono 1";
            this.colTelefono1.FieldName = "Telefono1";
            this.colTelefono1.Name = "colTelefono1";
            this.colTelefono1.Visible = true;
            this.colTelefono1.VisibleIndex = 12;
            // 
            // colTelefono2
            // 
            this.colTelefono2.Caption = "Telefono 2";
            this.colTelefono2.FieldName = "Telefono2";
            this.colTelefono2.Name = "colTelefono2";
            this.colTelefono2.Visible = true;
            this.colTelefono2.VisibleIndex = 13;
            // 
            // colTelefono3
            // 
            this.colTelefono3.Caption = "Telefono 3";
            this.colTelefono3.FieldName = "Telefono3";
            this.colTelefono3.Name = "colTelefono3";
            this.colTelefono3.Visible = true;
            this.colTelefono3.VisibleIndex = 14;
            // 
            // colEmail
            // 
            this.colEmail.Caption = "Correo";
            this.colEmail.FieldName = "Email";
            this.colEmail.Name = "colEmail";
            this.colEmail.Visible = true;
            this.colEmail.VisibleIndex = 15;
            // 
            // ColWebsite
            // 
            this.ColWebsite.Caption = "Sitio Web";
            this.ColWebsite.FieldName = "Website";
            this.ColWebsite.Name = "ColWebsite";
            this.ColWebsite.Visible = true;
            this.ColWebsite.VisibleIndex = 16;
            // 
            // colLimiteCredito
            // 
            this.colLimiteCredito.Caption = "LimiteCredito";
            this.colLimiteCredito.FieldName = "LimiteCredito";
            this.colLimiteCredito.Name = "colLimiteCredito";
            this.colLimiteCredito.Visible = true;
            this.colLimiteCredito.VisibleIndex = 17;
            // 
            // colPlazo
            // 
            this.colPlazo.Caption = "Plazo";
            this.colPlazo.FieldName = "Plazo";
            this.colPlazo.Name = "colPlazo";
            this.colPlazo.Visible = true;
            this.colPlazo.VisibleIndex = 18;
            // 
            // colCodigoCuenta
            // 
            this.colCodigoCuenta.Caption = "Codigo de Cuenta";
            this.colCodigoCuenta.FieldName = "CuentaCodigo";
            this.colCodigoCuenta.Name = "colCodigoCuenta";
            this.colCodigoCuenta.Visible = true;
            this.colCodigoCuenta.VisibleIndex = 4;
            this.colCodigoCuenta.Width = 104;
            // 
            // colNombreCuenta
            // 
            this.colNombreCuenta.Caption = "Nombre de Cuenta";
            this.colNombreCuenta.FieldName = "CuentaNombre";
            this.colNombreCuenta.Name = "colNombreCuenta";
            this.colNombreCuenta.Visible = true;
            this.colNombreCuenta.VisibleIndex = 5;
            this.colNombreCuenta.Width = 160;
            // 
            // colAplicaRetencion
            // 
            this.colAplicaRetencion.Caption = "Aplica Retención";
            this.colAplicaRetencion.FieldName = "AplicaRetencion";
            this.colAplicaRetencion.Name = "colAplicaRetencion";
            this.colAplicaRetencion.Visible = true;
            this.colAplicaRetencion.VisibleIndex = 19;
            this.colAplicaRetencion.Width = 116;
            // 
            // colRetencion
            // 
            this.colRetencion.Caption = "Cta. Retención";
            this.colRetencion.FieldName = "ImpuestoRetencion";
            this.colRetencion.Name = "colRetencion";
            this.colRetencion.Visible = true;
            this.colRetencion.VisibleIndex = 20;
            this.colRetencion.Width = 150;
            // 
            // colAplicaIVA
            // 
            this.colAplicaIVA.Caption = "Aplica IVA";
            this.colAplicaIVA.FieldName = "AplicaIVA";
            this.colAplicaIVA.Name = "colAplicaIVA";
            this.colAplicaIVA.Visible = true;
            this.colAplicaIVA.VisibleIndex = 21;
            // 
            // colAplicaAlcaldia
            // 
            this.colAplicaAlcaldia.Caption = "AplicaAlcaldia";
            this.colAplicaAlcaldia.FieldName = "AplicaAlcaldia";
            this.colAplicaAlcaldia.Name = "colAplicaAlcaldia";
            this.colAplicaAlcaldia.Visible = true;
            this.colAplicaAlcaldia.VisibleIndex = 22;
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
            this.colActivo.VisibleIndex = 23;
            // 
            // rchkActivo
            // 
            this.rchkActivo.AutoHeight = false;
            this.rchkActivo.Caption = "Check";
            this.rchkActivo.Name = "rchkActivo";
            // 
            // lkRetencion
            // 
            this.lkRetencion.AutoHeight = false;
            this.lkRetencion.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkRetencion.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Retencion")});
            this.lkRetencion.Name = "lkRetencion";
            // 
            // splitContainerControlCenter
            // 
            this.splitContainerControlCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerControlCenter.FixedPanel = DevExpress.XtraEditors.SplitFixedPanel.None;
            this.splitContainerControlCenter.Location = new System.Drawing.Point(0, 39);
            this.splitContainerControlCenter.Name = "splitContainerControlCenter";
            this.splitContainerControlCenter.Panel1.Controls.Add(this.grid);
            this.splitContainerControlCenter.Panel1.Text = "Panel1";
            this.splitContainerControlCenter.Panel2.Controls.Add(this.gridES);
            this.splitContainerControlCenter.Panel2.Controls.Add(this.gridAlcaldia);
            this.splitContainerControlCenter.Panel2.Text = "Panel2";
            this.splitContainerControlCenter.Size = new System.Drawing.Size(804, 515);
            this.splitContainerControlCenter.SplitterPosition = 626;
            this.splitContainerControlCenter.TabIndex = 6;
            this.splitContainerControlCenter.Text = "splitContainerControl1";
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
            this.gridES.Location = new System.Drawing.Point(0, 0);
            this.gridES.MainView = this.gvDataES;
            this.gridES.MenuManager = this.barManager;
            this.gridES.Name = "gridES";
            this.gridES.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1});
            this.gridES.Size = new System.Drawing.Size(173, 293);
            this.gridES.TabIndex = 8;
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
            this.colNombreES.Caption = "Masivo Estaciones de Servicio";
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
            // gridAlcaldia
            // 
            this.gridAlcaldia.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.gridAlcaldia.EmbeddedNavigator.Buttons.Append.Enabled = false;
            this.gridAlcaldia.EmbeddedNavigator.Buttons.Append.Visible = false;
            this.gridAlcaldia.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
            this.gridAlcaldia.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
            this.gridAlcaldia.EmbeddedNavigator.Buttons.Edit.Enabled = false;
            this.gridAlcaldia.EmbeddedNavigator.Buttons.EnabledAutoRepeat = false;
            this.gridAlcaldia.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
            this.gridAlcaldia.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
            this.gridAlcaldia.EmbeddedNavigator.Buttons.Remove.Enabled = false;
            this.gridAlcaldia.EmbeddedNavigator.Buttons.Remove.Visible = false;
            this.gridAlcaldia.EmbeddedNavigator.TextStringFormat = "Registro {0} de {1}";
            this.gridAlcaldia.Location = new System.Drawing.Point(0, 293);
            this.gridAlcaldia.MainView = this.gvDataAlcaldia;
            this.gridAlcaldia.MenuManager = this.barManager;
            this.gridAlcaldia.Name = "gridAlcaldia";
            this.gridAlcaldia.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit2});
            this.gridAlcaldia.Size = new System.Drawing.Size(173, 222);
            this.gridAlcaldia.TabIndex = 9;
            this.gridAlcaldia.UseEmbeddedNavigator = true;
            this.gridAlcaldia.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvDataAlcaldia});
            // 
            // gvDataAlcaldia
            // 
            this.gvDataAlcaldia.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2});
            this.gvDataAlcaldia.GridControl = this.gridAlcaldia;
            this.gvDataAlcaldia.Name = "gvDataAlcaldia";
            this.gvDataAlcaldia.OptionsBehavior.Editable = false;
            this.gvDataAlcaldia.OptionsMenu.EnableColumnMenu = false;
            this.gvDataAlcaldia.OptionsMenu.EnableFooterMenu = false;
            this.gvDataAlcaldia.OptionsMenu.EnableGroupPanelMenu = false;
            this.gvDataAlcaldia.OptionsMenu.ShowDateTimeGroupIntervalItems = false;
            this.gvDataAlcaldia.OptionsMenu.ShowGroupSortSummaryItems = false;
            this.gvDataAlcaldia.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gvDataAlcaldia.OptionsView.ColumnAutoWidth = false;
            this.gvDataAlcaldia.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn1
            // 
            this.gridColumn1.FieldName = "EstacionServicioID";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Alcaldías Estaciones de Servicio";
            this.gridColumn2.FieldName = "Nombre";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 157;
            // 
            // repositoryItemLookUpEdit2
            // 
            this.repositoryItemLookUpEdit2.AutoHeight = false;
            this.repositoryItemLookUpEdit2.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemLookUpEdit2.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Empresa")});
            this.repositoryItemLookUpEdit2.Name = "repositoryItemLookUpEdit2";
            this.repositoryItemLookUpEdit2.NullText = "<No tiene Empresa Asignada>";
            // 
            // FormProveedores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.splitContainerControlCenter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormProveedores";
            this.Text = "Lista de Proveedores";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.splitContainerControlCenter, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkTipoProveedor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpClaseID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkDepartamento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkRetencion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlCenter)).EndInit();
            this.splitContainerControlCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridAlcaldia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataAlcaldia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colTipoProveedor;
        private DevExpress.XtraGrid.Columns.GridColumn colCodigo;
        private DevExpress.XtraGrid.Columns.GridColumn colNombre;
        private DevExpress.XtraGrid.Columns.GridColumn colRuc;
        private DevExpress.XtraGrid.Columns.GridColumn colNombreComercial;
        private DevExpress.XtraGrid.Columns.GridColumn colContacto;
        private DevExpress.XtraGrid.Columns.GridColumn colDireccion;
        private DevExpress.XtraGrid.Columns.GridColumn colDepartamento;
        private DevExpress.XtraGrid.Columns.GridColumn colTelefono1;
        private DevExpress.XtraGrid.Columns.GridColumn colTelefono2;
        private DevExpress.XtraGrid.Columns.GridColumn colTelefono3;
        private DevExpress.XtraGrid.Columns.GridColumn colEmail;
        private DevExpress.XtraGrid.Columns.GridColumn ColWebsite;
        private DevExpress.XtraGrid.Columns.GridColumn colLimiteCredito;
        private DevExpress.XtraGrid.Columns.GridColumn colPlazo;
        private DevExpress.XtraGrid.Columns.GridColumn colCodigoCuenta;
        private DevExpress.XtraGrid.Columns.GridColumn colNombreCuenta;
        private DevExpress.XtraGrid.Columns.GridColumn colAplicaRetencion;
        private DevExpress.XtraGrid.Columns.GridColumn colRetencion;
        private DevExpress.XtraGrid.Columns.GridColumn colAplicaIVA;
        private DevExpress.XtraGrid.Columns.GridColumn colAplicaAlcaldia;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkTipoProveedor;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkDepartamento;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkRetencion;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControlCenter;
        private DevExpress.XtraGrid.GridControl gridES;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDataES;
        private DevExpress.XtraGrid.Columns.GridColumn colIDES;
        private DevExpress.XtraGrid.Columns.GridColumn colNombreES;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn colMasivo;
        private DevExpress.XtraGrid.GridControl gridAlcaldia;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDataAlcaldia;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpClaseID;
        private DevExpress.XtraGrid.Columns.GridColumn colActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rchkActivo;
    }
}

