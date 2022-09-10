namespace SAGAS.Administracion.Forms
{
    partial class FormEstaciones
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormEstaciones));
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
            this.grid = new DevExpress.XtraGrid.GridControl();
            this.gvData = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colNombre = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCodigo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colDireccion = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTelefono = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colZona = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkrZona = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colAdministrador = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rleAdminID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colRespCont = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rlkRespContID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colArqueador = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpArqueadorID = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rpProveedor = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colNumeroTurnos = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ColAlcaldia = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkCIA = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.lkCIP = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colActivo = new DevExpress.XtraGrid.Columns.GridColumn();
            this.rchkActivo = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.colFirma = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemMemoExEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrZona)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rleAdminID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlkRespContID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpArqueadorID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpProveedor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkCIA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkCIP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoExEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // barExportar
            // 
            this.barExportar.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("barExportar.ImageOptions.Image")));
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
            this.grid.Location = new System.Drawing.Point(0, 39);
            this.grid.MainView = this.gvData;
            this.grid.MenuManager = this.barManager;
            this.grid.Name = "grid";
            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.lkrZona,
            this.rleAdminID,
            this.rlkRespContID,
            this.rpArqueadorID,
            this.rpProveedor,
            this.lkCIP,
            this.lkCIA,
            this.rchkActivo,
            this.repositoryItemMemoExEdit1});
            this.grid.Size = new System.Drawing.Size(804, 515);
            this.grid.TabIndex = 5;
            this.grid.UseEmbeddedNavigator = true;
            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gvData});
            // 
            // gvData
            // 
            this.gvData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colID,
            this.colNombre,
            this.colCodigo,
            this.colDireccion,
            this.colTelefono,
            this.colZona,
            this.colAdministrador,
            this.colRespCont,
            this.colArqueador,
            this.gridColumn1,
            this.colNumeroTurnos,
            this.ColAlcaldia,
            this.gridColumn2,
            this.gridColumn3,
            this.colActivo,
            this.colFirma,
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
            // colNombre
            // 
            this.colNombre.Caption = "Estación de Servicio";
            this.colNombre.FieldName = "Nombre";
            this.colNombre.Name = "colNombre";
            this.colNombre.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colNombre.Visible = true;
            this.colNombre.VisibleIndex = 0;
            this.colNombre.Width = 253;
            // 
            // colCodigo
            // 
            this.colCodigo.Caption = "Código";
            this.colCodigo.FieldName = "Codigo";
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colCodigo.Visible = true;
            this.colCodigo.VisibleIndex = 1;
            this.colCodigo.Width = 145;
            // 
            // colDireccion
            // 
            this.colDireccion.Caption = "Dirección";
            this.colDireccion.FieldName = "Direccion";
            this.colDireccion.Name = "colDireccion";
            this.colDireccion.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colDireccion.Visible = true;
            this.colDireccion.VisibleIndex = 2;
            this.colDireccion.Width = 251;
            // 
            // colTelefono
            // 
            this.colTelefono.Caption = "Teléfono";
            this.colTelefono.Name = "colTelefono";
            this.colTelefono.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colTelefono.Visible = true;
            this.colTelefono.VisibleIndex = 3;
            this.colTelefono.Width = 154;
            // 
            // colZona
            // 
            this.colZona.Caption = "Zona";
            this.colZona.ColumnEdit = this.lkrZona;
            this.colZona.FieldName = "ZonaID";
            this.colZona.Name = "colZona";
            this.colZona.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colZona.Visible = true;
            this.colZona.VisibleIndex = 4;
            this.colZona.Width = 156;
            // 
            // lkrZona
            // 
            this.lkrZona.AutoHeight = false;
            this.lkrZona.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkrZona.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Nombre", "Zona", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.lkrZona.Name = "lkrZona";
            // 
            // colAdministrador
            // 
            this.colAdministrador.Caption = "Administrador";
            this.colAdministrador.ColumnEdit = this.rleAdminID;
            this.colAdministrador.FieldName = "AdministradorID";
            this.colAdministrador.Name = "colAdministrador";
            this.colAdministrador.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colAdministrador.Visible = true;
            this.colAdministrador.VisibleIndex = 5;
            this.colAdministrador.Width = 123;
            // 
            // rleAdminID
            // 
            this.rleAdminID.AutoHeight = false;
            this.rleAdminID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rleAdminID.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Administrador", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.rleAdminID.Name = "rleAdminID";
            this.rleAdminID.NullText = "<No tiene Administrador Asignado>";
            // 
            // colRespCont
            // 
            this.colRespCont.Caption = "Auxiliar / Contador";
            this.colRespCont.ColumnEdit = this.rlkRespContID;
            this.colRespCont.FieldName = "ResponsableContableID";
            this.colRespCont.Name = "colRespCont";
            this.colRespCont.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colRespCont.Visible = true;
            this.colRespCont.VisibleIndex = 6;
            this.colRespCont.Width = 115;
            // 
            // rlkRespContID
            // 
            this.rlkRespContID.AutoHeight = false;
            this.rlkRespContID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rlkRespContID.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Auxiliar / Contador", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.rlkRespContID.Name = "rlkRespContID";
            this.rlkRespContID.NullText = "<No tiene Responsable Contable Asignado>";
            // 
            // colArqueador
            // 
            this.colArqueador.Caption = "Arqueador";
            this.colArqueador.ColumnEdit = this.rpArqueadorID;
            this.colArqueador.FieldName = "ArqueadorID";
            this.colArqueador.Name = "colArqueador";
            this.colArqueador.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colArqueador.Visible = true;
            this.colArqueador.VisibleIndex = 7;
            this.colArqueador.Width = 106;
            // 
            // rpArqueadorID
            // 
            this.rpArqueadorID.AutoHeight = false;
            this.rpArqueadorID.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpArqueadorID.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Arqueador", 20, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Default, DevExpress.Data.ColumnSortOrder.Ascending)});
            this.rpArqueadorID.Name = "rpArqueadorID";
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Responsable Caja Chica";
            this.gridColumn1.ColumnEdit = this.rpProveedor;
            this.gridColumn1.FieldName = "ProveedorCajaChicaID";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 8;
            // 
            // rpProveedor
            // 
            this.rpProveedor.AutoHeight = false;
            this.rpProveedor.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.rpProveedor.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Proveedores")});
            this.rpProveedor.Name = "rpProveedor";
            // 
            // colNumeroTurnos
            // 
            this.colNumeroTurnos.Caption = "Numero de Turnos";
            this.colNumeroTurnos.FieldName = "NumeroTurnos";
            this.colNumeroTurnos.Name = "colNumeroTurnos";
            this.colNumeroTurnos.OptionsFilter.AutoFilterCondition = DevExpress.XtraGrid.Columns.AutoFilterCondition.Contains;
            this.colNumeroTurnos.Visible = true;
            this.colNumeroTurnos.VisibleIndex = 9;
            this.colNumeroTurnos.Width = 102;
            // 
            // ColAlcaldia
            // 
            this.ColAlcaldia.Caption = "Aplica Retencion Alcaldía";
            this.ColAlcaldia.FieldName = "AplicaRetencionAlcaldia";
            this.ColAlcaldia.Name = "ColAlcaldia";
            this.ColAlcaldia.Visible = true;
            this.ColAlcaldia.VisibleIndex = 10;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Cuenta Interna Activo";
            this.gridColumn2.ColumnEdit = this.lkCIA;
            this.gridColumn2.FieldName = "CuentaInternaActivo";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 11;
            this.gridColumn2.Width = 116;
            // 
            // lkCIA
            // 
            this.lkCIA.AutoHeight = false;
            this.lkCIA.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkCIA.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Cuentas")});
            this.lkCIA.DisplayMember = "Display";
            this.lkCIA.Name = "lkCIA";
            this.lkCIA.ValueMember = "ID";
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Cuenta Interna Pasivo";
            this.gridColumn3.ColumnEdit = this.lkCIP;
            this.gridColumn3.FieldName = "CuentaInternaPasivo";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 12;
            this.gridColumn3.Width = 123;
            // 
            // lkCIP
            // 
            this.lkCIP.AutoHeight = false;
            this.lkCIP.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkCIP.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("Display", "Cuentas")});
            this.lkCIP.DisplayMember = "Display";
            this.lkCIP.Name = "lkCIP";
            this.lkCIP.ValueMember = "ID";
            // 
            // colActivo
            // 
            this.colActivo.Caption = "Activo";
            this.colActivo.ColumnEdit = this.rchkActivo;
            this.colActivo.FieldName = "Activo";
            this.colActivo.Name = "colActivo";
            this.colActivo.OptionsColumn.AllowEdit = false;
            this.colActivo.OptionsColumn.ReadOnly = true;
            // 
            // rchkActivo
            // 
            this.rchkActivo.AutoHeight = false;
            this.rchkActivo.Caption = "Check";
            this.rchkActivo.Name = "rchkActivo";
            // 
            // colFirma
            // 
            this.colFirma.Caption = "Firma EECC";
            this.colFirma.ColumnEdit = this.repositoryItemMemoExEdit1;
            this.colFirma.FieldName = "FirmaEstadoCuenta";
            this.colFirma.Name = "colFirma";
            this.colFirma.Visible = true;
            this.colFirma.VisibleIndex = 13;
            this.colFirma.Width = 178;
            // 
            // repositoryItemMemoExEdit1
            // 
            this.repositoryItemMemoExEdit1.AutoHeight = false;
            this.repositoryItemMemoExEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemMemoExEdit1.Name = "repositoryItemMemoExEdit1";
            this.repositoryItemMemoExEdit1.ShowIcon = false;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Forma Costa";
            this.gridColumn4.FieldName = "FormatoCosta";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 14;
            this.gridColumn4.Width = 92;
            // 
            // FormEstaciones
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 554);
            this.Controls.Add(this.grid);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 0);
            this.Name = "FormEstaciones";
            this.Text = "Lista de Estaciones de Servicio";
            this.Load += new System.EventHandler(this.FormSucursal_Load);
            this.Controls.SetChildIndex(this.grid, 0);
            ((System.ComponentModel.ISupportInitialize)(this.bdsManejadorDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkrZona)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rleAdminID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rlkRespContID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpArqueadorID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpProveedor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkCIA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkCIP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rchkActivo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoExEdit1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
        private DevExpress.XtraGrid.GridControl grid;
        private DevExpress.XtraGrid.Views.Grid.GridView gvData;
        private DevExpress.XtraGrid.Columns.GridColumn colID;
        private DevExpress.XtraGrid.Columns.GridColumn colNombre;
        private DevExpress.XtraGrid.Columns.GridColumn colCodigo;
        private DevExpress.XtraGrid.Columns.GridColumn colDireccion;
        private DevExpress.XtraGrid.Columns.GridColumn colTelefono;
        private DevExpress.XtraGrid.Columns.GridColumn colZona;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkrZona;
        private DevExpress.XtraGrid.Columns.GridColumn colAdministrador;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rleAdminID;
        private DevExpress.XtraGrid.Columns.GridColumn colRespCont;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rlkRespContID;
        private DevExpress.XtraGrid.Columns.GridColumn colArqueador;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpArqueadorID;
        private DevExpress.XtraGrid.Columns.GridColumn colNumeroTurnos;
        private DevExpress.XtraGrid.Columns.GridColumn ColAlcaldia;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rpProveedor;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkCIA;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit lkCIP;
        private DevExpress.XtraGrid.Columns.GridColumn colActivo;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rchkActivo;
        private DevExpress.XtraGrid.Columns.GridColumn colFirma;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoExEdit repositoryItemMemoExEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
    }
}

