namespace SAGAS.Ventas.Forms
{
    partial class FormClientes
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormClientes));
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colRuc = new DevExpress.XtraGrid.Columns.GridColumn();
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
            this.colAplicaIVA = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colAplicaAlcaldia = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkTipoCliente = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.estPago = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCreditoLub = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colActivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rchkActivo = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.lkRetencion = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.rpClaseID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.splitContainerControlCenter = new DevExpress.XtraEditors.SplitContainerControl();
            this.gridES = new DevExpress.XtraGrid.GridControl();
            this.gvDataES = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colIDES = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombreES = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemLookUpEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkDepartamento)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkTipoCliente)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkRetencion)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpClaseID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlCenter)).BeginInit();
            this.splitContainerControlCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).BeginInit();
            this.SuspendLayout();
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
            this.grid.MenuManager = this.barManager;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lkTipoCliente,
            this.lkDepartamento,
            this.lkRetencion,
            this.rpClaseID,
            this.rchkActivo,
            this.repositoryItemCheckEdit1});
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
            this.colAplicaIVA,
            this.colAplicaAlcaldia,
            this.gridColumn3,
            this.gridColumn1,
            this.gridColumn2,
            this.estPago,
            this.colCreditoLub,
            this.colActivo,
            this.gridColumn4});
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
            // colNombreComercial
            // 
            this.colNombreComercial.Caption = "Razon Social";
            this.colNombreComercial.FieldName = "RazonSocial";
            this.colNombreComercial.Name = "colNombreComercial";
            this.colNombreComercial.Visible = true;
            this.colNombreComercial.VisibleIndex = 6;
            this.colNombreComercial.Width = 119;
            // 
            // colContacto
            // 
            this.colContacto.Caption = "Contactos";
            this.colContacto.FieldName = "Contactos";
            this.colContacto.Name = "colContacto";
            this.colContacto.Visible = true;
            this.colContacto.VisibleIndex = 7;
            // 
            // colDireccion
            // 
            this.colDireccion.Caption = "Dirección";
            this.colDireccion.FieldName = "Direccion";
            this.colDireccion.Name = "colDireccion";
            this.colDireccion.Visible = true;
            this.colDireccion.VisibleIndex = 8;
            // 
            // colDepartamento
            // 
            this.colDepartamento.Caption = "Departamento";
            this.colDepartamento.ColumnEdit = this.lkDepartamento;
            this.colDepartamento.FieldName = "DepartamentoID";
            this.colDepartamento.Name = "colDepartamento";
            this.colDepartamento.Visible = true;
            this.colDepartamento.VisibleIndex = 9;
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
            this.colTelefono1.VisibleIndex = 11;
            // 
            // colTelefono2
            // 
            this.colTelefono2.Caption = "Telefono 2";
            this.colTelefono2.FieldName = "Telefono2";
            this.colTelefono2.Name = "colTelefono2";
            this.colTelefono2.Visible = true;
            this.colTelefono2.VisibleIndex = 12;
            // 
            // colTelefono3
            // 
            this.colTelefono3.Caption = "Telefono 3";
            this.colTelefono3.FieldName = "Telefono3";
            this.colTelefono3.Name = "colTelefono3";
            this.colTelefono3.Visible = true;
            this.colTelefono3.VisibleIndex = 13;
            // 
            // colEmail
            // 
            this.colEmail.Caption = "Correo";
            this.colEmail.FieldName = "Email";
            this.colEmail.Name = "colEmail";
            this.colEmail.Visible = true;
            this.colEmail.VisibleIndex = 14;
            // 
            // ColWebsite
            // 
            this.ColWebsite.Caption = "Sitio Web";
            this.ColWebsite.FieldName = "WebSite";
            this.ColWebsite.Name = "ColWebsite";
            this.ColWebsite.Visible = true;
            this.ColWebsite.VisibleIndex = 15;
            // 
            // colLimiteCredito
            // 
            this.colLimiteCredito.Caption = "LimiteCredito";
            this.colLimiteCredito.DisplayFormat.FormatString = "N2";
            this.colLimiteCredito.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colLimiteCredito.FieldName = "LimiteCredito";
            this.colLimiteCredito.Name = "colLimiteCredito";
            this.colLimiteCredito.Visible = true;
            this.colLimiteCredito.VisibleIndex = 16;
            // 
            // colPlazo
            // 
            this.colPlazo.Caption = "Plazo";
            this.colPlazo.FieldName = "Plazo";
            this.colPlazo.Name = "colPlazo";
            this.colPlazo.Visible = true;
            this.colPlazo.VisibleIndex = 17;
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
            // colAplicaIVA
            // 
            this.colAplicaIVA.Caption = "Exento IVA";
            this.colAplicaIVA.FieldName = "ExentoIVA";
            this.colAplicaIVA.Name = "colAplicaIVA";
            this.colAplicaIVA.Visible = true;
            this.colAplicaIVA.VisibleIndex = 18;
            // 
            // colAplicaAlcaldia
            // 
            this.colAplicaAlcaldia.Caption = "Interes %";
            this.colAplicaAlcaldia.FieldName = "Interes";
            this.colAplicaAlcaldia.Name = "colAplicaAlcaldia";
            this.colAplicaAlcaldia.Visible = true;
            this.colAplicaAlcaldia.VisibleIndex = 19;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Descuento X Litro";
            this.gridColumn3.FieldName = "DescuentoXLitro";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 20;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Municipio";
            this.gridColumn1.FieldName = "Municipio";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 10;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Tipo Cliente";
            this.gridColumn2.ColumnEdit = this.lkTipoCliente;
            this.gridColumn2.FieldName = "TipoClienteID";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 3;
            // 
            // lkTipoCliente
            // 
            this.lkTipoCliente.AutoHeight = false;
            this.lkTipoCliente.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkTipoCliente.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Tipo de Cliente")});
            this.lkTipoCliente.DisplayMember = "Nombre";
            this.lkTipoCliente.Name = "lkTipoCliente";
            this.lkTipoCliente.ValueMember = "ID";
            // 
            // estPago
            // 
            this.estPago.Caption = "Estacion Pago";
            this.estPago.FieldName = "EstacionPago";
            this.estPago.Name = "estPago";
            this.estPago.Visible = true;
            this.estPago.VisibleIndex = 22;
            // 
            // colCreditoLub
            // 
            this.colCreditoLub.Caption = "Credito Lubricentro";
            this.colCreditoLub.ColumnEdit = this.repositoryItemCheckEdit1;
            this.colCreditoLub.FieldName = "AplicaCreditoLubricentro";
            this.colCreditoLub.Name = "colCreditoLub";
            this.colCreditoLub.Visible = true;
            this.colCreditoLub.VisibleIndex = 23;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "Check";
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
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
            this.colActivo.VisibleIndex = 21;
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
            // rpClaseID
            // 
            this.rpClaseID.AutoHeight = false;
            this.rpClaseID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpClaseID.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Clase de Proveedor")});
            this.rpClaseID.Name = "rpClaseID";
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
            this.gridES.Location = new System.Drawing.Point(0, 0);
            this.gridES.MainView = this.gvDataES;
            this.gridES.MenuManager = this.barManager;
            this.gridES.Name = "gridES";
            this.gridES.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemLookUpEdit1});
            this.gridES.Size = new System.Drawing.Size(173, 515);
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
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Realiza pago al grupo?";
            this.gridColumn4.FieldName = "RealizaPagoGrupoES";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 24;
            // 
            // FormClientes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.splitContainerControlCenter);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormClientes";
            this.Text = "Lista de Clientes";
            this.Load += new System.EventHandler(this.FormZona_Load);
            this.Controls.SetChildIndex(this.splitContainerControlCenter, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkDepartamento)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkTipoCliente)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkRetencion)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpClaseID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControlCenter)).EndInit();
            this.splitContainerControlCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemLookUpEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
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
        private DevExpress.XtraGrid.Columns.GridColumn colAplicaIVA;
        private DevExpress.XtraGrid.Columns.GridColumn colAplicaAlcaldia;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkTipoCliente;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkDepartamento;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkRetencion;
        private DevExpress.XtraEditors.SplitContainerControl splitContainerControlCenter;
        private DevExpress.XtraGrid.GridControl gridES;
        private DevExpress.XtraGrid.Views.Grid.GridView gvDataES;
        private DevExpress.XtraGrid.Columns.GridColumn colIDES;
        private DevExpress.XtraGrid.Columns.GridColumn colNombreES;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repositoryItemLookUpEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpClaseID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraGrid.Columns.GridColumn colActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rchkActivo;
        private DevExpress.XtraGrid.Columns.GridColumn estPago;
        private DevExpress.XtraGrid.Columns.GridColumn colCreditoLub;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
    }
}

